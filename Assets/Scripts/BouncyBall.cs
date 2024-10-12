using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBall : MonoBehaviour
{
    private bool isStarted = false;
    private bool isSticky = false;
    private float stickyTime = 5f;
    private float stickyTimer = 0f;
    public float minY = -5.5f;
    public float maxVelocity = 15f;
    public float speed = 5f;
    public Vector3 originalPosition;
    Rigidbody2D rb;
    private int score = 0;

    private int lives = 5;
    private int level = 1;

    public Paddle paddle;

    public AudioSource audioSource;
    public AudioClip gameOverSound;
    public AudioClip levelUpSound;
    public AudioClip brickHitSound;
    public AudioClip loseLifeSound;
    public AudioClip increaseLifeSound;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        paddle = FindObjectOfType<Paddle>();
        originalPosition = transform.position;
    }
    public int getScore()
    {
        return score;
    }
    public int getLives()
    {
        return lives;
    }
    public void setLives(int value)
    {
        if (value > 0)
        {
            lives = value;
            playSound(increaseLifeSound);
            return;
        };

        LevelGenerator levelGenerator = FindObjectOfType<LevelGenerator>();
        levelGenerator.ChangeLevel();
        playSound(gameOverSound);

        lives = 5;
        level = 1;
        score = 0;
        restartBall();


    }
    public int getLevel()
    {
        return level;
    }

    private void playSound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    public void startBall()
    {
        if (isStarted) return;
        rb.velocity = new Vector2(0, speed);
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

    public void levelUp()
    {
        score += 500 * level;
        level++;
        setLives(lives + 1);
        playSound(levelUpSound);
        restartBall();
    }

    private void handleFalling()
    {
        restartBall();
        playSound(loseLifeSound);
        paddle.ResetPaddle();
        setLives(lives - 1);
    }

    private void handleVelocity()
    {
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
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
        if (transform.position.y < minY) handleFalling();

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
            score += (int)hitValue;
        }
    }
}
