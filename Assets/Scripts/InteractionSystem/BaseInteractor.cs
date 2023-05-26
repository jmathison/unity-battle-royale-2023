using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] public class InteractorItemEvent : UnityEvent<Interactor, Transform> { }
public class BaseInteractor : MonoBehaviour, IInteractable
{
    [Tooltip("This is the text that displays when you're near the item.")]
    [SerializeField] private string _prompt;
    [Tooltip("This is the code that should run when the item is picked up.")]
    [SerializeField] private InteractorItemEvent _onPickUp = new InteractorItemEvent();
    [SerializeField] public Button.ButtonClickedEvent onClick = new Button.ButtonClickedEvent();
    public string InteractionPrompt => _prompt;
    public virtual Item item { get; set; }

    public virtual bool Interact(Interactor interactor)
    {
        _onPickUp.Invoke(interactor, transform);
        return true;
    }
}
