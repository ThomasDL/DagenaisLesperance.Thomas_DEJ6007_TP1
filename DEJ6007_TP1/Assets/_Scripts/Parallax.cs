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
        // Cette image de fond se déplace en suivant la caméra.
        // La grandeur de ce déplacement est modulée par la variable "parallaxEffect".
        // Plus cette image est considérée comme être loin du joueur dans le monde,
        // plus elle suit la caméra et inversement si elle est proche.
        float distX = cam.transform.position.x * parallaxEffect;
        float distY = cam.transform.position.y * parallaxEffect;
        transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

        // Si jamais la distance entre la caméra et cette image dépasse sa longueur,
        // elle est déplacée d'une distance égale à sa longueur vers le joueur.
        // Comme ça, le joueur a l'impression que le fond est infini.
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
