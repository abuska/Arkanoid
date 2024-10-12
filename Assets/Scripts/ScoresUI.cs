using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoresUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private GameManager gameManager;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        string text = "Score: " + gameManager.getScore().ToString() + "\nLevel: " + gameManager.getLevel().ToString();
        scoreText.text = text;
    }
}
