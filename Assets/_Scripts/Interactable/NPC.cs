using System;
using Fungus;
using UnityEngine;

public class NPC : Interactable
{
    [SerializeField] private bool isBarking = false;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isBarking)
        {
            base.OnTriggerEnter2D(collision);
            return;
        }

        currentPlayer = collision.GetComponent<Player>();
        if (currentPlayer.CurrentInteractable == null && interactableState == InteractableState.idle)
        {
            interactableState = InteractableState.isInteractable;
            Interact(currentPlayer);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if(isBarking)
        {              
            currentBlockReference.block.Stop();
            interactableState = InteractableState.idle;
        }

        base.OnTriggerExit2D(collision);
    }

    public void SetBarking()
    {
        isBarking = true;
    }
}
