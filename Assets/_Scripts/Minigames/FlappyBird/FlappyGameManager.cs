using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappyGameManager : MinigameBase
{
    public static int score = 0;
    public static int lives = 3;
    public static int bestScore = 0;
    public FlappyBirdPlayer player;
    public PipeSpawner spawner;
    public Parallax groundParallax;
    public Parallax backgroundParallax;



    private void Awake()
    {
        Application.targetFrameRate = 60;

        player.enabled = false;
        spawner.enabled = false;
        groundParallax.enabled = false;
        backgroundParallax.enabled = false;
    }

    public void Play()
    {
        player.enabled = true;
        spawner.enabled = true;
        groundParallax.enabled = true;
        backgroundParallax.enabled = true;

    }

    public void Pause()
    {
        player.enabled = false;
        spawner.enabled = false;
        groundParallax.enabled = false;
        backgroundParallax.enabled = false;
        score = 0;

        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
    }

    public void LoseLife()
    {
        Pause();
        lives--;
        if (lives == 0)
        {
            GameOver();
        } else
        {
            Invoke(nameof(Play), 2f);
        }
    }

    public void GameOver()
    {
        Debug.Log("Game over");
        EndingMinigame();
    }

    public void IncreaseScore()
    {
        score++;

        if (score > bestScore)
        {
            bestScore = score;
        }
    }
}
