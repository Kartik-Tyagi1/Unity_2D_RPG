using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotkeyController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hotKeyText;
    private Transform enemyTransform;
    private KeyCode keyCode;
    
    
    public void SetupHotkey(Transform _enemyTransform, KeyCode _newKeyCode)
    {
        enemyTransform = _enemyTransform;
        keyCode = _newKeyCode;
        hotKeyText.text = _newKeyCode.ToString();

        //hotkeyPrefab.transform.position = new Vector2(hotkeyPrefab.transform.position.x, hotkeyPrefab.transform.position.y + 10f);
    }

}
