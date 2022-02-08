using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverTest : MonoBehaviour
{
    [SerializeField] private MinigameHandler minigameHandler;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        minigameHandler.CloseMinigame();
    }
}
