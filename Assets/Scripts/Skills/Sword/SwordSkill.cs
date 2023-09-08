using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : SkillBase
{
    [Header("Skill Parameters")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDirection;
    [SerializeField] private float swordGravityScale;

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordController = newSword.GetComponent<SwordSkillController>();

        newSwordController.SetupSword(launchDirection, swordGravityScale);
    }
}
