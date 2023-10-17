using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Le rhino h�rite certaines propri�t�s de base du lapin.
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

        // Le rhino fait face � gauche par d�faut, mais si le moveDirection est 1 (donc vers la droite)
        // le SpriteRenderer doit �tre "flipp�" pour qu'il fasse face � droite.
        if (moveDirection == 1) rhinoSr.flipX = true;
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            // Si le rhino est en vie, il regarde constamment si le joueur est devant lui � une distance de 10.
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position - new Vector3(0, rhinoSr.bounds.size.y * 0.4f, 0), Vector2.right * moveDirection, runDistance, LayerMask.GetMask("Player"));
            // S'il d�tecte le joueur, il se met � courir sans s'arr�ter.
            if (raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Player"))
            {
                isRunning = true;
            }
            if (isRunning)
            {
                thisRb.velocity = new Vector2(speed * moveDirection, thisRb.velocity.y);
            }
            // Si jamais il tombe au-del� de la limite inf�rieure du monde, il s'autod�truit.
            if (transform.position.y < GameManager.bottomMapLimit)
            {
                Destroy(gameObject);
            }
        }
    }
}
