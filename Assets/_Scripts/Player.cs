using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string PlayerName { get { return playerName; } private set { } }

    [SerializeField] private string playerName;

    ///States:
    ///idle
    ///moving
    ///talking

    



    // Update is called once per frame
    void Update()
    {
        
    }
}
