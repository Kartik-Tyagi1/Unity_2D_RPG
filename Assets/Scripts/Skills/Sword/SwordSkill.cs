using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum SwordType
{
    Regular, 
    Bounce
}

public class SwordSkill : SkillBase
{
    public SwordType swordType = SwordType.Regular;


    [Header("Sword Throw Parameters")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravityScale;
    private Vector2 finalDirection;

    
    [Header("Dots Parameters")]
    [SerializeField] private int numOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;
    private GameObject[] dotsArray;


    [Header("Sword Return Parameters")]
    [SerializeField] private float returnTime;


    [Header("Bounce Parameters")]
    [SerializeField] private int amountOfBounces = 4;
    [SerializeField] private float bounceGravityScale = 4;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }

    protected override void Update()
    {
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDirection = new Vector2(
                AimDirection().normalized.x * launchForce.x, AimDirection().normalized.y * launchForce.y);
        }

        if(Input.GetKey(KeyCode.Mouse1))
        {
            for(int i = 0; i < dotsArray.Length; i++)
            {
                dotsArray[i].transform.position = DotsPostion(i * spaceBetweenDots);         
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordController = newSword.GetComponent<SwordSkillController>();

        if(swordType == SwordType.Bounce)
        {
            swordGravityScale = bounceGravityScale;
            newSwordController.SetupBounceSword(true, amountOfBounces);
        }

        newSwordController.SetupSword(finalDirection, swordGravityScale, player);
        player.AssignNewSword(newSword);
        ShowDots(false);
    }

    #region Aiming

    private Vector2 AimDirection()
    {
        Vector2 playerPostion = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPostion;

        return direction;
    }

    private void GenerateDots()
    {
        dotsArray = new GameObject[numOfDots];
        for(int i = 0; i < numOfDots; i++)
        {
            dotsArray[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dotsArray[i].SetActive(false);
        }
    }

    public void ShowDots(bool _show)
    {
        for(int i = 0; i < numOfDots; i++)
        {
            dotsArray[i].SetActive(_show);
        }
    }

    private Vector2 DotsPostion(float t)
    {
        Vector2 postion = (Vector2)player.transform.position 
                            + new Vector2(
                                AimDirection().normalized.x * launchForce.x, 
                                AimDirection().normalized.y * launchForce.y
                            )
                            * t + 0.5f 
                            * (Physics2D.gravity * swordGravityScale)
                            * (t * t);    
        
        return postion;
    }

    #endregion
}
