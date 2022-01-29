using UnityEngine;

public class AutomaticDoor : Interactable
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        currentPlayer = collision.GetComponent<Player>();
        if (currentPlayer.CurrentInteractible == null)
        {
            Interact(currentPlayer);
        }
    }
}
