using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Paddle : MonoBehaviour
{
    Vector3 BIG_PADDLE_SIZE = new Vector3(3f, 0.5f, 1f);
    Vector3 NORMAL_PADDLE_SIZE = new Vector3(2f, 0.5f, 1f);
    Vector3 SMALL_PADDLE_SIZE = new Vector3(1f, 0.5f, 1f);
    private float speed = 10f;
    private Vector2 originalPosition;
    private float sizeChangeTime = 5f;
    private float sizeChangeTimer = 0f;
    public AudioSource sizeChangeSound;
    private void Awake()
    {
        originalPosition = GetPosition();
    }
    public void Update()
    {
        handlePaddleSizeChanges();
    }
    public void handlePaddleSizeChanges()
    {
        if (sizeChangeTimer > 0)
        {
            sizeChangeTimer -= Time.deltaTime;
            if (sizeChangeTimer <= 0)
            {
                normalPaddle();
                sizeChangeSound.Play();
            }
        }
    }
    public void ResetPaddle()
    {
        normalPaddle();
        transform.position = originalPosition;
    }
    public Vector2 GetPosition()
    {
        return transform.position;
    }
    public void normalPaddle()
    {
        transform.localScale = NORMAL_PADDLE_SIZE;

    }
    public void bigPaddle()
    {
        transform.localScale = new Vector3(3f, 0.5f, 1f);
        sizeChangeTimer = sizeChangeTime;
        sizeChangeSound.Play();
    }
    public void smallPaddle()
    {
        transform.localScale = new Vector3(1f, 0.5f, 1f);
        sizeChangeTimer = sizeChangeTime;
        sizeChangeSound.Play();
    }
    public float getSpeed()
    {
        return speed;
    }
    public void setPosition(float movementValue)
    {
        transform.position = new Vector3(transform.position.x + movementValue * speed * Time.deltaTime, transform.position.y, transform.position.z);
    }
}