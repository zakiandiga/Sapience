using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        previousScore = score;
    }

    private void Update()
    {
        if (previousScore != score) //Plays the collection sound when the player earns a point, i.e. when they collect food
        {
            playCollectSound();
            previousScore = score;
        }

        if (snake.gameStarted == true && startMusicOnce == false) //Starts the in-game music when the Fungus intro ends. If this doesn't work, you can just hard code it to begin on start in the inspector.
        {
            snakeMusicSource.Play();
            Debug.Log("Music is now playing");
            startMusicOnce = true;
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
        snakeAudioSource.PlayOneShot(collectSound, 1);
        Debug.Log("CollectedSound!");
    }


    public void playLoseSound()
    {
        snakeAudioSource.PlayOneShot(gameOverSound, 1);
        Debug.Log("DeadSound!");
    }

}
