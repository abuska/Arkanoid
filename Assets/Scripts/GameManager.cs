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

    // Audio
    public AudioSource audioSource;
    public AudioClip increaseLifeSound;
    public AudioClip gameOverSound;
    public AudioClip levelUpSound;
    public AudioClip loseLifeSound;

    // UI Elements

    private ScoresUI scoresUI;

    void Start()
    {
        paddle = FindObjectOfType<Paddle>();
        bouncyBalls = FindObjectsOfType<BouncyBall>();
        origBouncyBallPosition = bouncyBalls[0].transform.position;
        levelGenerator = FindObjectOfType<LevelGenerator>();
        scoresUI = FindObjectOfType<ScoresUI>();
    }

    void Update()
    {
        // Check if all bricks are destroyed and level up if true
        if (levelGenerator.transform.childCount == 0)
        {
            levelUp();
        }
        paddle.handleIsSticky();
        paddle.handlePaddleSizeChanges();
        handleHorizontalMovement();
        handleVerticalMovement();
        handleBallsMovement();
    }

    public void increaseScore(int value)
    {
        setScore(score + value);
    }

    public void setScore(int value)
    {
        score = value;
        scoresUI.setScore(score, level);
    }

    public void setLevel(int value)
    {
        level = value;
        scoresUI.setScore(score, level);
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
        setLevel(1);
        setScore(0);
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
        increaseScore(500 * level);

        setLevel(level + 1);
        setLives(lives + 1);
        playSound(levelUpSound);
        levelGenerator.ChangeLevel();
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
        if (bouncyBalls.Length == 0)
        {
            // TODO error handling add a ball
        }
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
