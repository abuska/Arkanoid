using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementHandler : MonoBehaviour
{
    private float speed = 10f;
    private float bound = 7.5f;
    float movementHorizontal;
    float movementVertical;
    private Vector2 originalPosition;

    private float sizeChangeTime = 5f;
    private float sizeChangeTimer = 0f;

    public AudioSource sizeChangeSound;
    private void Awake()
    {
        originalPosition = GetPosition();
    }
    void Update()
    {
        handleHorizontalMovement();
        handleVerticalMovement();
        handlePaddleSizeChanges();
    }
    public void handleHorizontalMovement()
    {
        movementHorizontal = Input.GetAxis("Horizontal");
        if ((movementHorizontal > 0 && transform.position.x < bound) || (movementHorizontal < 0 && transform.position.x > -bound))
        {
            transform.position = new Vector3(transform.position.x + movementHorizontal * speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }
    public void handleVerticalMovement()
    {
        movementVertical = Input.GetAxis("Vertical");
        if (movementVertical > 0)
        {
            BouncyBall bouncyBall = FindObjectOfType<BouncyBall>();
            bouncyBall.startBall();
        }
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
        transform.localScale = new Vector3(2f, 0.5f, 1f);

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
}
