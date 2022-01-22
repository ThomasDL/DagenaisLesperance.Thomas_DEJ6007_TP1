using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public Vector2 lookDirection;
    public Vector2 move;
    public Animator playerAnim;
    private float walkSpeed = 3.0f;
    RaycastHit2D hit;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        lookDirection.Set(0, -1);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        move.Set(horizontalInput, verticalInput);

        if(!Mathf.Approximately(move.x,0) || !Mathf.Approximately(move.y, 0))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        if (Input.GetKeyDown(KeyCode.Space)&& GameManager.instance.playerActive)
        {
            Interact();
        }
    }
    private void FixedUpdate()
    {
        Animate();
        if (GameManager.instance.playerActive)
        {
            playerRb.MovePosition(playerRb.position += move * Time.deltaTime * walkSpeed);
        }
    }

    void Animate()
    {
        if (GameManager.instance.playerActive)
        {
            playerAnim.SetFloat("Look X", lookDirection.x);
            playerAnim.SetFloat("Look Y", lookDirection.y);
            playerAnim.SetFloat("Speed", move.magnitude);
        } else
        {
            playerAnim.SetFloat("Speed", 0);
        }
    }
    void Interact()
    {
        hit = Physics2D.Raycast(playerRb.position + Vector2.up * 0.2f,lookDirection,1.2f,LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            GameManager.instance.NPCDialogue(hit.collider.name);
        }
        hit = Physics2D.Raycast(playerRb.position + Vector2.up * 0.2f, lookDirection, 1.2f, LayerMask.GetMask("InteractableObject"));
        if (hit.collider != null)
        {
            Debug.Log("Object!");
        }
    }
}
