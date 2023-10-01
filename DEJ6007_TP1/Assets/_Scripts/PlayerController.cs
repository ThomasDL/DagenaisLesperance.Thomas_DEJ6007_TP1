using PixelCrushers.DialogueSystem;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;
    private Animator playerAnim;
    private SpriteRenderer playerSpriteRenderer;

    private const float walkSpeed = 4.5f;
    private const float runSpeed = 9.0f;
    private const float bottomMapLimit = -10f;
    private float speed;
    private float horizontalInput;
    private int lookDirection = 1;

    private float jumpForce = 11.0f;
    private bool isJumping = false;
    private float maxJumpTime = 0.2f;
    private float jumpTimer;
    private bool isGrounded;
    private bool isActive;
    private bool canInteract;
    private bool doubleJump = true;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        speed = walkSpeed;
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
            playerSpriteRenderer.flipX = false;

        }
        else if (horizontalInput < -0.1f)
        {
            lookDirection = -1;
            playerSpriteRenderer.flipX = true;
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
        RaycastHit2D hit = Physics2D.Raycast(playerRb.position, Vector2.right * lookDirection, 1.5f, LayerMask.GetMask("Interactable"));
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
        RaycastHit2D hit = Physics2D.Raycast(playerRb.position, Vector2.right * lookDirection, 1.5f, LayerMask.GetMask("Interactable"));
        if (canInteract && hit.collider.CompareTag("NPC"))
        {
            hit.collider.GetComponent<DialogueSystemTrigger>().OnUse();
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out IEnemy enemy))
        {
            if (transform.position.y - collision.transform.position.y > playerSpriteRenderer.bounds.size.y / 2) StartCoroutine(enemy.EnemyDead());
            else StartCoroutine(PlayerIsHit());
        }
    }
    IEnumerator PlayerIsHit()
    {
        GameManager.instance.isPlayerActive = false;
        GameManager.instance.ChangeLifePoints(-1);
        transform.position = new Vector3(0, 0, 0);
        playerSpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.4f);
        GameManager.instance.isPlayerActive = true;
        playerSpriteRenderer.color = Color.white;
    }
}
