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

    [SerializeField] private float swordReturnSpeed = 10f;
    [SerializeField] private float swordDestoryDistance = 0.5f;

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
        if(isReturning) return;

        animator.SetBool(ROTATION, false);

        // Stop rotation and shut off the collider component
        canRotate = false;
        cd.enabled = false;

        // Freeze any movement
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

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
}
