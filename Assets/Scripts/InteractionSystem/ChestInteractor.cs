using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] public class InteractorEvent : UnityEvent<Interactor> { };
public class ChestInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    [SerializeField] private InteractorEvent _onOpen;
    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        _onOpen.Invoke(interactor);
        return true;
    }
}
