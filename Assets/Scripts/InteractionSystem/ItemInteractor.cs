using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemInteractor : BaseInteractor
{
    //Change this later to reference consumable item scriptable object and tie in the uses to be private and based on item.uses.
    public int uses = 1;
    // Start is called before the first frame update
    void Start()
    {
        onClick.AddListener(() => { uses -= 1;
            if (uses <= 0)
            {
                transform.root.GetComponent<PlayerInventory>().Remove(gameObject);
            }
        });
    }
}
