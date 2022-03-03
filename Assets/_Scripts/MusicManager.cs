using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    public static MusicManager Instance => _instance;

    [SerializeField] private List<AudioSource> musics;

    [SerializeField] private List<Music> musicList;

    private Music currentlyPlay;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
            _instance = this;
    }

    public void PlayMusic(string soundName)
    {
        Music m = musicList.Find(m => m.name == soundName);
        if(m == null)
        {
            Debug.Log("Cannot find " + soundName + " in musicList");
            return;
        }

        if (!m.audioSource.isPlaying)
        {
            if(currentlyPlay != null && currentlyPlay.audioSource.isPlaying)
            {
                currentlyPlay.audioSource.Stop();
                currentlyPlay = null;
            }

            currentlyPlay = m;
            currentlyPlay.audioSource.Play();
        }
        else
        {
            Debug.Log("This music is already playing!");
        }
    }

    public void StopMusic()
    {
        if (currentlyPlay != null)
        {
            currentlyPlay.audioSource.Stop();
        }
        else
            Debug.Log("No music to stop!");
    }

}

[System.Serializable]
public class Music
{
    public string name;
    //public AudioClip clip;
    public AudioSource audioSource;    
}
