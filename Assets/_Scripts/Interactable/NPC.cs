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
            if (currentPlayer.CurrentInteractible == null)
            {
                Interact(currentPlayer);
            }
        }
    }

    public override void Interact(Player player)
    {
        base.Interact(player);

        Debug.Log(gameObject.name + "NPC Interact with " + currentPlayer.name);

        //Do extra stuff NPC do

    }
}
