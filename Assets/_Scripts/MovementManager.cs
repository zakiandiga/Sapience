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

    public void SetScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }

    public void AnnouncingFlowchart(Flowchart flowchart)
    {
        OnAnnounceFlowchart?.Invoke(flowchart);
    }
}
