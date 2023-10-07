using PixelCrushers.DialogueSystem;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;
    private Animator playerAnim;
    private SpriteRenderer playerSpriteRenderer;

    private Vector3 checkpointPosition;

    private const float walkSpeed = 4.5f;
    private const float runSpeed = 9.0f;
    private const float interactDistance = 1.8f;
    private float speed;
    private float horizontalInput;
    private int lookDirection = 1;
    private float jumpForce = 11.0f;
    private bool isJumping = false;
    private float maxJumpTime = 0.2f;
    private float jumpTimer;
    private bool isGrounded;
    private bool isActive = true;
    private bool canInteract;
    private bool doubleJump = true;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        speed = walkSpeed;
        checkpointPosition = transform.position;
    }

    // Les input et l'animation sont gérés par la méthode Update.
    void Update()
    {
        HandleInput();
        HandleAnimation();
    }
    //  Le check de sol, le mouvement, le check d'interactions et le check de la limite du monde sont gérés par le FixedUpdate.
    private void FixedUpdate()
    {
        CheckIfGrounded();
        HandleMovement();
        CheckForInteractions();

        if (transform.position.y < GameManager.bottomMapLimit) StartCoroutine(PlayerIsHit());
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size * 0.9f, 0f, Vector2.down, 0.3f, LayerMask.GetMask("Ground"));
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
        RaycastHit2D hit = Physics2D.Raycast(playerRb.position, Vector2.right * lookDirection, interactDistance, LayerMask.GetMask("Interactable"));
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
            GameManager.instance.CreateInteractionPrompt("Appuie sur E pour interagir");
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Spikes"))
        {
            StartCoroutine(PlayerIsHit());
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Checkpoint")) 
        {
            checkpointPosition = collision.transform.position;
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
        isActive = false;
        GameManager.instance.ChangePlayerHP(-1);
        transform.position = checkpointPosition;
        playerSpriteRenderer.color = Color.red;
        playerRb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.4f);
        if (GameManager.instance.currentHP != 0) isActive = true;
        playerSpriteRenderer.color = Color.white;
    }
    private void OnEnable()
    {
        GameManager.playerActivationChange += PlayerActivationChange;
    }
    private void OnDisable()
    {
        GameManager.playerActivationChange -= PlayerActivationChange;
    }
    private void PlayerActivationChange(bool newIsActive)
    {
        isActive = newIsActive;
    }
}
