using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;
    public Animator playerAnim;
    private SpriteRenderer playerSprite;

    private float speed = 5.0f;
    private float horizontalInput;
    private int lookDirection = 1;

    private float jumpForce = 11.0f;
    private bool isJumping = false;
    private float maxJumpTime = 0.2f;
    private float jumpTimer;
    private bool grounded;
    private RaycastHit2D hit;
    private bool isActive;
    private bool canInteract;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        isActive = GameManager.instance.isPlayerActive;
        HandleInput();
        HandleAnimation();
    }
    private void FixedUpdate()
    {
        CheckIfGrounded();
        HandleMovement();
        CheckForInteractions();
    }
    void HandleInput()
    {
        if (isActive)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            if (grounded && Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            if (Input.GetKeyUp(KeyCode.Space) || jumpTimer > maxJumpTime)
            {
                isJumping = false;
            }
            if (Input.GetKeyDown(KeyCode.E) && grounded)
            {
                Interact();
            }
        }
        else
        {
            horizontalInput = 0;
            isJumping = false;
        }
    }
    void HandleAnimation()
    {
        if (horizontalInput > 0.1f)
        {
            lookDirection = 1;
            playerSprite.flipX = false;

        }
        else if (horizontalInput < -0.1f)
        {
            lookDirection = -1;
            playerSprite.flipX = true;
        }
        if (grounded)
        {
            playerAnim.SetBool("IsAirborne", false);
        }
        else
        {
            playerAnim.SetBool("IsAirborne", true);
        }
        playerAnim.SetFloat("Speed", Mathf.Abs(playerRb.velocity.x));
    }
    void HandleMovement()
    {
        playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y);
        if (isJumping)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            jumpTimer += Time.deltaTime;
        }

    }
    void Jump()
    {
        isJumping = true;
        jumpTimer = 0;
    }
    void CheckIfGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size * 1.05f, 0f, Vector2.down, 0.5f, LayerMask.GetMask("Ground"));
        if (raycastHit.collider != null)
        {
            if (!grounded)
            {
                playerAnim.SetTrigger("Landing");
            }
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
    void CheckForInteractions()
    {
        hit = Physics2D.Raycast(playerRb.position, Vector2.right * lookDirection, 1.5f, LayerMask.GetMask("Interactable"));
        if (hit.collider == null && canInteract)
        {
            GameManager.instance.RemoveInteractionPrompt();
            canInteract = false;
        }
        else if (!isActive)
        {
            canInteract = false;
        }
        else if (hit.collider != null && !canInteract)
        {
            GameManager.instance.CreateInteractionPrompt("Press E to interact");
            canInteract = true;
        }

    }
    void Interact()
    {
        if (canInteract && hit.collider.CompareTag("NPC"))
        {
            hit.collider.GetComponent<DialogueSystemTrigger>().OnUse();
        }
        else if (canInteract && hit.collider.CompareTag("Object"))
        {
            Debug.Log("Object!");
        }
    }
}
