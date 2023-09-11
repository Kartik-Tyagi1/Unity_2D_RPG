using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float swordReturnSpeed = 10f;
    [SerializeField] private float swordDestoryDistance = 0.5f;


    [Header("Boucing Parameters")]
    public int amountOfBounces = 4;
    public bool isBouncing = true;
    public float bouncCollisionRadius = 10f;
    public float bounceSpeed;
    public List<Transform> enemyTargets;
    private int targetIndex;


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (canRotate) transform.right = rb.velocity;

        if(isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, swordReturnSpeed * Time.deltaTime);
            if(Vector2.Distance(player.transform.position, transform.position) < swordDestoryDistance) player.CatchSword();
        }

        if(isBouncing && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[targetIndex].position, bounceSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, enemyTargets[targetIndex].position) < .1f) 
            {
                targetIndex++;
                amountOfBounces--;

                if(targetIndex >= enemyTargets.Count)
                {
                    targetIndex = 0;
                }

                if(amountOfBounces <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                    targetIndex = 0;
                }
            }
        }

    }

    public void SetupSword(Vector2 _direction, float _gravityScale, Player _player)
    {
        rb.velocity = _direction;
        rb.gravityScale = _gravityScale;
        player = _player;

        animator.SetBool(ROTATION, true);
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (isReturning) return;

        GetBouceTargets(collider2D);
        StickSwordIntoObject(collider2D);

    }

    private void StickSwordIntoObject(Collider2D collider2D)
    {

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

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, bouncCollisionRadius);
    }
}
