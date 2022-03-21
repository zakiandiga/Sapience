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

    private Flowchart currentFlowchart;

    private string currentStoryScene;
    private string currentMinigameScene;

    private void Start()
    {
        cameraManager = GetComponent<CameraManager>();
        if(!SceneManager.GetSceneByName("AudioManager").isLoaded)
        {
            Debug.Log("Loading AudioManager");
            SceneManager.LoadScene("AudioManager", LoadSceneMode.Additive);
        }
    }

    private void OnEnable()
    {
        currentStoryScene = SceneManager.GetActiveScene().name;

    }
    private void OnDisable()
    {

    }    

    #region Event Subscribers
    private void FinalizeLoadMinigame(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= FinalizeLoadMinigame;
        SceneManager.SetActiveScene(scene);
        Debug.Log("OnMinigameLoaded: " + currentMinigameScene);

        MinigameBase.OnMinigameClose += CloseMinigame;
        cameraManager.OnCameraBlendingStart += WaitingCameraBlendToMinigame;
        cameraManager.SetMinigameCamera(true);

        //Handle fade out here
    }

    private void WaitingCameraBlendToMinigame(bool cameraBlend)
    {
        cameraManager.OnCameraBlendingStart -= WaitingCameraBlendToMinigame;

        if (cameraBlend)
        {
            Debug.Log("Camera is transitioning");
        }
    }

    private void WaitingCameraBlendFromMinigame(bool cameraBlend)
    {
        cameraManager.OnCameraBlendingFinish -= WaitingCameraBlendFromMinigame;
        Debug.Log("Camera blending done!");
        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(currentMinigameScene));
        SceneManager.sceneUnloaded += FinalizeClosingMinigame;
    }

    private void CloseMinigame(string sceneName)
    {
        Debug.Log("CLOSING MINIGAME, Waiting for camera blending to finish");
        cameraManager.OnCameraBlendingFinish += WaitingCameraBlendFromMinigame;
        cameraManager.SetMinigameCamera(false);
        //put transition here        
        //waiting for the OnCameraBlending(false) event
    }

    private void FinalizeClosingMinigame(Scene scene)
    {
        Debug.Log("Finalizing minigame");
        SceneManager.sceneUnloaded -= FinalizeClosingMinigame;
        MinigameBase.OnMinigameClose -= CloseMinigame;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentStoryScene));

        //enable player movement here
        //PlayerMove("Minigame");
        currentMinigameScene = null;
        currentFlowchart.SetBooleanVariable("OnMinigame", false);
    }
    #endregion

    #region Fungus Invoke Methods
    public void SetPlayerPosition(Transform targetPosition) => OnSetPlayerSpawn?.Invoke(targetPosition);

    public void PlayerMove(string blockName) => OnBlockEnd?.Invoke(blockName);

    public void DisablePlayer(string blockName) => OnBlockStart?.Invoke(blockName);

    public void LoadDay(GameObject dayObject) => dayObject.SetActive(true);

    public void LoadMinigame(string minigameSceneName, Flowchart flowchart)
    {
        currentFlowchart = flowchart;
        currentMinigameScene = minigameSceneName;
        SceneManager.LoadSceneAsync(currentMinigameScene, LoadSceneMode.Additive);
        SceneManager.sceneLoaded += FinalizeLoadMinigame;
    }

    public void PlayMusic(string musicName)
    {
        //MusicManager.Instance.PlayMusic(musicName);
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
