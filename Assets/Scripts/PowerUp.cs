using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private BouncyBall bouncyBall;
    private Paddle paddle;
    public enum PowerUpType
    {
        ExtraLife,
        LevelUp,
        SpeedUp,
        SlowDown,
        MultiBall,
        StickyPaddle,
        BigPaddle,
        SmallPaddle,
        LockDown,

    }
    public PowerUpType powerUpType;
    private GameManager gameManager;

    public float minY = -5.5f;

    public void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        bouncyBall = FindObjectOfType<BouncyBall>();
        paddle = FindObjectOfType<Paddle>();
    }
    public void Update()
    {
        if (transform.position.y < minY)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Paddle"))
        {
            {
                switch (powerUpType)
                {
                    case PowerUpType.ExtraLife:
                        gameManager.setLives(gameManager.getLives() + 1);
                        break;
                    case PowerUpType.LevelUp:
                        FindObjectOfType<LevelGenerator>().ChangeLevel();
                        break;
                    case PowerUpType.SpeedUp:
                        // bouncyBall.maxVelocity += 2;
                        break;
                    case PowerUpType.SlowDown:
                        // bouncyBall.maxVelocity -= 2;
                        break;
                    case PowerUpType.MultiBall:
                        gameManager.addBalls(2);
                        break;
                    case PowerUpType.StickyPaddle:
                        bouncyBall.stickyBall();
                        break;
                    case PowerUpType.BigPaddle:
                        paddle.bigPaddle();
                        break;
                    case PowerUpType.SmallPaddle:
                        paddle.smallPaddle();
                        break;
                    case PowerUpType.LockDown:
                        gameManager.setLockDown();
                        break;

                }
                Destroy(this.gameObject);
            }


        }
    }
}
