using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class BrickGameManager : MonoBehaviour
{
    public Ball ball { get; private set; }
    public Paddle paddle { get; private set; }

    public Brick[] bricks { get; private set; }
    
    public static int score = 0;

    public static int lives = 3;


    /*public AudioClip brickBreakerMusic;
    public AudioClip paddleHitSound;
    public AudioClip brickHitSound;
    public AudioClip wallHitSound;
    public AudioClip loseSound;
    public AudioClip winSound;
    public AudioSource brickAudioSource;
    public AudioSource brickMusicSource;*/


    //FMOD version of this system
    [SerializeField] private EventReference musicPath, paddleHitSoundPath, brickHitSoundPath, wallHitSoundPath, loseSoundPath, winSoundPath;
    private EventInstance musicInstance;
    private PLAYBACK_STATE currentMusicState;


    private void Awake()
    {
        this.ball = FindObjectOfType<Ball>();
        this.paddle = FindObjectOfType<Paddle>();
        this.bricks = FindObjectsOfType<Brick>();

       /* brickAudioSource = GetComponent<AudioSource>(); //If these GetComponentFunctions don't work, then you can comment them out or delete them and just drag the audio sources into the open script component

        brickMusicSource = GetComponent<AudioSource>();
        brickMusicSource.clip = brickBreakerMusic;
        brickMusicSource.loop = true;*/
    }

    private void Start()
    {
        //create the instance of the BGM to be ready to use, don't forget to stop and release it later when it's no longer used (OnDisable?)
        musicInstance = RuntimeManager.CreateInstance(musicPath);
    }


    public void ResetLevel()
    {
        this.ball.ResetBall();
        this.paddle.ResetPaddle();
    }

    private void GameOver()
    {
        this.paddle.Dead();
        score = 0;
        lives = 3;
    }
    

    public void Miss()
    {
        lives--;

        if (lives > 0)
        {
            ResetLevel();
        } else
        {
            //brickAudioSource.PlayOneShot(loseSound, 1);
            RuntimeManager.PlayOneShot(loseSoundPath, this.transform.position);
            GameOver();
        }
    }
    

    public void Hit(Brick brick)
    {
        score += brick.points;
        //brickAudioSource.PlayOneShot(brickHitSound, 1);
        RuntimeManager.PlayOneShot(brickHitSoundPath, this.transform.position);
        Debug.Log("Brick hit sound");

        if (Cleared()){
            //brickAudioSource.PlayOneShot(winSound, 1);
            RuntimeManager.PlayOneShot(winSoundPath, this.transform.position);
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
        //brickMusicSource.Play();

        //Check if the music is playing or starting, then if it's not, start the music
        musicInstance.getPlaybackState(out currentMusicState);
        if (currentMusicState != PLAYBACK_STATE.PLAYING || currentMusicState != PLAYBACK_STATE.STARTING)
        {
            musicInstance.start();
            musicInstance.release();
        }
    }

    public void StopMusic()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Debug.Log("Music has stopped");
    }


    public void PlayPaddleBounceSound()
    {
        //brickAudioSource.PlayOneShot(paddleHitSound, 1);
        RuntimeManager.PlayOneShot(paddleHitSoundPath, this.transform.position);
        Debug.Log("Paddle hit sound");
    }

    public void PlayWallSound()
    {
        //brickAudioSource.PlayOneShot(wallHitSound, 1);
        RuntimeManager.PlayOneShot(wallHitSoundPath, this.transform.position);
        Debug.Log("Wall hit sound");
    }
}
