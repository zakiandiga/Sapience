using System;
using UnityEngine;
using Fungus;

public class MinigameBase : MonoBehaviour
{
    [SerializeField] protected BlockReference targetBlock;

    public static event Action<BlockReference> OnMinigameEnd;
    public static event Action<string> OnMinigameClose;

    protected void EndingMinigame()
    {
        OnMinigameEnd(targetBlock);
    }

    public void ClosingMinigame()
    {
        OnMinigameClose?.Invoke(this.name);
    }
}
