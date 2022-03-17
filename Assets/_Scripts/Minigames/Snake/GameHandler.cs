using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;

    private static int score;
    private int previousScore;
    
    [SerializeField] public Snake snake;

    private LevelGrid levelGrid;

    public AudioClip snakeMusic;
    public AudioClip collectSound;
    public AudioClip gameOverSound;
    public AudioSource snakeAudioSource;
    public AudioSource snakeMusicSource;
    private bool startMusicOnce;

    //FMOD version of this system
    [SerializeField] private EventReference musicPath, collectPath, gameOverPath;
    private EventInstance musicInstance;
    private PLAYBACK_STATE currentMusicState;


    private void Awake()
    {
        instance = this;
    }

 
    void Start()
    {
        Debug.Log("GameHandler.Start");

        levelGrid = new LevelGrid(20, 20);

        snake.Setup(levelGrid);
        levelGrid.Setup(snake);

        snakeAudioSource = GetComponent<AudioSource>(); //If these GetComponentFunctions don't work, then you can comment them out or delete them and just drag the audio sources into the open script component

        snakeMusicSource = GetComponent<AudioSource>();
        snakeMusicSource.clip = snakeMusic;
        snakeMusicSource.loop = true;
        startMusicOnce = false;

        //create the instance of the BGM to be ready to use, don't forget to stop and release it later when it's no longer used (OnDisable?)
        musicInstance = RuntimeManager.CreateInstance(musicPath);
        

        previousScore = score;
    }

    private void Update()
    {
        if (previousScore != score) //Plays the collection sound when the player earns a point, i.e. when they collect food
        {
            //playCollectSound();

            //We ask FMOD bank to play one shot this path
            RuntimeManager.PlayOneShot(collectPath, this.transform.position);
            previousScore = score;
        }

        if (snake.gameStarted == true && startMusicOnce == false) //Starts the in-game music when the Fungus intro ends. If this doesn't work, you can just hard code it to begin on start in the inspector.
        {
            //snakeMusicSource.Play();
 
            Debug.Log("Music is now playing");
            startMusicOnce = true;
        }

        //fmod version of the above
        if(snake.gameStarted)
        {
            //Check if the music is playing or starting, then if it's not, start the music
            musicInstance.getPlaybackState(out currentMusicState);
            if (currentMusicState != PLAYBACK_STATE.PLAYING || currentMusicState != PLAYBACK_STATE.STARTING)
                musicInstance.start();
        }
    }

    public static int GetScore()
    {
        return score;
    }

    public static void AddScore()
    {
        score += 1;
    }


    public void playCollectSound()
    {
        //snakeAudioSource.PlayOneShot(collectSound, 1);

        RuntimeManager.PlayOneShot(collectPath, this.transform.position);
        Debug.Log("CollectedSound!");
    }


    public void playLoseSound()
    {
        //snakeAudioSource.PlayOneShot(gameOverSound, 1);
        RuntimeManager.PlayOneShot(gameOverPath, this.transform.position);
        Debug.Log("DeadSound!");
    }

}
