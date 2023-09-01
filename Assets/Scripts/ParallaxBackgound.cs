using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgound : MonoBehaviour
{
    private GameObject gameCamera;
    [SerializeField] private float parallaxEffect;
    private float xPosition;
    private float length;

    // Start is called before the first frame update
    void Start()
    {
        gameCamera = GameObject.Find("Main Camera");
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = gameCamera.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = gameCamera.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if(distanceMoved > xPosition + length)
        {
            xPosition += length;
        }
        else if(distanceMoved < xPosition - length)
        {
            xPosition -= length;
        }
    }
}
