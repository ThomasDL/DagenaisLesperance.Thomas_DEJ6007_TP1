using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;
    public Animator playerAnim;
    private SpriteRenderer playerSprite;

    private float speed = 5.0f;
    private float horizontalInput;
    private int lookDirection;

    private float jumpForce = 11.0f;
    private bool isJumping = false;
    private float maxJumpTime = 0.2f;
    private float jumpTimer;
    private bool grounded;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        lookDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.playerActive)
        {
            HandleInput();
        }
        HandleAnimation();
    }
    private void FixedUpdate()
    {
        if (GameManager.instance.playerActive) { HandleMovement(); }
        CheckIfGrounded();
    }
    void HandleInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        if(grounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyUp(KeyCode.Space) || jumpTimer > maxJumpTime)
        {
            isJumping = false;
        }
        if (Input.GetKeyDown(KeyCode.E)&&grounded)
        {
            Interact();
        }
    }
    void HandleAnimation()
    {
        if (GameManager.instance.playerActive)
        {
            if (horizontalInput > 0.2f)
            {
                lookDirection = 1;
                playerSprite.flipX = false;

            }
            else if (horizontalInput < -0.2f)
            {
                lookDirection = -1;
                playerSprite.flipX = true;
            }
            if (grounded)
            {
                playerAnim.SetBool("IsJumping", false);
            }
            else
            {
                playerAnim.SetBool("IsJumping", true);
            }
            playerAnim.SetFloat("Speed", Mathf.Abs(playerRb.velocity.x));
        }
        else
        {
            playerAnim.SetFloat("Speed", 0);
            playerAnim.SetBool("IsJumping", false);
        } 
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
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, 0.5f, LayerMask.GetMask("Ground"));
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
    void Interact()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerRb.position + Vector2.down * 0.2f, Vector2.right*lookDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider != null)
        {
            GameManager.instance.NPCDialogue(hit.collider.name);
            return;
        }
        hit = Physics2D.Raycast(playerRb.position + Vector2.up * 0.2f, Vector2.right * lookDirection, 1.2f, LayerMask.GetMask("InteractableObject"));
        if (hit.collider != null)
        {
            Debug.Log("Object!");
            return;
        }
    }
}
