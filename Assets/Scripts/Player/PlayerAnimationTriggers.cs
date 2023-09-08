using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    public void AnimationTrigger()
    {
        player.AnimationFinishTrigger();
    }

    public void AttackTrigger()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(Collider2D enemyHit in enemiesHit)
        {
            if(enemyHit.GetComponent<Enemy>() != null)
            {
                enemyHit.GetComponent<Enemy>().Damage();
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.Instance.swordSkill.CreateSword();
    }
}
