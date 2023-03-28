using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [Tooltip("Turn this off to allow the sprite to face the camera even as it moves up and down.")]
    [SerializeField] bool freezeXZAxis = true;
    // Update is called once per frame
    void LateUpdate()
    {
        if (freezeXZAxis)
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        else
            transform.rotation = Camera.main.transform.rotation;
    }
}
