using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private const string ATTACK_COUNT = "AttackCount";

    // Components
    private SpriteRenderer sr;
    private Animator animator;

    // Collision
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = 0.8f;
    [SerializeField] private float enemyRadius = 5f;

    // Clone Manager Parameters
    private float cloneTimer;
    [SerializeField] private float dimAlphaSpeed;
    private Transform closestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if(cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * dimAlphaSpeed));
            if(sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Transform _clonePosition, float _cloneDuration, bool _cloneAttackSkillUnlocked)
    {
        if(_cloneAttackSkillUnlocked)
        {
            animator.SetInteger(ATTACK_COUNT, UnityEngine.Random.Range(1,3));
        }

        transform.position = _clonePosition.position;
        cloneTimer = _cloneDuration;
        FaceClosestEnemy();
    }

    public void AnimationTrigger()
    {
        cloneTimer = -1f; // Make clone time < 0 after attack, which will cause sprite to disappear 
    }

    public void AttackTrigger()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach(Collider2D enemyHit in enemiesHit)
        {
            if(enemyHit.GetComponent<Enemy>() != null)
            {
                enemyHit.GetComponent<Enemy>().Damage();
            }
        }
    }

    private void FaceClosestEnemy()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, enemyRadius);
        float closestDistance = Mathf.Infinity;

        foreach(Collider2D enemyHit in enemiesHit)
        {
            if(enemyHit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = UnityEngine.Vector2.Distance(transform.position, enemyHit.transform.position);
                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemyHit.transform;
                }
            }
        }

        if(closestEnemy != null)
        {
            if(closestEnemy.transform.position.x < transform.position.x)
            {
                transform.Rotate(0f, 180f, 0f);
            }
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
        Gizmos.DrawWireSphere(transform.position, enemyRadius);
    }
}
