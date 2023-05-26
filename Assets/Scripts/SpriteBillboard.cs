using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class SpriteBillboard : NetworkBehaviour {
    [Tooltip("Turn this off to allow the sprite to face the camera even as it moves up and down.")]
    [SerializeField] bool freezeXZAxis = true;

    public bool useFrontAndBackSprites = false;
    public Sprite frontSprite;
    public Sprite backSprite;

    // Start is called before the first frame update
    void Start() {
        if (useFrontAndBackSprites) {
            if (IsOwner) {
                GetComponent<SpriteRenderer>().sprite = backSprite;
            }
            else {
                GetComponent<SpriteRenderer>().sprite = frontSprite;
            }
        }
    } 
    // Update is called once per frame
    void LateUpdate()
    {
        if (freezeXZAxis)
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        else
            transform.rotation = Camera.main.transform.rotation;
    }
}
