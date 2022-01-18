using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

namespace Fungus
{
    [EventHandlerInfo("",
                      "OnDirectDialogue",
                      "The block will execute on Calling the block directly.")]
    [AddComponentMenu("")]
    public class DirectBlockEventHandler : EventHandler
    {
        public BlockReference targetBlock;

        private void Start()
        {
            PlayerController.OnCallingDialogue += CallingDialogue;
        }

        private void OnDisable()
        {
            PlayerController.OnCallingDialogue -= CallingDialogue;
        }

        private void CallingDialogue(BlockReference currentBlock)
        {
            targetBlock = currentBlock;
            targetBlock.Execute();
        }
    }
}

