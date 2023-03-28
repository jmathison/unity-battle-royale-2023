using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable] public class InteractorItemEvent : UnityEvent<Interactor, Transform> { }
public class ItemInteractor : MonoBehaviour, IInteractable
{
    [Tooltip("This is the text that displays when you're near the item.")]
    [SerializeField] private string _prompt;
    [Tooltip("This is the code that should run when the item is picked up.")]
    [SerializeField] private InteractorItemEvent _onPickUp = new InteractorItemEvent();
    [SerializeField] public Button.ButtonClickedEvent onClick = new Button.ButtonClickedEvent();
    public string InteractionPrompt => _prompt;
    public Item item;
    public int uses = 1;
    // Start is called before the first frame update
    void Start()
    {
        onClick.AddListener(() => { uses -= 1;
            Debug.Log(uses);
            if (uses <= 0)
            {
                transform.root.GetComponent<PlayerInventory>().Remove(gameObject);
            }
        });
    }

    public bool Interact(Interactor interactor)
    {
        _onPickUp.Invoke(interactor, transform);
        return true;
    }
}
