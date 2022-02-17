using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject virtualCam;
    private CinemachineVirtualCamera cinemachine;
    private Transform currentPlayer;

    private void Start()
    {
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

    private void AssignPlayer(Transform player)
    {
        Debug.Log("Assign player " + player.gameObject.name);
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
}
