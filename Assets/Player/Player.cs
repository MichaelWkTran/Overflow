using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] bool showGroundCheck;

    public bool isGrounded { get; private set; }
    [SerializeField] Vector2 groundCheckOffset;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] LayerMask groundLayerMask;

    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void OnDestroy()
    {
        gameManager.triggerGameOver();
    }

    void Update()
    {
        //Check whether the player is on the ground
        isGrounded = Physics2D.OverlapBox((Vector2)transform.position + groundCheckOffset, groundCheckSize, 0, groundLayerMask);

        //Stop the player velocity when grounded
        if (isGrounded && GetComponent<Rigidbody2D>().velocity.y <= 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, GetComponent<Rigidbody2D>().velocity.y);
        }

        //Player Movment
        if (isGrounded && (Input.GetKey("space") || Input.touchCount > 0 || Input.GetMouseButton(0)))
        {
            //Add score every jump
            if (GetComponent<Rigidbody2D>().velocity.y <= 0.0f) gameManager.score++;

            //Move the player
            Vector2 velocity;
            velocity.x = moveSpeed;
            velocity.y = Mathf.Sqrt(2.0f * Physics2D.gravity.magnitude * GetComponent<Rigidbody2D>().gravityScale * jumpHeight);

            GetComponent<Rigidbody2D>().velocity = velocity;
        }

        //Defeat the player if they fall down a pit
        if (transform.position.y < 2.0f) Destroy(gameObject);
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
