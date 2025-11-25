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

    public void Regenerate(float percentPerSecond, float duration)
    {
        StartCoroutine(RegenerationRoutine(percentPerSecond, duration));
    }

    private IEnumerator RegenerationRoutine(float percentPerSecond, float duration)
    {
        float timer = 0f;

        // Definir cada cuánto se aplica la cura (1 segundo)
        float tickInterval = 1f;

        while (timer < duration)
        {
            yield return new WaitForSeconds(tickInterval);

            // Calculamos cuánta vida curar en este tick
            // (Porcentaje / 100) * VidaMáxima * (tiempo del tick)
            int healAmount = Mathf.RoundToInt((percentPerSecond / 100f) * maxHealth * tickInterval);

            if (healAmount < 1) healAmount = 1; // Curar al menos 1

            Heal(healAmount);

            timer += tickInterval;
        }
    }

    public void Heal(int healAmount)
    {
        if (healAmount < 0) return;

        if (currentHealth <= 0) return; // No puede curarse si esta muerto

        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Notificar la nueva vida a los suscriptores
        if (gameObject.CompareTag("Player"))
        {
            GameEvents.ReportPlayerHealthChanged(currentHealth, maxHealth);
        }
    }

    public void ModifyMaxHealth(int amount)
    {
        maxHealth += amount;

        if (maxHealth < 1)
        {
            maxHealth = 1; // Asegurarse de que la vida máxima no sea menor a 1
        }

        // Ajustar la vida actual si excede la nueva vida máxima
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Notificar la nueva vida a los suscriptores
        if (gameObject.CompareTag("Player"))
        {
            GameEvents.ReportPlayerHealthChanged(currentHealth, maxHealth);
        }
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public void SetMaxHealth(int value) { maxHealth = value; }
    public void SetHealth(int value) { currentHealth = value; }
}
