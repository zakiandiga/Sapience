using UnityEngine;

public class AutomaticDoor : Interactable
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        currentPlayer = collision.GetComponent<Player>();
        if (currentPlayer.CurrentInteractable == null)
        {
            interactableState = InteractableState.isInteractable;
            Interact(currentPlayer);
        }
    }
}
