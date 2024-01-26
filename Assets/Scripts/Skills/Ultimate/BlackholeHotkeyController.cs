using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{
    private SpriteRenderer sr;
    private TextMeshProUGUI hotKeyText;
    private Transform enemyTransform;
    private BlackholeSkillController blackholeSkillController;
    private KeyCode hotKeyCode;
    
    
    public void SetupHotkey(Transform _enemyTransform, KeyCode _newHotKeyCode, BlackholeSkillController _blackholeSkillController)
    {
        sr = GetComponent<SpriteRenderer>();
        hotKeyText = GetComponentInChildren<TextMeshProUGUI>();

        enemyTransform = _enemyTransform;
        blackholeSkillController = _blackholeSkillController;
        
        hotKeyCode = _newHotKeyCode;
        hotKeyText.text = _newHotKeyCode.ToString();

        //hotkeyPrefab.transform.position = new Vector2(hotkeyPrefab.transform.position.x, hotkeyPrefab.transform.position.y + 10f);
    }

    private void Update()
    {
        if(Input.GetKeyUp(hotKeyCode))
        {
            blackholeSkillController.AddEnemyToTargetList(enemyTransform);
            hotKeyText.color = Color.clear;
            sr.color = Color.clear;
        }
    }

}
