using System;
using Fungus;
using UnityEngine;

public class NPC : Interactable
{
    [SerializeField] private bool isBarking = false;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isBarking)
            base.OnTriggerEnter2D(collision);

        else if(isBarking)
        {
            currentPlayer = collision.GetComponent<Player>();
            if (currentPlayer.CurrentInteractable == null && interactableState == InteractableState.idle)
            {
                interactableState = InteractableState.isInteractable;
                Interact(currentPlayer);
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (!isBarking)
            base.OnTriggerExit2D(collision);

        else
        {            
            if(currentBlockReference.block.IsExecuting())
                currentBlockReference.block.Stop();

            interactableState = InteractableState.idle;
            base.OnTriggerExit2D(collision);
        }
    }

    public override void Interact(Player player)
    {

        base.Interact(player);

        //Debug.Log(gameObject.name + "NPC Interact with " + currentPlayer.gameObject.name);

        //Do extra stuff NPC do

    }

    public void SetBarking()
    {
        isBarking = true;
    }
}
