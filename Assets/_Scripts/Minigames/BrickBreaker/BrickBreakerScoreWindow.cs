using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BrickBreakerScoreWindow : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI livesText;
    private void Awake()
    {
        scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        livesText = transform.Find("LivesText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        scoreText.text = "Score: " + GameManager.score.ToString();
        livesText.text = "Lives: " + GameManager.lives.ToString();
    }
}
