using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private float bound = 7.5f;
    public float minY = -5.5f;
    private int score = 0;
    private int lives = 5;
    private int level = 1;
    float movementHorizontal;
    float movementVertical;
    private Paddle paddle;
    private Vector3 origBouncyBallPosition;
    private BouncyBall[] bouncyBalls;

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
        bouncyBalls = FindObjectsOfType<BouncyBall>();
        foreach (BouncyBall bouncyBall in bouncyBalls)
        {
            if (bouncyBall.transform.position.y < minY) handleFalling(bouncyBall);
        }
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

    private void playSound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    private void handleHorizontalMovement()
    {
        movementHorizontal = Input.GetAxis("Horizontal");
        if ((movementHorizontal > 0 && paddle.transform.position.x < bound) || (movementHorizontal < 0 && paddle.transform.position.x > -bound))
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
