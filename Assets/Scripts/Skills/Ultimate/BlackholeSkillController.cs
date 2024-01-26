using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> hotKeyList;

    public float maxSize;
    public float growSpeed;
    public bool canGrow;

    private List<Transform> enemyTargets = new List<Transform>();

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
        if (collider2D.GetComponent<Enemy>() != null)
        {
            CreateHotkeys(collider2D);
        }
    }

    private void CreateHotkeys(Collider2D collider2D)
    {
        if (hotKeyList.Count <= 0)
        {
            Debug.LogWarning("Hot Keycode List is Empty");
            return;
        }

        GameObject newHotKey = Instantiate(hotkeyPrefab, collider2D.transform.position + new Vector3(0, 2), Quaternion.identity);

        KeyCode chosenKeyCode = hotKeyList[Random.Range(0, hotKeyList.Count)];
        hotKeyList.Remove(chosenKeyCode);

        BlackholeHotkeyController blackholeHotkeyController = newHotKey.GetComponent<BlackholeHotkeyController>();
        blackholeHotkeyController.SetupHotkey(collider2D.transform, chosenKeyCode, this);
    }

    public void AddEnemyToTargetList(Transform _enemyTransform) => enemyTargets.Add(_enemyTransform);
}
