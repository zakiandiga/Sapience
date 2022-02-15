using UnityEngine;

namespace Fungus
{
    [EventHandlerInfo("",
                      "OnAfterMinigames",
                      "The block will execute on Calling the block directly.")]
    [AddComponentMenu("")]
    public class AfterMinigameEventHandler : EventHandler
    {
        private BlockReference targetBlock;


        private void Start()
        {
            MinigameBase.OnMinigameEnd += CallingDialogue;
        }

        private void OnDisable()
        {
            MinigameBase.OnMinigameEnd -= CallingDialogue;
        }

        private void CallingDialogue(BlockReference currentBlock)
        {
            targetBlock = currentBlock;
            if(targetBlock.block.BlockName == this.parentBlock.BlockName)
                targetBlock.Execute();
        }
    }
}
