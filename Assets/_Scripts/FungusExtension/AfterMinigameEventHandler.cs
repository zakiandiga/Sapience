using UnityEngine;

namespace Fungus
{
    [EventHandlerInfo("",
                      "OnAfterMinigames",
                      "The block will execute on Calling the block directly.")]
    [AddComponentMenu("")]
    public class AfterMinigameEventHandler : MonoBehaviour
    {
        public BlockReference targetBlock;


        private void Start()
        {
            Snake.OnMinigameEnd += CallingDialogue;
        }

        private void OnDisable()
        {
            Snake.OnMinigameEnd -= CallingDialogue;
        }

        private void CallingDialogue(BlockReference currentBlock)
        {
            targetBlock = currentBlock;
            targetBlock.Execute();
        }
    }
}
