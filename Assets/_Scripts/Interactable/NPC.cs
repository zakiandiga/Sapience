using System;
using Fungus;
using UnityEngine;

public class NPC : Interactable
{     
    public override void Interact(Player player)
    {
        base.Interact(player);

        Debug.Log(gameObject.name + "NPC Interact with " + currentPlayer.name);

        //Do extra stuff NPC do

    }
}
