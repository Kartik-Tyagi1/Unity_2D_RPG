using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    public void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    public void AttackTrigger()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (Collider2D enemyHit in enemiesHit)
        {
            if (enemyHit.GetComponent<Player>() != null)
            {
                enemyHit.GetComponent<Player>().Damage();
            }
        }
    }


    public void TurnOnCounterImage() => enemy.TurnOnCounterImage();
    public void TurnOffCounterImage() => enemy.TurnOffCounterImage();
    
}
