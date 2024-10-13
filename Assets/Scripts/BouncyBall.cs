using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBall : MonoBehaviour
{
    private GameManager gameManager;
    public Rigidbody2D rb;
    private Paddle paddle;
    public Vector3 originalPosition;
    public bool isStarted = false;

    // Sounds     
    public AudioSource audioSource;
    public AudioClip brickHitSound;

    // Ball speed
    public float ballSpeed = 10f;
    public float maxSpeed = 16f;
    public float minSpeed = 5f;
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

    // Functions
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

    public void handleBoundary(float boundX, float boundY)
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

    // Lifecycle helpers
    public void handleVelocity()
    {
        if (!isStarted) return;

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
        }
    }

    public void setPositionToPaddle()
    {
        if (isStarted) return;
        transform.position = new Vector3(paddle.GetPosition().x, originalPosition.y, originalPosition.z);

    }

    // Lifecycle
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        paddle = FindObjectOfType<Paddle>();
        originalPosition = transform.position;
        gameManager = FindObjectOfType<GameManager>();
    }

    // Collisions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Brick"))
        {
            CollideBrick(collision.gameObject.GetComponent<Brick>());
        }
    }

    // Collisions helpers
    private void CollideBrick(Brick brick)
    {
        playSound(brickHitSound);
        float hitScoreValue = brick.Hit();
        if (hitScoreValue > 0)
        {
            gameManager.increaseScore((int)hitScoreValue);
        }
    }
}
