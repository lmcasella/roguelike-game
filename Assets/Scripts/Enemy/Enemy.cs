using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int scoreValue = 10;

    private SystemHealth healthComponent;
    public int CurrentHealth => healthComponent.GetCurrentHealth();

    private void Awake()
    {
        healthComponent = GetComponent<SystemHealth>();
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
        GameEvents.ReportEnemyKilled(scoreValue);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
