using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

public class MovementManager : MonoBehaviour
{
    public static event Action<string> OnBlockEnd;
    public static event Action<string> OnBlockStart;
    public static event Action<Transform> OnSetPlayerSpawn;
    public static event Action<Flowchart> OnAnnounceFlowchart; //CHECK USAGE

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

    #region Event Subscribers
    private void FinalizeLoadMinigame(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= FinalizeLoadMinigame;
        SceneManager.SetActiveScene(scene);
        Debug.Log("OnMinigameLoaded: " + scene.name);

        cameraManager.SetMinigameCamera(true);

        //Handle fade out here
    }

    private void CloseMinigame(string sceneName)
    {
        cameraManager.SetMinigameCamera(false);
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));
        SceneManager.sceneUnloaded += FinalizeClosingMinigame;
    }

    private void FinalizeClosingMinigame(Scene scene)
    {
        SceneManager.sceneUnloaded -= FinalizeClosingMinigame;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));

        //enable player movement here
        PlayerMove("Minigame");
    }
    #endregion

    #region Fungus Invoke Methods
    public void SetPlayerPosition(Transform targetPosition) => OnSetPlayerSpawn?.Invoke(targetPosition);

    public void PlayerMove(string blockName) => OnBlockEnd?.Invoke(blockName);

    public void DisablePlayer(string blockName) => OnBlockStart?.Invoke(blockName);

    public void LoadDay(GameObject dayObject) => dayObject.SetActive(true);

    public void LoadMinigame(string minigameSceneName)
    {
        SceneManager.LoadSceneAsync(minigameSceneName, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += FinalizeLoadMinigame;
    }

    public void ChangeRoom(GameObject currentRoom, GameObject nextRoom)
    {
        //fade out
        currentRoom.SetActive(false);
        nextRoom.SetActive(true);
        //fade in
    }

    public void AnnouncingFlowchart(Flowchart flowchart) //Do we use this?
    {
        OnAnnounceFlowchart?.Invoke(flowchart);
    }
    #endregion
}
