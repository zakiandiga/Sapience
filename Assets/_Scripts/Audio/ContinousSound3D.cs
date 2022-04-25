using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class ContinousSound3D : MonoBehaviour
{
    [SerializeField] private EventReference soundPath;

    private void Start()
    {
        if(!soundPath.IsNull)
        {

        }
    }
}
