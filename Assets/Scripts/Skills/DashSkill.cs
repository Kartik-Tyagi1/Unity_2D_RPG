using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : SkillBase
{
    public override void UseSkill()
    {
        base.UseSkill();

        Debug.Log("Dash Skill Used");
    }
}
