using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

public class MovementManager : MonoBehaviour
{
    public static event Action<string> OnBlockEnd;
    public static event Action<string> OnBlockStart;

    public static event Action<Flowchart> OnAnnounceFlowchart;

    private string currentScene;
    private AsyncOperation minigameLoading;

    private void OnEnable()
    {
        currentScene = SceneManager.GetActiveScene().name;
        MinigameHandler.OnMinigameClosing += CloseMinigame;
    }
    private void OnDisable()
    {
        MinigameHandler.OnMinigameClosing -= CloseMinigame;
    }

    public void PlayerMove(string blockName)
    {
        Debug.Log("Calling OnBlockEnd() with: " + blockName);
        OnBlockEnd?.Invoke(blockName);
    }

    public void DisablePlayer(string blockName)
    {
        OnBlockStart?.Invoke(blockName);
    }

    public void LoadDay(GameObject dayObject)
    {
        dayObject.SetActive(true);
    }

    private void CloseMinigame(string sceneName)
    {
        Debug.Log("CloseMinigame CALLED!");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
        SceneManager.UnloadSceneAsync(sceneName);

        //enable player movement here
        PlayerMove(sceneName);
    }

    public void LoadMinigame(string minigameSceneName)
    {
        //Handle loading screen here
        //StartCoroutine(LoadMinigameAsync(minigameSceneName));
        SceneManager.LoadSceneAsync(minigameSceneName, LoadSceneMode.Additive);

        SceneManager.sceneLoaded += OnMinigameLoaded;
    }

    private void OnMinigameLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);

        //Handle fade out here
    }


    public void AnnouncingFlowchart(Flowchart flowchart)
    {
        OnAnnounceFlowchart?.Invoke(flowchart);
    }

    public void ChangeRoom(GameObject currentRoom, GameObject nextRoom)
    {
        //fade out
        currentRoom.SetActive(false);
        nextRoom.SetActive(true);
        //fade in
    }
}
