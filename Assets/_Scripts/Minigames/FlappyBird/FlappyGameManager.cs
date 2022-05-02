using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class FlappyGameManager : MinigameBase
{
    public static int score = 0;
    public static int lives = 3;
    public static int bestScore = 0;
    public FlappyBirdPlayer player;
    public PipeSpawner spawner;
    public Parallax groundParallax;
    public Parallax backgroundParallax;

    //FMOD version of the below system
    [SerializeField] private EventReference musicPath, scorePointSoundPath, collisionSoundPath, gameOverSoundPath;
    private EventInstance musicInstance;
    private PLAYBACK_STATE currentMusicState;

    /*public AudioClip flappyBirdMusic;
    public AudioClip scorePointSound;
    public AudioClip collisionSound;
    public AudioClip gameOverSound;
    public AudioSource flappyAudioSource;
    public AudioSource flappyMusicSource;*/
    private bool startMusicOnce;



    private void Awake()
    {
        Application.targetFrameRate = 60;

        player.enabled = false;
        spawner.enabled = false;
        groundParallax.enabled = false;
        backgroundParallax.enabled = false;

        /*flappyAudioSource = GetComponent<AudioSource>(); //If these GetComponentFunctions don't work, then you can comment them out or delete them and just drag the audio sources into the open script component

        flappyMusicSource = GetComponent<AudioSource>();
        flappyMusicSource.clip = flappyBirdMusic;
        flappyMusicSource.loop = true;*/
    }

    private void Start()
    {
        //create the instance of the BGM to be ready to use, don't forget to stop and release it later when it's no longer used (OnDisable?)
        musicInstance = RuntimeManager.CreateInstance(musicPath);

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
            //flappyMusicSource.Play();

            //Check if the music is playing or starting, then if it's not, start the music
            musicInstance.getPlaybackState(out currentMusicState);
            if (currentMusicState != PLAYBACK_STATE.PLAYING || currentMusicState != PLAYBACK_STATE.STARTING)
            {
                musicInstance.start();
                musicInstance.release();
            }
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
        //flappyAudioSource.PlayOneShot(collisionSound, 1);
        RuntimeManager.PlayOneShot(collisionSoundPath, this.transform.position);
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
        //flappyAudioSource.PlayOneShot(gameOverSound, 1);
        RuntimeManager.PlayOneShot(gameOverSoundPath, this.transform.position);
        Debug.Log("Game over");
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Debug.Log("Music has stopped");
        EndingMinigame();
        score = 0;
        bestScore = 0;
        lives = 3;
    }

    public void IncreaseScore()
    {
        score++;
        //flappyAudioSource.PlayOneShot(scorePointSound, 1);
        RuntimeManager.PlayOneShot(scorePointSoundPath, this.transform.position);

        if (score > bestScore)
        {
            bestScore = score;
        }
    }
}
