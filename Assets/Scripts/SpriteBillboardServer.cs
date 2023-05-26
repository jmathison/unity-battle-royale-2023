using Unity.Netcode;
using UnityEngine;

public class SpriteBillboardServer : NetworkBehaviour
{
    [Tooltip("Turn this off to allow the sprite to face the camera even as it moves up and down.")]
    [SerializeField] bool freezeXZAxis = true;
    // Update is called once per frame
    [ServerRpc(RequireOwnership = false)]
    void LateUpdateServerRpc()
    {
        if (freezeXZAxis)
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        else
            transform.rotation = Camera.main.transform.rotation;
    }
    void LateUpdate()
    {
        if (!IsServer)
            LateUpdateServerRpc();
    }
}
