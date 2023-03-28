using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DoorInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private InteractorEvent _onDoorOpen = new InteractorEvent();
    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        _onDoorOpen.Invoke(interactor);
        return true;
    }
}
