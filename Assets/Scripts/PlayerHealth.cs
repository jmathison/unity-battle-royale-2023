using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour, IDamageable {

    public float MaxHealth;

    [SerializeField]
    private float Health;

    public NetworkVariable<float> _Health = new NetworkVariable<float>(0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [Header("Client Events")]
    public UnityEvent onDeath = new UnityEvent();
    [Header("Server Events")]
    public UnityEvent onDeathServer = new UnityEvent();
    public UnityEvent beforeHealthChanged = new UnityEvent();
    public UnityEvent afterHealthChanged = new UnityEvent();
    [SerializeField] private Slider healthSlider;

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            GameManager.Instance.ChangeNameOnServer();
        }
        // Health starts at max health
        Health = MaxHealth;
        // Set network variable
        if (IsServer) {
            _Health.Value = Health;
        }
        // Find & assign the health slider
        if (IsOwner) {
            healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>();
        }
        // Update health bar position
        this.UpdateHealthBar();
    }

    // Start is called before the first frame update
    void Start() {
        // EMPTY
    }

    // Update is called once per frame
    void Update() {
        if (IsServer) {
            _Health.Value = Health;
        }
        else if (IsClient) {
            this.SetHealth(_Health.Value);
        }
        if (IsDead() && IsOwner) {
            onDeath.Invoke();
            OnDeathServerRpc();
        }
    }

    [ServerRpc()]
    public void OnDeathServerRpc() {
        onDeathServer.Invoke();
    }

    // Called when the player takes damage
    public void Damage(float damage) {
        this.SetHealth(Health - damage);
    }

    // Called when the player gains health
    public void Heal(float heal) {
        this.SetHealth(Health + heal);
    }

    // Check if the player is dead (less than or equal to 0 health)
    public bool IsDead() {
        return (Health <= 0.0f) ? true : false;
    }

    // Getter function for health
    public float GetHealth() {
        return Health;
    }

    // Setter function for health
    public void SetHealth(float newHealth) {
        beforeHealthChanged.Invoke();
        Health = Mathf.Clamp(newHealth, 0, MaxHealth);
        afterHealthChanged.Invoke();
        this.UpdateHealthBar();

    }

    // Update health bar position
    private void UpdateHealthBar() {
        if (!IsOwner) return;
        healthSlider.value = Health / MaxHealth;
    }
}
