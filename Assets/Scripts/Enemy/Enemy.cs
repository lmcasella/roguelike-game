using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SystemHealth))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private int scoreValue = 10;

    private SystemHealth healthComponent;

    // private EnemyAI enemyAI;

    public int CurrentHealth => healthComponent.GetCurrentHealth();

    private void Awake()
    {
        healthComponent = GetComponent<SystemHealth>();
        // enemyAI = GetComponent<EnemyAI>();
    }

    // --- Implementacion del IDamageable ---
    public void TakeDamage(int damageAmount)
    {
        // Logica especifica del Enemy
        Debug.Log("Enemy took damage");
    }

    public void Die()
    {
        // Logica especifica de muerte del Enemy
        Debug.Log("Enemy died");
        GameEvents.ReportEnemyDied(this, scoreValue);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
