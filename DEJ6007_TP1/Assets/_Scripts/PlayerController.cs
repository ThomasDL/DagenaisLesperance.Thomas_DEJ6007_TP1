using PixelCrushers.DialogueSystem;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    private BoxCollider2D playerCollider;

    private Animator playerAnim;
    private SpriteRenderer playerSpriteRenderer;

    private AudioSource playerAudioSource;
    public AudioClip jumpAudioClip;
    public AudioClip hurtAudioClip;

    private Vector3 checkpointPosition;

    private float speed;
    private const float walkSpeed = 4.5f;
    private const float runSpeed = 9.0f;
    
    private const float interactDistance = 2f;
    private bool canInteract;

    private float horizontalInput;
    private int lookDirection = 1;

    private const float jumpForce = 11.0f;
    private bool isJumping = false;
    private const float maxJumpTime = 0.2f;
    private float jumpTimer;
    private bool isGrounded;
    private bool doubleJump = true;

    private bool isActive = true;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerAudioSource = GetComponent<AudioSource>();
        speed = walkSpeed;
        checkpointPosition = transform.position;
    }

    // Le check de ground, les inputs, le check d'interactions et l'animation sont gérés par l'Update.
    void Update()
    {
        CheckIfGrounded();
        HandleInput();
        CheckForInteractions();
        HandleAnimation();
    }

    //  Le mouvement est géré par le FixedUpdate.
    private void FixedUpdate()
    {
        HandleMovement();
        
        // Si le joueur a dépassé la limite inférieur du monde, il subit un coup.
        if (transform.position.y < GameManager.bottomMapLimit) StartCoroutine(PlayerIsHit());
    }
    void HandleInput()
    {
        if (isActive)
        {
            // Les input de mouvements horizontaux sont enregistrés dans la variable horizontalInput.
            horizontalInput = Input.GetAxis("Horizontal");

            // Si le personnage est sur le sol ou qu'il n'a pas encore utilisé son double saut et que le joueur appuie sur le bouton de saut,
            // la méthode jump est appelée.
            if ((isGrounded || doubleJump) && Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            // S'il relache le bouton de saut ou que le temps de saut est écoulé, le personnage ne reçoit plus de force
            // vers le haut (voir HandleMovement()). À savoir que le joueur peut augmenter son saut
            // s'il appuie plus longtemps sur la touche de jump.
            if (Input.GetButtonUp("Jump") || jumpTimer > maxJumpTime)
            {
                isJumping = false;
            }

            // Si le joueur appuie sur le bouton Run et qu'il est sur le sol,
            // sa vitesse est augmentée à la vitesse de course.
            // Sinon et qu'il est sur le sol, elle est réduite à la vitesse de marche.
            if (Input.GetButton("Run") && isGrounded) speed = runSpeed;
            else if (isGrounded) speed = walkSpeed;
        }
        else
        {
            horizontalInput = 0;
            isJumping = false;
        }
    }
    void HandleAnimation()
    {
        // Si le joueur va vers la gauche, le sprite de son personnage est "flippé" pour qu'il fasse face à gauche.
        // Inversement s'il fait face à droite.
        if (horizontalInput < -0.1f)
        {
            lookDirection = -1;
            playerSpriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0.1f)
        {
            lookDirection = 1;
            playerSpriteRenderer.flipX = false;
        }

        // Si le joueur est sur le sol, la variable d'animation IsAirborne est fausse, sinon elle est vraie.
        playerAnim.SetBool("IsAirborne", !isGrounded);

        // La vitesse du joueur en x est envoyée à l'Animator.
        playerAnim.SetFloat("Speed", Mathf.Abs(playerRb.velocity.x));
    }
    void HandleMovement()
    {
        // La vitesse horizontale est définie par le input horizontal du joueur fois la vitesse.
        playerRb.velocity = new Vector2(horizontalInput * speed, playerRb.velocity.y);

        // Si le joueur est en train de sauter (c'est-à-dire qu'il appuie sur le bouton de saut 
        // et que le timer de saut n'est pas écoulé), sa vélocité verticale est celle définie par la force de saut.
        if (isJumping)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpForce);
            jumpTimer += Time.deltaTime;
        }
    }
    void Jump()
    {
        // S'il saute alors qu'il n'est pas au sol, son double saut est épuisé.
        if (!isGrounded) doubleJump = false;

        // La variable isJumping permet d'appliquer une force verticale sur le joueur tant qu'elle est vraie.
        isJumping = true;

        // Le timer de saut limite dans le temps l'application de la force verticale.
        jumpTimer = 0;

        // On fait jouer le son de saut.
        playerAudioSource.PlayOneShot(jumpAudioClip);
    }
    void CheckIfGrounded()
    {
        // Le jeu regarde si un objet de type sol est à environ 0.2 unités de distance du joueur. Si oui, il est sur le sol.
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size*0.95f, 0f, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));
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
        // Si un objet de type Interactable est devant le joueur, il peut interagir.
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
        else if (hit.collider != null && !canInteract && isGrounded)
        {
            // Un message apparaît à l'écran si le joueur peut interagir avec quelque chose.
            GameManager.instance.CreateInteractionPrompt("Appuie sur E pour interagir");
            canInteract = true;
        }

        // Si le joueur peut interagir avec quelque chose (voir CheckForInteractions()) et qu'il pèse sur 
        // le bouton Submit et qu'il est au sol, la méthode Interact est appelée.
        if (canInteract && Input.GetButtonDown("Submit"))
        {
            Interact(hit);
        }
    }
    void Interact(RaycastHit2D hit)
    {
        // Pour l'instant le joueur ne peut interagir qu'avec les NPCs, mais la logique est là pour interagir avec autres choses.
        if (canInteract && hit.collider.CompareTag("NPC"))
        {
            hit.collider.GetComponent<DialogueSystemTrigger>().OnUse();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le joueur atteint un checkpoint, la position checkpoint est updatée.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Checkpoint")) 
        {
            checkpointPosition = collision.transform.position;
        }

        // Si le joueur touche à des pics, il prend un coup.
        if (collision.gameObject.layer == LayerMask.NameToLayer("Spikes"))
        {
            StartCoroutine(PlayerIsHit());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out IEnemy enemy))
        {
            if (enemy.isAlive)
            {
                // Si le joueur touche un ennemi et que ses pieds sont plus hauts que son point milieu, il le tue.
                // Sinon, le joueur prend un coup.
                if (transform.position.y - collision.transform.position.y > playerSpriteRenderer.bounds.size.y / 2) StartCoroutine(enemy.EnemyDead());
                else StartCoroutine(PlayerIsHit());
            }
        }
    }
    IEnumerator PlayerIsHit()
    {
        // Si le joueur prend un coup, il perd une vie, revient au dernier checkpoint,
        // flashe rouge et ne peut pas bouger pendant 0.4 secondes.
        playerAudioSource.PlayOneShot(hurtAudioClip);
        isActive = false;
        GameManager.instance.ChangePlayerHP(-1);
        transform.position = checkpointPosition;
        playerSpriteRenderer.color = Color.red;
        playerRb.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.4f);
        if (GameManager.instance.currentHP != 0) isActive = true;
        playerSpriteRenderer.color = Color.white;
    }

    // Le personnage souscrit à l'événement PlayerActivationChange du GM. Celui-ci affecte s'il est actif ou non.
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
