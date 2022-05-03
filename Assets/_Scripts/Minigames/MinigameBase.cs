using System;
using UnityEngine;
using Fungus;
using UnityEngine.SceneManagement;

public class MinigameBase : MonoBehaviour
{
    [SerializeField] protected BlockReference targetBlock;
    private string minigameScene;

    public static event Action<BlockReference> OnMinigameEnd;
    public static event Action<string> OnMinigameClose;


    protected void EndingMinigame()
    {
        OnMinigameEnd(targetBlock);
    }

    public void ClosingMinigame()
    {
        minigameScene = SceneManager.GetActiveScene().name;
        OnMinigameClose?.Invoke(minigameScene);        
    }
}
