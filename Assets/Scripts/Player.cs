using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private SystemHealth healthComponent;

    // Getter
    // '=>' reemplaza '{ get { return ...; } }'
    public int CurrentHealth => healthComponent.GetCurrentHealth();

    private void Awake()
    {
        healthComponent = GetComponent<SystemHealth>();
        if (healthComponent == null )
        {
            Debug.LogError("Player is missing a Health component");
        }
    }

    // --- Implementacion del IDamageable ---
    public void TakeDamage(int damageAmount)
    {
        // Logica especifica del Player
        Debug.Log("Player took damage");

        // Notificar la nueva vida a los suscriptores
        GameEvents.ReportPlayerHealthChanged(healthComponent.GetCurrentHealth(), healthComponent.GetMaxHealth());
    }

    public void Die()
    {
        // Logica especifica de muerte del Player
        Debug.Log("Player has died");
        GameEvents.ReportPlayerDied();
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
