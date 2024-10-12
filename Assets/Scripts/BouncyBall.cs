using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBall : MonoBehaviour
{
    public bool isStarted = false;
    private bool isSticky = false;
    // TODO sticky should be in paddle
    private float stickyTime = 5f;
    private float stickyTimer = 0f;
    public float ballSpeed = 10f;
    public float maxSpeed = 16f;
    public float minSpeed = 5f;
    public Vector3 originalPosition;

    public Rigidbody2D rb;

    public Paddle paddle;

    public AudioSource audioSource;
    public AudioClip brickHitSound;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        paddle = FindObjectOfType<Paddle>();
        originalPosition = transform.position;
    }

    private void playSound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    public void startBall()
    {
        if (isStarted) return;
        rb.velocity = new Vector2(0, ballSpeed);
        isStarted = true;
    }
    public void restartBall()
    {
        transform.position = originalPosition;
        rb.velocity = Vector2.zero;
        isStarted = false;
    }
    public void stickyBall()
    {
        stickyTimer = stickyTime;
        isSticky = true;
    }

    private void handleVelocity()
    {

        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            rb.velocity = new Vector2(0, ballSpeed);
        }
        if (rb.velocity.y == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, -ballSpeed);
        }
        rb.velocity = rb.velocity.normalized * ballSpeed;
        if (rb.velocity.magnitude > ballSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, ballSpeed);
            Debug.Log("Clamping velocity" + rb.velocity);
        }
    }

    public void handleBoundary(
        float boundX,
        float boundY
    )
    {
        if (transform.position.y > boundY)
        {
            rb.velocity = new Vector2(rb.velocity.x, -ballSpeed);
            transform.position = new Vector3(transform.position.x, boundY, transform.position.z);
        }
        if (transform.position.x > boundX)
        {
            rb.velocity = new Vector2(-ballSpeed, rb.velocity.y);
            transform.position = new Vector3(boundX, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -boundX)
        {
            rb.velocity = new Vector2(ballSpeed, rb.velocity.y);
            transform.position = new Vector3(-boundX, transform.position.y, transform.position.z);
        }
    }

    public void increaseBallSpeed()
    {
        if (ballSpeed >= maxSpeed) return;
        ballSpeed += 2;
    }
    public void decreaseBallSpeed()
    {
        if (ballSpeed <= minSpeed) return;
        ballSpeed -= 2;
    }

    private void setPositionToPaddle()
    {
        transform.position = new Vector3(paddle.GetPosition().x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (isSticky)
        {
            if (stickyTimer <= 0)
            {
                isSticky = false;
            }
            stickyTimer -= Time.deltaTime;
        }
        if (!isStarted)
        {
            setPositionToPaddle();
        }
        else
        {
            handleVelocity();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            CollideBrick(collision.gameObject.GetComponent<Brick>());
        }
        if (collision.gameObject.CompareTag("Paddle"))
        {
            HandleStickyBall();
        }
    }

    private void HandleStickyBall()
    {
        if (isSticky && isStarted)
        {
            rb.velocity = Vector2.zero;
            isStarted = false;
        }
    }

    private void CollideBrick(Brick brick)
    {
        playSound(brickHitSound);
        float hitValue = brick.Hit();
        if (hitValue > 0)
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            gameManager.setScore(gameManager.getScore() + (int)hitValue);
        }
    }
}
