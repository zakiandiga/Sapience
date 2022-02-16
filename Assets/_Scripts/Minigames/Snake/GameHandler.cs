using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;

    private static int score;
    
    [SerializeField] public Snake snake;

    private LevelGrid levelGrid;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler.Start");

        levelGrid = new LevelGrid(20, 20);

        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
    }

    public static int GetScore()
    {
        return score;
    }

    public static void AddScore()
    {
        score += 1;
    }

}
