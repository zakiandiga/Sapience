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
    private EventReference pathToUnload;
    private EventReference nullMusicPath;

    private StudioListener studioListener;

    private void OnEnable()
    {
        studioListener = GetComponent<StudioListener>();
        PlayMusicFMOD.OnStartMusic += StartMusic;
        PlayMusicFMOD.OnStopMusic += StopMusic;
    }

    private void OnDisable()
    {
        PlayMusicFMOD.OnStartMusic -= StartMusic;
        PlayMusicFMOD.OnStopMusic -= StopMusic;
        StopMusic();
    }

    private void DeassignPlayer(Transform player)
    {
        studioListener.attenuationObject = null;
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

        if(!currentMusicPath.IsNull && currentMusicPath.Guid != soundPath.Guid)
        {
            StopMusic();

            currentMusicPath.Guid = soundPath.Guid;
            musicInstance = RuntimeManager.CreateInstance(currentMusicPath);
            musicInstance.start();
            musicInstance.release();
        }

    }

    private void StopMusic(EventReference soundPath)
    {
        musicInstance.getPlaybackState(out isPlaying);
        if (isPlaying == PLAYBACK_STATE.PLAYING)
            StopMusic();

        currentMusicPath = nullMusicPath;
    }

    public void StopMusic()
    {
        if(!currentMusicPath.IsNull)
        {
            musicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusicPath = nullMusicPath;
        }

    }
}


