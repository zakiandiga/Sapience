using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDestroyTester : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log(gameObject.name + " loaded!");
    }

    private void OnDestroy()
    {
        Debug.Log(gameObject.name + " destroyed!");
    }
}
