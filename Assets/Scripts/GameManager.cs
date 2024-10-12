using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private float boundX = 7.5f;
    public float boundY = 5.5f;

    private float boundOffset = 0.5f;
    private int score = 0;
    private int lives = 5;
    private int level = 1;
    float movementHorizontal;
    float movementVertical;
    private Paddle paddle;
    private Vector3 origBouncyBallPosition;
    private BouncyBall[] bouncyBalls;

    public GameObject lockDown;
    private LevelGenerator levelGenerator;

    public AudioSource audioSource;
    public AudioClip increaseLifeSound;
    public AudioClip gameOverSound;
    public AudioClip levelUpSound;
    public AudioClip loseLifeSound;

    void Start()
    {
        paddle = FindObjectOfType<Paddle>();
        bouncyBalls = FindObjectsOfType<BouncyBall>();
        origBouncyBallPosition = bouncyBalls[0].transform.position;
        levelGenerator = FindObjectOfType<LevelGenerator>();
    }

    void Update()
    {
        handleHorizontalMovement();
        handleVerticalMovement();
        handleBallsMovement();
    }

    public int getScore()
    {
        return score;
    }

    public void setScore(int value)
    {
        score = value;
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
        levelGenerator.ChangeLevel();
        playSound(gameOverSound);

        lives = 5;
        level = 1;
        score = 0;
        // TODO destroy all balls except one
        foreach (BouncyBall bouncyBall in bouncyBalls)
        {

            bouncyBall.restartBall();
        }
    }

    public void addBalls(int count = 1)
    {
        GameObject newBallObject = Instantiate(bouncyBalls[0].gameObject);
        Vector3 origBallPosition = bouncyBalls[0].transform.position;
        for (int i = 0; i < count; i++)
        {
            Instantiate(newBallObject, new Vector3(
                origBallPosition.x + Random.Range(-1.0f, 1.0f),
                origBallPosition.y + Random.Range(-1.0f, 1.0f),
                0
            ), Quaternion.identity);
        }
        bouncyBalls = FindObjectsOfType<BouncyBall>();
        foreach (BouncyBall bouncyBall in bouncyBalls)
        {
            bouncyBall.startBall();
            bouncyBall.originalPosition = origBouncyBallPosition;

        }
    }

    public void setLockDown()
    {
        LockDown lockObj = FindObjectOfType<LockDown>();
        if (lockObj != null)
        {
            lockObj.ResetTimer();
        }
        else
        {

            Instantiate(lockDown, new Vector3(0, -4.5f, 0), Quaternion.identity);
        }
    }

    public void increaseBallsSpeed()
    {
        bouncyBalls = FindObjectsOfType<BouncyBall>();
        foreach (BouncyBall bouncyBall in bouncyBalls)
        {
            bouncyBall.increaseBallSpeed();
        }
    }
    public void decreaseBallsSpeed()
    {
        bouncyBalls = FindObjectsOfType<BouncyBall>();
        foreach (BouncyBall bouncyBall in bouncyBalls)
        {
            bouncyBall.decreaseBallSpeed();
        }
    }
    public int getLevel()
    {
        return level;
    }

    public int ballCount()
    {
        return bouncyBalls.Length;
    }

    public void levelUp()
    {
        score += 500 * level;
        level++;
        setLives(lives + 1);
        playSound(levelUpSound);
        foreach (BouncyBall bouncyBall in bouncyBalls)
        {
            bouncyBall.restartBall();
        }
    }

    private void handleFalling(BouncyBall bouncyBall)
    {
        if (ballCount() == 1)
        {
            bouncyBall.restartBall();
            playSound(loseLifeSound);
            paddle.ResetPaddle();
            setLives(lives - 1);
        }
        else
        {
            Destroy(bouncyBall.gameObject);
        }
    }

    private void handleBallsMovement()
    {
        bouncyBalls = FindObjectsOfType<BouncyBall>();
        foreach (BouncyBall bouncyBall in bouncyBalls)
        {
            Vector3 ballPosition = bouncyBall.transform.position;
            if (ballPosition.y < -boundY) handleFalling(bouncyBall);

            bouncyBall.handleBoundary(boundX, boundY);


        }
    }

    private void playSound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    private void handleHorizontalMovement()
    {
        movementHorizontal = Input.GetAxis("Horizontal");
        if ((movementHorizontal > 0 && paddle.transform.position.x < boundX) || (movementHorizontal < 0 && paddle.transform.position.x > -boundX))
        {
            paddle.setPosition(movementHorizontal);
        }
    }

    private void handleVerticalMovement()
    {
        movementVertical = Input.GetAxis("Vertical");
        if (movementVertical > 0)
        {
            bouncyBalls = FindObjectsOfType<BouncyBall>();
            foreach (BouncyBall bouncyBall in bouncyBalls)
            {
                bouncyBall.startBall();
            }
        }
    }

}
