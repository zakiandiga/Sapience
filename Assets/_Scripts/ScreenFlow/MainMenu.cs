using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CurtainManager curtainManager;

    public void PlayGame()
    {
        CurtainManager.OnFinishFadeTo += OpenGameplayScene;
        curtainManager.FadeTo();
    }

    private void OpenGameplayScene(CurtainManager c)
    {
        CurtainManager.OnFinishFadeTo -= OpenGameplayScene;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //Load the game
    }

    public void QuitGame()
    {
        CurtainManager.OnFinishFadeTo += CloseApplication;
        curtainManager.FadeTo();
    }

    private void CloseApplication(CurtainManager c)
    {
        CurtainManager.OnFinishFadeTo -= CloseApplication;
        Application.Quit();
    }
}
