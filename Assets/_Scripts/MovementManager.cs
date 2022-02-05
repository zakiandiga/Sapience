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

    public void LoadMinigame(string minigameSceneName)
    {
        //Handle loading screen here
        SceneManager.LoadSceneAsync(minigameSceneName, LoadSceneMode.Additive);
    
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
