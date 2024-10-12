using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoresUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private BouncyBall bouncyBall;
    void Awake()
    {
        bouncyBall = FindObjectOfType<BouncyBall>();
    }

    void Update()
    {
        string text = "Score: " + bouncyBall.getScore().ToString() + "\nLevel: " + bouncyBall.getLevel().ToString();
        scoreText.text = text;
    }
}
