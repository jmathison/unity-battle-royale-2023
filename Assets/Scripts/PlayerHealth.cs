using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour, IDamageable
{
    public NetworkVariable<float> maxHealth = new NetworkVariable<float>(10.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public NetworkVariable<float> health = new NetworkVariable<float>(10.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private Slider healthSlider;

    public override void OnNetworkSpawn()
    {
        GameManager.Instance.Test();
    }
    // Start is called before the first frame update
    void Start() {
        // Health starts at max health
        health.Value = maxHealth.Value;
        // Find & assign the health slider
        healthSlider = GameObject.Find("Health Slider").GetComponent<Slider>();
        // Update health bar position
        this.UpdateHealthBar();
    }

    // Update is called once per frame
    void Update() {
        // EMPTY
    }

    // Called when the player takes damage
    public void Damage(float damage) {
        this.SetHealth(health.Value - damage);
    }

    // Called when the player gains health
    public void Heal(float heal) {
        this.SetHealth(health.Value + heal);
    }

    // Check if the player is dead (less than or equal to 0 health)
    public bool IsDead() {
        return (health.Value <= 0.0f) ? true : false;
    }

    // Getter function for health
    public float GetHealth() {
        return health.Value;
    }

    // Setter function for health
    public void SetHealth(float newHealth) {
        health.Value = newHealth;
        if (health.Value > maxHealth.Value) {
            health.Value = maxHealth.Value;
        }
        else if (health.Value < 0.0f) {
            health.Value = 0.0f;
        }
        this.UpdateHealthBar();
    }

    // Update health bar position
    private void UpdateHealthBar() {
        healthSlider.value = health.Value / maxHealth.Value;
    }

}
