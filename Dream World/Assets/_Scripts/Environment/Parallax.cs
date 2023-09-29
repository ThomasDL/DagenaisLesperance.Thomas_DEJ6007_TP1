using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private GameObject cam;
    private float startPosX;
    private float startPosY;
    private float length;
    public float parallaxEffect;

    void Start()
    {
        cam = GameObject.Find("CM vcam1");
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }


    void Update()
    {
        float distX = cam.transform.position.x * parallaxEffect;
        float distY = cam.transform.position.y * parallaxEffect;

        transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

        if ((cam.transform.position.x - transform.position.x) > length) 
        {
            startPosX += length;
            transform.position += new Vector3(length, 0, 0);
        }
        else if ((transform.position.x - cam.transform.position.x) > length)
        {
            startPosX -= length;
            transform.position -= new Vector3(length, 0, 0);
        }
    }
}
