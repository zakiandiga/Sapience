using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ball ball { get; private set; }
    public Paddle paddle { get; private set; }
    
    public int score = 0;

    public int lives = 3;


    private void Awake()
    {
        this.ball = FindObjectOfType<Ball>();
        this.paddle = FindObjectOfType<Paddle>();
    }


    private void ResetLevel()
    {
        this.ball.ResetBall();
        this.paddle.ResetPaddle();
    }

    private void GameOver()
    {
        Paddle.Dead();
    }
    

    public void Miss()
    {
        this.lives--;

        if (this.lives > 0)
        {
            ResetLevel();
        } else
        {
            GameOver();
        }
    }
    

    public void Hit(Brick brick)
    {
        this.score += brick.points;
    }
}
