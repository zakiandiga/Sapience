using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace Fungus
{
    [CommandInfo("Audio",
                 "Play FMOD Sound",
                 "Play sound(s) with FMOD")]
    [AddComponentMenu("")]
    public class PlaySoundFMOD : Command
    {
        [SerializeField] private bool waitUntilFinished;

        [SerializeField] private EventReference soundPath;
        private EventInstance soundInstance;

        private PLAYBACK_STATE currentPlaybackState;

        public override void OnEnter()
        {
            if(soundPath.Path != null)
            {
                soundInstance = RuntimeManager.CreateInstance(soundPath);
                soundInstance.start();
            }
            else if(soundPath.Path == null)
            {
                Debug.LogError("Sound path not set");
            }

            if (waitUntilFinished)
            {
                soundInstance.getPlaybackState(out currentPlaybackState);
                StartCoroutine(FinishingSound());
            }
            else
                Continue();
        }

        public override void OnExit()
        {
            if(soundInstance.isValid())
                soundInstance.release();
        }

        private IEnumerator FinishingSound()
        {
            while (currentPlaybackState != PLAYBACK_STATE.STOPPED)
            {
                soundInstance.getPlaybackState(out currentPlaybackState);
                yield return null;
            }

            Continue();
        }

        public override string GetSummary()
        {
            if(soundPath.Path == null)
            {
                return "Error: Sound Path is empty";
            }

            return soundPath.Path;
        }

        public override Color GetButtonColor()
        {
            return new Color32(58, 136, 255, 255);
        }
    }



}

