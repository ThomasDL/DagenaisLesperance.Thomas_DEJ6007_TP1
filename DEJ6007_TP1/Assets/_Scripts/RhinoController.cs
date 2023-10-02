using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoController : RabbitController
{
    SpriteRenderer rhinoSr;
    float runDistance = 10f;
    bool isRunning = false;

    void Start()
    {
        thisRb = GetComponent<Rigidbody2D>();
        rhinoSr = GetComponent<SpriteRenderer>();
        if (moveDirection == 1) rhinoSr.flipX = true;
    }

    void FixedUpdate()
    {
        if (isAlive)
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position - new Vector3(0, rhinoSr.bounds.size.y * 0.4f, 0), Vector2.right * moveDirection, runDistance, LayerMask.GetMask("Player"));
            if (raycastHit2D.collider != null && raycastHit2D.collider.CompareTag("Player"))
            {
                isRunning = true;
                Debug.Log("Yes");
            }
            if (isRunning)
            {
                thisRb.velocity = new Vector2(speed * moveDirection, thisRb.velocity.y);
            }
            if (transform.position.y < GameManager.bottomMapLimit)
            {
                Destroy(gameObject);
            }
        }
    }
}
