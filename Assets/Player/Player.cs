using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;
    [SerializeField] bool showGroundCheck;

    public bool isGrounded { get; private set; }
    [SerializeField] Vector2 groundCheckOffset;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] LayerMask groundLayerMask;

    [SerializeField] ParticleSystem jumpParticle;
    [SerializeField] ParticleSystem deathParticle;

    GameManager gameManager;
    Rigidbody2D rb;
    Animator animator;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void OnDestroy()
    {
        deathParticle.transform.parent = null;
        deathParticle.Play();
        gameManager.Invoke("triggerGameOver", 1.0f);
    }

    void Update()
    {
        //Check whether the player is on the ground
        isGrounded = Physics2D.OverlapBox((Vector2)transform.position + groundCheckOffset, groundCheckSize, 0, groundLayerMask);

        //Stop the player velocity when grounded
        if (isGrounded && rb.velocity.y <= 0)
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }

        //Player Movment
        if
        (
            Input.GetButton("Jump") && isGrounded &&
            !InputUtilities.IsMouseOverUI((RaycastResult _raycastResult)=>{return _raycastResult.gameObject.GetComponent<UnityEngine.UI.Selectable>() == null;})
        )
        {
            if (rb.velocity.y <= 0.0f) gameManager.AddScore(1U);
            Jump();
        }

        //Defeat the player if they fall down a pit
        if (transform.position.y < -2.0f) Destroy(gameObject);
    }

    void LateUpdate()
    {
        //Set Animations
        if (isGrounded) animator.CrossFade("Idle", 0.0f);
        else if (rb.velocity.y > 0.0f) animator.CrossFade("Jump", 0.0f);
        else animator.CrossFade("Fall", 0.0f);
    }

    public void Jump()
    {
        Vector2 velocity;
        velocity.x = moveSpeed;
        velocity.y = Mathf.Sqrt(2.0f * Physics2D.gravity.magnitude * rb.gravityScale * jumpHeight);

        rb.velocity = velocity;
        jumpParticle.Play();
    }

    void OnDrawGizmosSelected()
    {
        //Show Ground Check
        if (showGroundCheck)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + (Vector3)groundCheckOffset, groundCheckSize);
        }
    }

    void OnTriggerEnter2D(Collider2D _collision)
    {
        //Kill player
        if (_collision.tag == "Obstacle")
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnCollisionEnter2D(Collision2D _collision)
    {
        //Kill player
        if (_collision.transform.tag == "Obstacle")
        {
            Destroy(gameObject);
            return;
        }
    }
}
