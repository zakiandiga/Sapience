using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

namespace Fungus
{
    /// <summary>
    /// The block will execute when the game starts playing.
    /// </summary>
    [EventHandlerInfo("",
                      "OnCallingDialogue",
                      "The block will execute on OnCallingDialogue.")]
    [AddComponentMenu("")]
    public class CustomEventHandler : EventHandler
    {
        private void Start()
        {
            PlayerController.OnCallingDialogue += CallingDialogue;
        }

        private void OnDisable()
        {
            PlayerController.OnCallingDialogue -= CallingDialogue;
        }

        private void CallingDialogue(BlockReference playerName)
        {
            ExecuteBlock();
        }
    }

}
