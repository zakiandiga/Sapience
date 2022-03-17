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


    public AudioClip flappyBirdMusic;
    public AudioClip scorePointSound;
    public AudioClip collisionSound;
    public AudioClip gameOverSound;
    public AudioSource flappyAudioSource;
    public AudioSource flappyMusicSource;
    private bool startMusicOnce;



    private void Awake()
    {
        Application.targetFrameRate = 60;

        player.enabled = false;
        spawner.enabled = false;
        groundParallax.enabled = false;
        backgroundParallax.enabled = false;

        flappyAudioSource = GetComponent<AudioSource>(); //If these GetComponentFunctions don't work, then you can comment them out or delete them and just drag the audio sources into the open script component

        flappyMusicSource = GetComponent<AudioSource>();
        flappyMusicSource.clip = flappyBirdMusic;
        flappyMusicSource.loop = true;
        startMusicOnce = false;
    }

    public void Play()
    {  
        player.enabled = true;
        spawner.enabled = true;
        groundParallax.enabled = true;
        backgroundParallax.enabled = true;

        if (!startMusicOnce)
        {
            flappyMusicSource.Play();
            startMusicOnce = true;
        }

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
        flappyAudioSource.PlayOneShot(collisionSound, 1);
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
        flappyAudioSource.PlayOneShot(gameOverSound, 1);
        Debug.Log("Game over");
        EndingMinigame();
    }

    public void IncreaseScore()
    {
        score++;
        flappyAudioSource.PlayOneShot(scorePointSound, 1);

        if (score > bestScore)
        {
            bestScore = score;
        }
    }
}
