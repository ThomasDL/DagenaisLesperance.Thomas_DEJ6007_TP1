using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Le rhino hérite certaines propriétés de base du lapin.
public class RhinoController : RabbitController
{
    SpriteRenderer rhinoSr;
    float runDistance = 10f;
    bool isRunning = false;

    void Start()
    {
        thisRb = GetComponent<Rigidbody2D>();
        rhinoSr = GetComponent<SpriteRenderer>();
        thisAudioSource = GetComponent<AudioSource>();

        // Le rhino fait face à gauche par défaut, mais si le moveDirection est 1 (donc vers la droite)
        // le SpriteRenderer doit être "flippé" pour qu'il fasse face à droite.
        if (moveDirection == 1) rhinoSr.flipX = true;
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            // Si le rhino est en vie, il regarde constamment si le joueur est devant lui à une distance de 10.
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position - new Vector3(0, rhinoSr.bounds.size.y * 0.4f, 0), Vector2.right * moveDirection, runDistance, LayerMask.GetMask("Player"));
            // S'il détecte le joueur, il se met à courir sans s'arrêter.
            if (raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Player"))
            {
                isRunning = true;
            }
            if (isRunning)
            {
                thisRb.velocity = new Vector2(speed * moveDirection, thisRb.velocity.y);
            }
            // Si jamais il tombe au-delà de la limite inférieure du monde, il s'autodétruit.
            if (transform.position.y < GameManager.bottomMapLimit)
            {
                Destroy(gameObject);
            }
        }
    }
}
