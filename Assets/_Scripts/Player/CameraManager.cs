using System;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private GameObject virtualCam;
    private CinemachineBrain cinemachineBrain;
    private CinemachineVirtualCamera cinemachine;
    private Transform currentPlayer;

    public event Action<bool> OnCameraBlendingStart;
    public event Action<bool> OnCameraBlendingFinish;

    private bool cameraBlending = false;

    private void Start()
    {
        mainCam = Camera.main;
        cinemachineBrain = mainCam.GetComponent<CinemachineBrain>();
        cinemachine = virtualCam.GetComponent<CinemachineVirtualCamera>();
    }

    private void OnEnable()
    {
        Player.OnSetPlayerPosition += AssignPlayer;
        Player.OnPlayerDisabled += DeassignPlayer;
    }

    private void OnDisable()
    {
        Player.OnSetPlayerPosition -= AssignPlayer;
        Player.OnPlayerDisabled -= DeassignPlayer;
    }

    public void AssignPlayer(Transform player)
    {
        if(currentPlayer != player)
            currentPlayer = player;

        cinemachine.m_Follow = currentPlayer;
    }

    private void DeassignPlayer(Transform player) => cinemachine.m_Follow = null;

    public void SetMinigameCamera(bool inMinigame)
    {
        if (inMinigame)
            cinemachine.Priority = 0;
        else
            cinemachine.Priority = 10;
    }

    private void Update()
    {    
        if (cinemachineBrain.IsBlending && !cameraBlending)
        {
            //Debug.Log("Camera start blending");
            cameraBlending = true;
            OnCameraBlendingStart?.Invoke(true);
        }
        else if (!cinemachineBrain.IsBlending && cameraBlending)
        {
            //Debug.Log("Camera STOP blending");
            cameraBlending = false;
            OnCameraBlendingFinish?.Invoke(false);
        }
    }

    #region Fungus Invoke Methods

    public void LookAtCharacter(Transform characterTransform, float zoomLevel)
    {
        //cameraManager.AssignPlayer(characterTransform);
        cinemachine.m_Follow = characterTransform;
        cinemachine.m_Lens.OrthographicSize = zoomLevel;
        //cinemachine.m_Transitions.m_BlendHint = 
        //Normal orthographic size is 4
    }

    #endregion

}
