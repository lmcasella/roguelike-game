using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private IDamageable target;
    [SerializeField] private int currentHealth;

    private void Awake()
    {
        // Encontramos el componente en este GameObject que implemente un IDamageable
        target = GetComponent<IDamageable>();
        if (target == null)
        {
            Debug.LogError("Health component needs an IDamageable script on the same GameObject");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        // Notificar la vida inicial a los suscriptores
        if (gameObject.CompareTag("Player"))
        {
            GameEvents.ReportPlayerHealthChanged(currentHealth, maxHealth);
        }
    }

    // --- Metodos para que sean llamados por poderes, trampas, etc. ---
    public void DealDamage(int damageAmount)
    {
        if (damageAmount < 0) return;
        if (currentHealth <= 0) return;

        currentHealth -= damageAmount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Llamar a la funcion de daño del strategy. No hace falta que sepa nada mas
        target.TakeDamage(damageAmount);

        // Checkear si murio
        if (currentHealth == 0)
        {
            target.Die();
        }
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
