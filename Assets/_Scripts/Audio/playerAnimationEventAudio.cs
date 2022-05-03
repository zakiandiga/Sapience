using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class playerAnimationEventAudio : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private EventReference jump, step, land;
    private EventReference currentEvent;


    public void PlayAudio(int soundPathCode)
    {        
        switch (soundPathCode)
        {
            case 0:
                currentEvent = jump;
                break;

            case 1:
                currentEvent = step;
                break;
            case 3:
                currentEvent = land;
                break;

        }
        RuntimeManager.PlayOneShot(currentEvent);
    }
}
