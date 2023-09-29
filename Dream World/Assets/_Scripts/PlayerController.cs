using PixelCrushers.DialogueSystem;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;
    private Animator playerAnim;
    private SpriteRenderer playerSprite;

    private const float walkSpeed = 4.0f;
    private const float runSpeed = 9.0f;
    private const float bottomMapLimit = -10f;
    private float speed = 4.0f;
    private float horizontalInput;
    private int lookDirection = 1;

    private float jumpForce = 11.0f;
    private bool isJumping = false;
    private float maxJumpTime = 0.2f;
    private float jumpTimer;
    private bool isGrounded;
    private RaycastHit2D hit;
    private bool isActive;
    private bool canInteract;
    private bool doubleJump = true;

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
        CheckForInfiniteFall();
    }
    void HandleInput()
    {
        if (isActive)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            if ((isGrounded || doubleJump) && Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            if (Input.GetKeyUp(KeyCode.Space) || jumpTimer > maxJumpTime)
            {
                isJumping = false;
            }
            if (Input.GetButtonDown("Submit") && isGrounded)
            {
                Interact();
            }
            if (Input.GetButton("Fire3") && isGrounded) speed = runSpeed;
            else if(isGrounded) speed = walkSpeed;
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
        if (isGrounded)
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
        if (!isGrounded) doubleJump = false;
        isJumping = true;
        jumpTimer = 0;
    }
    void CheckIfGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size * 1.05f, 0f, Vector2.down, 0.3f, LayerMask.GetMask("Ground"));
        if (raycastHit.collider != null)
        {
            if (!isGrounded)
            {
                playerAnim.SetTrigger("Landing");
                doubleJump = true;
            }
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
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
    void CheckForInfiniteFall()
    {
        if(transform.position.y < bottomMapLimit)
        {
            StartCoroutine(PlayerIsHit());
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Spikes"))
        {
            StartCoroutine(PlayerIsHit());
        }
    }
    IEnumerator PlayerIsHit()
    {
        GameManager.instance.ChangeLifePoints(-1);
        transform.position = new Vector3(0, 0, 0);
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(0.4f);
        playerSprite.color = Color.white;
    }
}
