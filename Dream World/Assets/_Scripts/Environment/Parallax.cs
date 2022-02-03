using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private GameObject mainCamera;
    private float startPosX;
    private float startPosY;
    private float length;
    public float parallaxEffect;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distX = mainCamera.transform.position.x * parallaxEffect;
        float distY = mainCamera.transform.position.y * parallaxEffect;

        transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

        if ((transform.position.x - startPosX) > length)
        {
            transform.position += new Vector3(length,0,0);
        } else if ((startPosX-transform.position.x) > length)
        {
            transform.position -= new Vector3(length, 0, 0);
        }
    }
}
