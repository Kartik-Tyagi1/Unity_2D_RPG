using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Flash FX")]
    [SerializeField] private Material flashMaterial;
    private Material originalMaterial;
    [SerializeField] private float flashTime;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    private IEnumerator FlashFX()
    {
        spriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(flashTime);

        spriteRenderer.material = originalMaterial;
    }

    private void StunnedRedBlink()
    {
        if(spriteRenderer.color == Color.white)
        {
            spriteRenderer.color = Color.red;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    private void CancelStunnedRedBlink()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;
    }
}
