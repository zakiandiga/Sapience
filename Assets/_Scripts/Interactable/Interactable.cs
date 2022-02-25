using System;
using UnityEngine;
using Fungus;

public class Interactable : MonoBehaviour
{
    protected InteractableState interactableState = InteractableState.idle;

    protected Player currentPlayer;
    [SerializeField] protected GameObject interactIcon;

    public BlockReference currentBlockReference;

    public static event Action<BlockReference> OnCallingDialogue;

    protected void Start()
    {
        InteractIconSwitch(false);
    }

    protected void OnEnable()
    {
        interactableState = InteractableState.idle;
    }


    protected void OnDisable()
    {
        if (interactableState != InteractableState.idle)
            interactableState = InteractableState.idle;

        if (currentPlayer != null)
        {
            currentPlayer.OnPlayerInteract -= Interact;
            InteractIconSwitch(false);
            currentPlayer = null;
        }        

        MovementManager.OnBlockEnd -= SwitchingInteractIconFromBlock;
    }

    protected void SwitchingInteractIconFromBlock(string blockName)
    {
        MovementManager.OnBlockEnd -= SwitchingInteractIconFromBlock;
        interactableState = InteractableState.idle;
        //InteractIconSwitch(true);
    }

    protected void InteractIconSwitch(bool status)
    {
        if(interactIcon != null)
            interactIcon.SetActive(status);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        currentPlayer = collision.GetComponent<Player>();
        if (currentPlayer.CurrentInteractable == null && interactableState == InteractableState.idle)
        {
            currentPlayer.SetInteractible(this);
            interactableState = InteractableState.isInteractable;
            InteractIconSwitch(true);
            currentPlayer.OnPlayerInteract += Interact;
        }
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (currentPlayer == null)
        {
            currentPlayer = collision.GetComponent<Player>();
            if (currentPlayer.CurrentInteractable == null && interactableState == InteractableState.idle)
            {
                currentPlayer.SetInteractible(this);
                interactableState = InteractableState.isInteractable;
                InteractIconSwitch(true);
                currentPlayer.OnPlayerInteract += Interact;
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (currentPlayer != null && interactableState == InteractableState.isInteractable)
        {
            currentPlayer.OnPlayerInteract -= Interact;            
            interactableState = InteractableState.idle;
            currentPlayer.SetInteractible(null);
            InteractIconSwitch(false);
            currentPlayer = null;
            //isInteractable = false;
            
        }
    }

    public virtual void Interact(Player player)
    {
        if(interactableState == InteractableState.isInteractable)
        {
            interactableState = InteractableState.interacting;

            MovementManager.OnBlockEnd += SwitchingInteractIconFromBlock;

            InteractIconSwitch(false);
            OnCallingDialogue?.Invoke(currentBlockReference);        

        }
    }
}

public enum InteractableState
{ 
    idle,
    isInteractable,
    interacting,
    disabled
}
