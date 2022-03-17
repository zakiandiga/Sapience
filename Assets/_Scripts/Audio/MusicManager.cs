using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class MusicManager : MonoBehaviour
{
    [SerializeField]private PlayerInputHandler input;
    private int currentParameter;

    private EventInstance musicInstance;

    [SerializeField] private EventReference musicEventReference;

    [SerializeField] private string musicSwitch;

    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        if(input.IsJumpPressed)
        {
            //StartMusic();
        }

        if(input.Interacting)
        {
            currentParameter = (currentParameter == 0) ? 1 : 0;
            SwitchTrack(currentParameter);
        }
    }

    private void StartMusic()
    {
        var isPlaying = PLAYBACK_STATE.PLAYING; 
        musicInstance.getPlaybackState(out isPlaying);
        
        if (isPlaying == PLAYBACK_STATE.PLAYING)
            StopMusic();
        else
        {
            Debug.Log("PLAY");
            musicInstance = RuntimeManager.CreateInstance(musicEventReference);
            musicInstance.start();
            musicInstance.release();
        }
    }

    private void SwitchTrack(int section)
    {
        musicInstance.setParameterByName(musicSwitch, section);
    }

    private void StopMusic()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

}


