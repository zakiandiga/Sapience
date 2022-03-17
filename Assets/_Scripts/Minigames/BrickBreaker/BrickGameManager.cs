using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickGameManager : MonoBehaviour
{
    public Ball ball { get; private set; }
    public Paddle paddle { get; private set; }

    public Brick[] bricks { get; private set; }
    
    public static int score = 0;

    public static int lives = 3;


    public AudioClip brickBreakerMusic;
    public AudioClip paddleHitSound;
    public AudioClip brickHitSound;
    public AudioClip wallHitSound;
    public AudioClip loseSound;
    public AudioClip winSound;
    public AudioSource brickAudioSource;
    public AudioSource brickMusicSource;



    private void Awake()
    {
        this.ball = FindObjectOfType<Ball>();
        this.paddle = FindObjectOfType<Paddle>();
        this.bricks = FindObjectsOfType<Brick>();

        brickAudioSource = GetComponent<AudioSource>(); //If these GetComponentFunctions don't work, then you can comment them out or delete them and just drag the audio sources into the open script component

        brickMusicSource = GetComponent<AudioSource>();
        brickMusicSource.clip = brickBreakerMusic;
        brickMusicSource.loop = true;
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
            brickAudioSource.PlayOneShot(loseSound, 1);
            GameOver();
        }
    }
    

    public void Hit(Brick brick)
    {
        score += brick.points;
        brickAudioSource.PlayOneShot(brickHitSound, 1);
        Debug.Log("Brick hit sound");

        if (Cleared()){
            brickAudioSource.PlayOneShot(winSound, 1);
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


    public void StartMusic()
    {
        brickMusicSource.Play();
        Debug.Log("Music is now playing");
    }


    public void PlayPaddleBounceSound()
    {
        brickAudioSource.PlayOneShot(paddleHitSound, 1);
        Debug.Log("Paddle hit sound");
    }

    public void PlayWallSound()
    {
        brickAudioSource.PlayOneShot(wallHitSound, 1);
        Debug.Log("Wall hit sound");
    }
}
