using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

namespace Fungus
{
    [CommandInfo("Audio",
                 "Play FMOD Music",
                 "Start BGM during fmod block execution")]
    [AddComponentMenu("")]
    public class PlayMusicFMOD : Command
    {
        [SerializeField] private CommandType commandType = CommandType.Start;
        [SerializeField] private EventReference soundPath;

        public static event Action<EventReference> OnStartMusic;
        public static event Action<EventReference> OnStopMusic;

        public override void OnEnter()
        {
            if (!soundPath.IsNull)
            {
                if (commandType == CommandType.Start)
                    OnStartMusic?.Invoke(soundPath);

                else
                    OnStopMusic?.Invoke(soundPath);
            }
            else
                Debug.LogError("Sound path not found at: " + this.ParentBlock.BlockName);

            Continue();
        }

        public override string GetSummary()
        {
            if (soundPath.IsNull)
            {
                return "Error: Sound Path is empty";
            }

            return soundPath.Guid.ToString();            
        }

        public override Color GetButtonColor()
        {
            return new Color32(58, 136, 255, 255);
        }

        public enum CommandType
        {
            Start,
            Stop
        }
    }

}
