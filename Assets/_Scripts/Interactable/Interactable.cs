using System;
using UnityEngine;
using Fungus;

public class Interactable : MonoBehaviour
{
    protected bool isInteractable;

    protected Player currentPlayer;
    public BlockReference currentBlockReference;

    public static event Action<BlockReference> OnCallingDialogue;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        currentPlayer = collision.GetComponent<Player>();
        if (currentPlayer.CurrentInteractible == null)
        {
            currentPlayer.SetInteractible(this);
            isInteractable = true;
            currentPlayer.OnPlayerInteract += Interact;
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (currentPlayer == null)
        {
            currentPlayer = collision.GetComponent<Player>();
            if (currentPlayer.CurrentInteractible == null)
            {
                currentPlayer.SetInteractible(this);
                isInteractable = true;
                currentPlayer.OnPlayerInteract += Interact;
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (currentPlayer != null)
        {
            currentPlayer.OnPlayerInteract -= Interact;
            currentPlayer.SetInteractible(null);
            currentPlayer = null;
            isInteractable = false;            
        }
    }
    

    public virtual void Interact(Player player)
    {
        Debug.Log("Base class call Interact()");
        OnCallingDialogue?.Invoke(currentBlockReference);
    }
    
}
