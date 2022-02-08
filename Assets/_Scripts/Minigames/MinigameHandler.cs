using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameHandler : MonoBehaviour
{
    public static event Action<string> OnMinigameClosing;

    private string currentSceneName;

    public void CloseMinigame()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        OnMinigameClosing?.Invoke(currentSceneName);
    }
}
