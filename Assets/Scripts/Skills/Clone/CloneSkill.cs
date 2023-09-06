using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : SkillBase
{
    [Header("Clone Parameters")]
    [SerializeField] private GameObject playerClonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool cloneAttackSkillUnlocked;
    

    public void CreateClone(Transform _clonePosition)
    {
        GameObject clone = Instantiate(playerClonePrefab);

        // TODO: Change cloneAttackSillUnlocked to actual skill tree value
        clone.GetComponent<CloneSkillController>().SetupClone(_clonePosition, cloneDuration, cloneAttackSkillUnlocked);
    }
}
