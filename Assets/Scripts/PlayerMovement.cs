using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CoinManager coinManager; // Reference to the CoinManager instance

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer; // Layer for the ground
    [SerializeField] private LayerMask wallLayer; // Layer for the walls
    [SerializeField] private float jumpPower; // Force applied when jumping

    [SerializeField] private float speed; // Player movement speed
    private Rigidbody2D body; // Rigidbody component for physics
    private Animator anim; // Animator component for animations
    private BoxCollider2D boxCollider; // BoxCollider component for collision detection
    private float wallJumpCooldown; // Cooldown timer for wall jumps
    private float horizontalInput; // Player's horizontal input

    [Header("SFX")]
    [SerializeField] private AudioClip sceneTransitionSound; // Sound for scene transitions
    [SerializeField] private AudioClip jumpSound; // Sound for jumping
    [SerializeField] private AudioClip coinSound; // Sound for collecting coins

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        SoundManager.instance.PlaySound(sceneTransitionSound); // Play the transition sound
    }

    private void Update()
    {   
        horizontalInput = Input.GetAxis("Horizontal");
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // Flip the player's sprite based on movement direction
        transform.localScale = horizontalInput > 0.01f ? new Vector3(2, 2, 2) : 
            (horizontalInput < -0.01f ? new Vector3(-2, 2, 2) : transform.localScale);

        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
        }

        // Wall jump
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0; // Disable gravity to prevent falling
                body.velocity = Vector2.zero; // Stop movement
            }
            else
            {
                body.gravityScale = 2; // Restore gravity
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();

                if (isGrounded())
                {
                    SoundManager.instance.PlaySound(jumpSound);
                }
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime; // Increment cooldown timer
        }
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower); // Apply jump force
            anim.SetTrigger("jump"); // Trigger jump animation
        }
        else if (onWall() && !isGrounded())
        {
            body.velocity = horizontalInput == 0 
                ? new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 1) // Jump off the wall if not moving horizontally
                : new Vector2(-Mathf.Sign(transform.localScale.x) * 2, 7); // Jump with horizontal push

            wallJumpCooldown = 0; // Reset cooldown
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, 
            boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null; // Return true if grounded
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, 
            boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null; // Return true if on wall
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            SoundManager.instance.PlaySound(coinSound); // Play coin collection sound
            Destroy(other.gameObject); // Destroy the collected coin
            coinManager.coinCount++; // Increment the coin count
        }
    }
}
