using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SystemHealth))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IDamageable
{
    [Header("Health")]
    private SystemHealth healthComponent;

    // Getter
    // '=>' reemplaza '{ get { return ...; } }'
    public int CurrentHealth => healthComponent.GetCurrentHealth();

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        healthComponent = GetComponent<SystemHealth>();
        rb = GetComponent<Rigidbody2D>();

        if (healthComponent == null )
        {
            Debug.LogError("Player is missing a Health component");
        }
    }

    // Update is called once per frame
    // Implementar aca inputs
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;

        if (Input.GetKeyDown(KeyCode.H))
        {
            healthComponent.DealDamage(10);
        }
    }

    // Implementar aca fisicas
    private void FixedUpdate()
    {
        Vector2 newVelocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
        rb.velocity = newVelocity;
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
}
