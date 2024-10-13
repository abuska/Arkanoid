using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Paddle : MonoBehaviour
{
    // Default paddle sizes
    Vector3 BIG_PADDLE_SIZE = new Vector3(3f, 0.5f, 1f);
    Vector3 NORMAL_PADDLE_SIZE = new Vector3(2f, 0.5f, 1f);
    Vector3 SMALL_PADDLE_SIZE = new Vector3(1f, 0.5f, 1f);
    private Vector2 originalPosition;

    // Paddle speed
    private float speed = 10f;
    public float getSpeed()
    {
        return speed;
    }

    // Sticky paddle variables
    private bool isSticky = false;
    private float stickyTime = 5f;
    private float stickyTimer = 0f;
    public void setPaddleToSticky()
    {
        audioSource.clip = stickyClip;
        audioSource.Play();
        stickyTimer = stickyTime;
        isSticky = true;
    }
    public void handleIsSticky()
    {
        if (isSticky)
        {
            if (stickyTimer <= 0)
            {
                isSticky = false;
                audioSource.Stop();
            }
            stickyTimer -= Time.deltaTime;
        }
    }

    // Paddle size change variables
    private float sizeChangeTime = 5f;
    private float sizeChangeTimer = 0f;
    public AudioSource audioSource;
    public AudioClip sizeChangeClip;
    public AudioClip stickyClip;
    public void handlePaddleSizeChanges()
    {
        if (sizeChangeTimer > 0)
        {
            sizeChangeTimer -= Time.deltaTime;
            if (sizeChangeTimer <= 0)
            {
                normalPaddle();
                audioSource.clip = sizeChangeClip;
                audioSource.Play();
            }
        }
    }

    // Paddle position 
    public Vector2 GetPosition()
    {
        return transform.position;
    }
    public void setPosition(float movementValue)
    {
        transform.position = new Vector3(transform.position.x + movementValue * speed * Time.deltaTime, transform.position.y, transform.position.z);
    }

    // Functions
    private void Awake()
    {
        originalPosition = GetPosition();
    }

    //Reset paddle position
    public void ResetPaddle()
    {
        normalPaddle();
        sizeChangeTimer = 0;
        transform.position = originalPosition;
    }

    // Paddle size change functions
    public void normalPaddle()
    {
        transform.localScale = NORMAL_PADDLE_SIZE;
    }
    public void bigPaddle()
    {
        transform.localScale = BIG_PADDLE_SIZE;
        sizeChangeTimer = sizeChangeTime;
        audioSource.clip = sizeChangeClip;
        audioSource.Play();
    }
    public void smallPaddle()
    {
        transform.localScale = SMALL_PADDLE_SIZE;
        sizeChangeTimer = sizeChangeTime;
        audioSource.clip = sizeChangeClip;
        audioSource.Play();
    }

    // Sticky paddle collision with balls
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // TODO: Add different position to the ball based on where it hits the paddle
            // To avoid set multiple ball position to same position
            // But it's not a priority now
            BouncyBall ball = collision.gameObject.GetComponent<BouncyBall>();
            if (isSticky && ball.isStarted)
            {
                ball.rb.velocity = Vector2.zero;
                ball.isStarted = false;
            }

        }
    }
}