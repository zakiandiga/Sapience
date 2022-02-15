using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

public class MovementManager : MonoBehaviour
{
    public static event Action<string> OnBlockEnd;
    public static event Action<string> OnBlockStart;
    public static event Action<Flowchart> OnAnnounceFlowchart;

    private CameraManager cameraManager;

    private string currentScene;

    private void Start()
    {
        cameraManager = GetComponent<CameraManager>();
    }

    private void OnEnable()
    {
        currentScene = SceneManager.GetActiveScene().name;
        MinigameBase.OnMinigameClose += CloseMinigame;
    }
    private void OnDisable()
    {
        MinigameBase.OnMinigameClose -= CloseMinigame;
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
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
        SceneManager.UnloadSceneAsync(sceneName);

        cameraManager.SetMinigameCamera(false);
        //enable player movement here
        PlayerMove(sceneName);
    }

    public void LoadMinigame(string minigameSceneName)
    {
        //Handle loading screen here
        SceneManager.LoadSceneAsync(minigameSceneName, LoadSceneMode.Additive);

        SceneManager.sceneLoaded += OnMinigameLoaded;
    }

    private void OnMinigameLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);

        cameraManager.SetMinigameCamera(true);

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
