using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

public class MovementManager : MonoBehaviour
{
    public static event Action<string> OnBlockEnd;
    public static event Action<string> OnBlockStart;
    public static event Action<Transform, int> OnSetPlayerSpawn;
    public static event Action<Flowchart> OnAnnounceFlowchart; //CHECK USAGE

    private CameraManager cameraManager;

    private Flowchart currentFlowchart;

    private string currentStoryScene;
    private string currentMinigameScene;

    [SerializeField] private CurtainManager curtainManager;

    private GameObject roomToActivate;
    private GameObject roomToDeactivate;

    private void Start()
    {
        cameraManager = GetComponent<CameraManager>();
        if(!SceneManager.GetSceneByName("AudioManager").isLoaded)
        {
            SceneManager.LoadSceneAsync("AudioManager", LoadSceneMode.Additive);
        }
    }

    private void OnEnable()
    {
        currentStoryScene = SceneManager.GetActiveScene().name;
    }

    #region Event Subscribers
    private void StartLoadingMinigame(CurtainManager c)
    {
        CurtainManager.OnFinishFadeTo -= StartLoadingMinigame;
        SceneManager.LoadSceneAsync(currentMinigameScene, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += FinalizeLoadMinigame;
    }

    private void FinalizeLoadMinigame(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= FinalizeLoadMinigame;
        SceneManager.SetActiveScene(scene);

        MinigameBase.OnMinigameClose += CloseMinigame;
        cameraManager.OnCameraBlendingStart += WaitingCameraBlendToMinigame;
        cameraManager.SetMinigameCamera(true);


        //Handle fade out here
    }

    private void WaitingCameraBlendToMinigame(bool cameraBlend)
    {
        cameraManager.OnCameraBlendingStart -= WaitingCameraBlendToMinigame;
        cameraManager.OnCameraBlendingFinish += RevealMinigame;
    }

    private void RevealMinigame(bool b)
    {
        cameraManager.OnCameraBlendingFinish -= RevealMinigame;
        curtainManager.FadeFrom();
    }

    private void WaitingCameraBlendFromMinigame(bool cameraBlend)
    {
        cameraManager.OnCameraBlendingFinish -= WaitingCameraBlendFromMinigame;

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(currentMinigameScene));
        SceneManager.sceneUnloaded += FinalizeClosingMinigame;
    }

    private void CloseMinigame(string sceneName)
    {
        cameraManager.OnCameraBlendingFinish += WaitingCameraBlendFromMinigame;
        cameraManager.SetMinigameCamera(false);
        curtainManager.FadeTo();
        //put transition here        
        //waiting for the OnCameraBlending(false) event
    }

    private void FinalizeClosingMinigame(Scene scene)
    {
        SceneManager.sceneUnloaded -= FinalizeClosingMinigame;
        MinigameBase.OnMinigameClose -= CloseMinigame;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentStoryScene));

        currentMinigameScene = null;

        CurtainManager.OnFinishFadeFrom += ReactivateFlowchart;
        curtainManager.FadeFrom();
    }

    private void ReactivateFlowchart(CurtainManager c)
    {
        CurtainManager.OnFinishFadeFrom -= ReactivateFlowchart;
        currentFlowchart.SetBooleanVariable("OnMinigame", false);
    }
    #endregion

    #region Fungus Invoke Methods
    public void SetPlayerPosition(Transform targetPosition, int characterCode) => OnSetPlayerSpawn?.Invoke(targetPosition, characterCode);

    public void PlayerMove(string blockName) => OnBlockEnd?.Invoke(blockName);

    public void DisablePlayer(string blockName) => OnBlockStart?.Invoke(blockName);

    public void LoadDay(GameObject dayObject) => dayObject.SetActive(true);

    public void LoadMinigame(string minigameSceneName, Flowchart flowchart)
    {
        currentFlowchart = flowchart;
        currentStoryScene = SceneManager.GetActiveScene().name;
        currentMinigameScene = minigameSceneName;
        CurtainManager.OnFinishFadeTo += StartLoadingMinigame;
        curtainManager.FadeTo();
    }

    public void ChangeRoom(GameObject currentRoom, GameObject nextRoom)
    {
        roomToDeactivate = currentRoom;
        roomToActivate = nextRoom;
        CurtainManager.OnFinishFadeTo += DisableCurrentRoom;
        curtainManager.FadeTo();
    }

    private void DisableCurrentRoom(CurtainManager c)
    {
        CurtainManager.OnFinishFadeTo -= DisableCurrentRoom;
        roomToDeactivate.SetActive(false);
        roomToActivate.SetActive(true);
        curtainManager.FadeFrom();
    }

    public void AnnouncingFlowchart(Flowchart flowchart) //Do we use this?
    {
        OnAnnounceFlowchart?.Invoke(flowchart);
    }
    #endregion
}
