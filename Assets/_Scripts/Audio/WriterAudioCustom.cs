using System.Collections.Generic;
using UnityEngine;
using Fungus;
using FMOD.Studio;
using FMODUnity;

public class WriterAudioCustom : MonoBehaviour, IWriterListener
{
    [SerializeField] protected EventReference audioEvent;
    private EventInstance currentSpeaker;

    private PLAYBACK_STATE currentPlaybackState;

    public void OnStart(AudioClip audioClip)
    {
        if (audioEvent.Path == null)
        {
            Debug.LogError("No audioEvent assign on " + this.gameObject);
            return;
        }

        if(currentSpeaker.isValid())
        {
            Debug.Log("No need to set currentSpeaker");
            return;
        }
        else
        {
            
            currentSpeaker = RuntimeManager.CreateInstance(audioEvent);
            Debug.Log("Set currentSpeaker to " + currentSpeaker);

        }
            Play();
    }

    private void OnDisable()
    {
        Debug.Log("at OnDisable");
        currentSpeaker.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        currentSpeaker.release();
    }

    protected void Play()
    {
        currentSpeaker.start();
        
    }

    protected void Stop()
    {
        //currentSpeaker.release();
        //currentSpeaker.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    }

    public void OnPause()
    {
        if (audioEvent.Path == null)
            return;

        Pause();
    }

    protected void Pause()
    {
        //pause
        Debug.Log("Pausing");
    }
    public void OnResume()
    {
        if(audioEvent.Path == null)
            return;

        Resume();
    }

    protected void Resume()
    {
        //resume
        Debug.Log("Resume");
    }
    public void OnEnd(bool stopAudio)
    {
        Debug.Log("OnEnd called!");
        Debug.Log("currentSpeaker is Valid = " + currentSpeaker.isValid());
        if(stopAudio)
        {
            Stop();
        }
    }


    public void OnAllWordsWritten()
    {
        return;
    }


    public void OnGlyph()
    {
        return;
    }

    public void OnInput()
    {
        if(audioEvent.Path != null)
        {
            
            currentSpeaker.release();
            currentSpeaker.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }




    public void OnVoiceover(AudioClip voiceOverClip)
    {
        return;
    }
}
