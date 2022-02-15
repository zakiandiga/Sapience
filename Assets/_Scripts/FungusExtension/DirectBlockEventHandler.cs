using UnityEngine;

namespace Fungus
{
    [EventHandlerInfo("",
                      "OnDirectDialogue",
                      "The block will execute on Calling the block directly.")]
    [AddComponentMenu("")]
    public class DirectBlockEventHandler : EventHandler
    {
        private BlockReference targetBlock;

        private void Start()
        {
            Interactable.OnCallingDialogue += CallingDialogue;
        }

        private void OnDisable()
        {
            Interactable.OnCallingDialogue -= CallingDialogue;
        }
       
        private void CallingDialogue(BlockReference currentBlock)
        {            
            targetBlock = currentBlock;
            if(currentBlock.block.BlockName == this.parentBlock.BlockName)
                targetBlock.Execute();
        }    
    }
}

