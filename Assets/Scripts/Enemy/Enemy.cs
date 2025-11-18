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

    [Header("Audio")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    //[Header("Attack")]
    //[SerializeField] private int attackDamage = 10;
    //public int AttackDamage => attackDamage; // Propiedad pública para leerlo

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

        if (hurtSound != null) AudioManager.Instance.PlaySoundEffect(hurtSound);
    }

    public void Die()
    {
        // Logica especifica de muerte del Enemy
        Debug.Log("Enemy died");
        GameEvents.ReportEnemyDied(this, scoreValue);

        if (deathSound != null) AudioManager.Instance.PlaySoundEffect(deathSound);

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
