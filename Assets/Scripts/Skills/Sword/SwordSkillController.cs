using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private const string ROTATION = "Rotation";

    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;


    [Header("Sword Parameters")]
    private float swordReturnSpeed;
    [SerializeField] private float swordDestoryDistance = 0.5f;
    private float freezeTimeDuration;


    [Header("Bouncing Parameters")]
    private int bounceAmount;
    private bool isBouncing;
    public float bouncCollisionRadius = 10f;
    private float bounceSpeed;
    private List<Transform> enemyTargets;
    private int targetIndex;


    [Header("Pierce Parameters")]
    [SerializeField] private int pierceAmount;


    [Header("Spin Sword")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool stoppedMoving;
    private bool isSpinning;
    private float hitCooldown;
    private float hitTimer;
    private float spinDirection;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (canRotate) transform.right = rb.velocity;

        HandleSwordReturn();
        HandleBounceSword();
        HandleSpinSword();
    }

    public void SetupSword(Vector2 _direction, float _gravityScale, Player _player, float _freezeTimeDuration, float _swordReturnSpeed)
    {
        rb.velocity = _direction;
        rb.gravityScale = _gravityScale;
        player = _player;

        freezeTimeDuration = _freezeTimeDuration;
        swordReturnSpeed = _swordReturnSpeed;

        // Only turn on sword spin animation for non-pierce sword
        if(pierceAmount <= 0) animator.SetBool(ROTATION, true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
    }

    public void SetupBounceSword(bool _isBouncing, int _amountOfBounces, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounces;
        bounceSpeed = _bounceSpeed;

        enemyTargets = new List<Transform>();
    }

    public void SetupPierceSword(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupSpinSword(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;

        
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (isReturning) return;

        if(collider2D.GetComponent<Enemy>() != null)
        {
            DamageAndFreezeEnemy(collider2D.GetComponent<Enemy>());
        }

        GetBouceTargets(collider2D);
        StickSwordIntoObject(collider2D);

    }

    private void DamageAndFreezeEnemy(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeEnemyTimer", freezeTimeDuration);
    }

    private void StickSwordIntoObject(Collider2D collider2D)
    {
        if(isSpinning) 
        {
            StopSpinSwordMovement();
            return;
        }

        if(pierceAmount > 0 && collider2D.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        // Stop rotation and shut off the collider component
        canRotate = false;
        cd.enabled = false;

        // Freeze any movement
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // stop rotation anim and attack to parent if no bouncing or if no targets otherwise keep going
        if(isBouncing && enemyTargets.Count > 0) return;

        animator.SetBool(ROTATION, false);

        // Attach the sword to what it collided with
        transform.parent = collider2D.transform;
    }

    public void ReturnSwordToPlayer()
    {
        animator.SetBool(ROTATION, true);
        
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }

    public void GetBouceTargets(Collider2D collider2D)
    {
        if(collider2D.GetComponent<Enemy>() != null)
        {
            // If sword hit an enemy and there are no enemies in the list, find enemies within overlap circle
            if(isBouncing && enemyTargets.Count <= 0)
            {
                Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bouncCollisionRadius);
                foreach(Collider2D enemy in enemies)
                {
                    if(enemy.GetComponent<Enemy>() != null)
                    {
                        enemyTargets.Add(enemy.transform);
                    }
                }
            }
        }
    }

    private void HandleBounceSword()
    {
        if (isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < .1f)
            {
                if(enemyTargets[targetIndex].GetComponent<Enemy>() != null)
                {
                    DamageAndFreezeEnemy(enemyTargets[targetIndex].GetComponent<Enemy>());
                }
                

                targetIndex++;
                bounceAmount--;

                if (targetIndex >= enemyTargets.Count)
                {
                    targetIndex = 0;
                }

                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                    targetIndex = 0;
                }
            }
        }
    }

    private void HandleSpinSword()
    {
        if (isSpinning)
        {
            // Stop sword movement when it gets to the max travel distance from player
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !stoppedMoving)
            {
                StopSpinSwordMovement();
            }

            // Let sword spin in the stopped postion for the spin duration
            if (stoppedMoving)
            {
                spinTimer -= Time.deltaTime;

                //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                // auto return sword to player when spinning duration is reached
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                // Hit the enemies around the sword spin
                SpinSwordHit();
            }
        }
    }

    private void StopSpinSwordMovement()
    {
        // Do not reset the spin timer once the movement has stopped
        if(!stoppedMoving) spinTimer = spinDuration;
        stoppedMoving = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    private void SpinSwordHit()
    {
        hitTimer -= Time.deltaTime;
        if (hitTimer < 0)
        {
            hitTimer = hitCooldown;
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bouncCollisionRadius);
            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponent<Enemy>()?.Damage();
            }
        }
    }

    private void HandleSwordReturn()
    {
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, swordReturnSpeed * Time.deltaTime);
            if (Vector2.Distance(player.transform.position, transform.position) < swordDestoryDistance) player.CatchSword();
        }
    }
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, bouncCollisionRadius);
    }
}
