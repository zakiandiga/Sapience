using UnityEngine;
using FMOD.Studio;
using FMODUnity;
using Fungus;

public class MusicManager : MonoBehaviour
{
    private int currentParameter;

    private PLAYBACK_STATE isPlaying;

    private EventInstance musicInstance;
    private EventReference currentMusicPath;
    private EventReference nullMusicPath;

    private void OnEnable()
    {
        PlayMusicFMOD.OnStartMusic += StartMusic;
        PlayMusicFMOD.OnStopMusic += StopMusic;
    }

    private void OnDisable()
    {
        PlayMusicFMOD.OnStartMusic -= StartMusic;
        PlayMusicFMOD.OnStopMusic -= StopMusic;
    }

    private void StartMusic(EventReference soundPath)
    {
        if (currentMusicPath.Guid == soundPath.Guid || currentMusicPath.IsNull)
        {
            currentMusicPath.Guid = soundPath.Guid;

            musicInstance = RuntimeManager.CreateInstance(currentMusicPath);
            musicInstance.start();
            musicInstance.release();
        }
        else
        {
            Debug.Log("Another music is playing!");
        }
    }

    private void StopMusic(EventReference soundPath)
    {
        if (currentMusicPath.Guid == soundPath.Guid)
        {
            musicInstance.getPlaybackState(out isPlaying);
            if (isPlaying == PLAYBACK_STATE.PLAYING)
                StopMusic();

            currentMusicPath = nullMusicPath;
        }
        else
            Debug.LogError("Music you're trying to stop is not playing");
    }

    private void StopMusic()
    {
        musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}


