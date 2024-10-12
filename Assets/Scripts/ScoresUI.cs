using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoresUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public void setScore(int score, int level)
    {
        scoreText.text = "Score: " + score.ToString() + "\nLevel: " + level.ToString();

    }
}
