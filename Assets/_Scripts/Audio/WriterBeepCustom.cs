using UnityEngine;
using Fungus;
using FMOD.Studio;
using FMODUnity;

public class WriterBeepCustom : MonoBehaviour, IWriterListener
{
    [SerializeField] protected EventReference audioEvent;
    private EventInstance currentSpeaker;

    private void OnDisable()
    {
        currentSpeaker.release();
    }

    public void OnStart(AudioClip audioClip)
    {
        if (audioEvent.Path == null)
        {
            Debug.LogError("No audioEvent assigned on " + this.gameObject);
            return;
        }

        currentSpeaker = RuntimeManager.CreateInstance(audioEvent);
    }

    public void OnGlyph()
    {
        Play();
    }

    private void Play()
    {
        currentSpeaker.start();
    }

    public void OnEnd(bool stopAudio)
    {
        if(stopAudio)
        {
            Stop();
        }
    }

    private void Stop()
    {
        return;
    }

    public void OnInput()
    {
        //Use this for handling click sounds during an ongoind dialogue interruption
        return;
    }

    public void OnPause()
    {
        currentSpeaker.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        return;        
    }

    public void OnResume()
    {
        return;
    }

    public void OnVoiceover(AudioClip voiceOverClip)
    {
        Debug.LogError("dialogue set to Voiceover on the flowchart");
        return;
    }

    public void OnAllWordsWritten()
    {
        return;
    }

}
