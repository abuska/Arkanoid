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

    public float maxVelocity = 15f;
    public float ballSpeed = 10f;
    public Vector3 originalPosition;

    Rigidbody2D rb;

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
        Debug.Log("Velocity" + rb.velocity);
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
            Debug.Log("Clamping velocity" + rb.velocity);
        }
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
