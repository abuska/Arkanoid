using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementHandler : MonoBehaviour
{
    private float bound = 7.5f;
    float movementHorizontal;
    float movementVertical;
    private Paddle paddle;

    void Start()
    {
        paddle = FindObjectOfType<Paddle>();
    }


    void Update()
    {
        handleHorizontalMovement();
        handleVerticalMovement();
    }
    public void handleHorizontalMovement()
    {
        movementHorizontal = Input.GetAxis("Horizontal");
        if ((movementHorizontal > 0 && paddle.transform.position.x < bound) || (movementHorizontal < 0 && paddle.transform.position.x > -bound))
        {
            paddle.setPosition(movementHorizontal);
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


}
