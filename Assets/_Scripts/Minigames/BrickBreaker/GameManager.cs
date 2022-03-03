using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ball ball { get; private set; }
    public Paddle paddle { get; private set; }

    public Brick[] bricks { get; private set; }
    
    public static int score = 0;

    public static int lives = 3;


    private void Awake()
    {
        this.ball = FindObjectOfType<Ball>();
        this.paddle = FindObjectOfType<Paddle>();
        this.bricks = FindObjectsOfType<Brick>();
    }


    public void ResetLevel()
    {
        this.ball.ResetBall();
        this.paddle.ResetPaddle();
    }

    private void GameOver()
    {
        this.paddle.Dead();
    }
    

    public void Miss()
    {
        lives--;

        if (lives > 0)
        {
            ResetLevel();
        } else
        {
            GameOver();
        }
    }
    

    public void Hit(Brick brick)
    {
        score += brick.points;

        if (Cleared()){
            GameOver();
        }
    }

    private bool Cleared() //Checks to see if all of the bricks have been cleared
    {
        for (int i = 0; i < this.bricks.Length; i++)
        {
            if (this.bricks[i].gameObject.activeInHierarchy && !this.bricks[i].unbreakable)
            {
                return false;
            }
        }

        return true;
    }
}
