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
        if (audioEvent.IsNull)
            return;

        if(currentSpeaker.isValid())
            return;

        else           
            currentSpeaker = RuntimeManager.CreateInstance(audioEvent);
 
        
        Play();
    }

    private void OnDisable()
    {
        if(currentSpeaker.isValid())
            currentSpeaker.release();  
    }

    protected void Play()
    {
        currentSpeaker.start();
        
    }

    protected void Stop()
    {
        if(currentSpeaker.isValid())
            currentSpeaker.release();
        //currentSpeaker.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

    }

    public void OnPause()
    {
        if (audioEvent.IsNull)
            return;

        Pause();
    }

    protected void Pause()
    {

    }
    public void OnResume()
    {
        if(audioEvent.IsNull)
            return;

        Resume();
    }

    protected void Resume()
    {

    }
    public void OnEnd(bool stopAudio)
    {

        if(stopAudio)
            Stop();
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
        if(audioEvent.IsNull)          
            currentSpeaker.release();
    }


    public void OnVoiceover(AudioClip voiceOverClip)
    {
        return;
    }
}
