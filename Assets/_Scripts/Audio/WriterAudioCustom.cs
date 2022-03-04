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

        currentSpeaker = RuntimeManager.CreateInstance(audioEvent);

        Play();
    }

    protected void Play()
    {
        currentSpeaker.start();
        
    }

    protected void Stop()
    {
        currentSpeaker.release();
        currentSpeaker.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

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
