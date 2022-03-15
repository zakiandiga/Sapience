using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlappyScoreWindow : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI livesText;
    private TextMeshProUGUI bestScoreText;

    private void Awake()
    {
        scoreText = transform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        livesText = transform.Find("LivesText").GetComponent<TextMeshProUGUI>();
        bestScoreText = transform.Find("BestScoreText").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        scoreText.text = "Score: " + FlappyGameManager.score.ToString();
        livesText.text = "Lives: " + FlappyGameManager.lives.ToString();
        bestScoreText.text = "Best Score: " + FlappyGameManager.bestScore.ToString();
    }
}
