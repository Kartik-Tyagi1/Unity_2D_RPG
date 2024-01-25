using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private bool canGrow;
    private List<Transform> enemyTargets = new List<Transform>();
    private List<KeyCode> hotKeyList = new List<KeyCode>();

    // Update is called once per frame
    private void Update()
    {
        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        /** Freeze any enemy in the cirlce and display hotkey above them */
        collider2D.GetComponent<Enemy>()?.FreezeEnemy(true);
        CreateHotkeys(collider2D);
    }

    private void CreateHotkeys(Collider2D collider2D)
    {
        if(hotKeyList.Count <= 0)
        {
            Debug.LogWarning("Hot Keycode List is Empty");
            return;
        } 
        
        KeyCode chosenKeyCode = hotKeyList[Random.Range(0, hotKeyList.Count)];

        BlackholeHotkeyController blackholeHotkeyController = new BlackholeHotkeyController();
        blackholeHotkeyController.SetupHotkey(collider2D.transform, chosenKeyCode);
    }

    public void AddEnemyToTargetList(Transform transform) => enemyTargets.Add(transform);
}
