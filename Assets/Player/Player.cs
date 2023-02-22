using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;

    [Header("Dash Variables")]
    public float dashSpeed;
    public float dashHeight;
    public float dashInputDistance; //How far does the player has to draw their finger or mouse

    [Header("Grounded Variables")]
    [SerializeField] bool showGroundCheck;
    public bool isGrounded { get; private set; }
    [SerializeField] Vector2 groundCheckOffset;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] LayerMask groundLayerMask;

    [Header("Particles")]
    [SerializeField] ParticleSystem jumpParticle;
    public ParticleSystem deathParticle;

    [Header("Audio")]
    [SerializeField] AudioClip m_jumpSound;
    [SerializeField] AudioClip m_landSound;
    [SerializeField] AudioClip m_deathSound;

    GameManager gameManager;
    Rigidbody2D rb;
    Animator animator;
    AudioSource m_audioSource;

    void Start()
    {
        //Get Components
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();

        //Get the Current Skin
        if (ShopItem.SkinData.m_currentSkin != null) ShopItem.SkinData.m_currentSkin.SetSkin();
    }

    void OnDestroy()
    {
        //Play the death animation
        {
            deathParticle.transform.parent = transform.parent;
            deathParticle.gameObject.SetActive(true);
            deathParticle.Play();
        }

        //Play the screen flash animation
        {
            gameManager.m_screenFlash.gameObject.SetActive(true);
            gameManager.m_screenFlash.GetComponent<Animator>().Play("Screen Flash");
            Destroy(gameManager.m_screenFlash, 1.0f);
        }
        
        //Play the death sound effect
        {
            AudioSource deathSoundSource = new GameObject().AddComponent<AudioSource>();
            deathSoundSource.outputAudioMixerGroup = m_audioSource.outputAudioMixerGroup;
            deathSoundSource.clip = m_deathSound;
            deathSoundSource.transform.position = transform.position;
            Destroy(deathSoundSource.gameObject, m_deathSound.length);
            deathSoundSource.Play();
        }

        //Open the game over screen
        gameManager.Invoke("triggerGameOver", 1.0f);

        //Save the game
        SaveSystem.Save();
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
        if (InputUtilities.CanUseJumpInput() && isGrounded)
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
        if (isGrounded && animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            animator.CrossFade("Land", 0.0f);
            if (rb.velocity.y <= 0.0f)
            {
                m_audioSource.clip = m_landSound;
                m_audioSource.Play();
            }
            
        }
        else if (isGrounded) { }
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
        m_audioSource.Stop();
        m_audioSource.clip = m_jumpSound;
        m_audioSource.Play();
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
