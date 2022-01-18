using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static event Action<string> OnBlockEnd;
    public void PlayerMove(string blockName)
    {
        Debug.Log("Calling OnBlockEnd() with: " + blockName);
        OnBlockEnd?.Invoke(blockName);
    }
}
