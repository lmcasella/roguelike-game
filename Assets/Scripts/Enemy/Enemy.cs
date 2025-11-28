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
    [SerializeField] private bool isBoss = false;

    [Header("Loot System")]
    [Tooltip("Probabilidad de soltar un objeto (0 a 100%)")]
    [Range(0, 100)][SerializeField] private float dropChance = 30f;

    [Tooltip("Objeto Loot_Item")]
    [SerializeField] private GameObject lootPrefab;

    [Tooltip("Lista de posibles buffs que puede soltar este enemigo")]
    [SerializeField] private List<BuffEffect> possibleDrops;

    [Header("Audio")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    //[Header("Attack")]
    //[SerializeField] private int attackDamage = 10;
    //public int AttackDamage => attackDamage; // Propiedad pública para leerlo

    private SystemHealth healthComponent;
    private Animator animator;

    // private EnemyAI enemyAI;

    public int CurrentHealth => healthComponent.GetCurrentHealth();

    private void Awake()
    {
        healthComponent = GetComponent<SystemHealth>();
        animator = GetComponentInChildren<Animator>();
        // enemyAI = GetComponent<EnemyAI>();
    }

    // --- Implementacion del IDamageable ---
    public void TakeDamage(int damageAmount)
    {
        // Logica especifica del Enemy
        Debug.Log("Enemy took damage");

        if (hurtSound != null) AudioManager.Instance.PlaySoundEffect(hurtSound);

        if (animator != null)
        {
            animator.SetTrigger("OnHit");
        }
    }

    public void Die()
    {
        // Logica especifica de muerte del Enemy
        Debug.Log("Enemy died");

        TryDropLoot();

        GameEvents.ReportEnemyDied(this, scoreValue);

        if (isBoss)
        {
            GameEvents.ReportBossDied();
        }

        if (deathSound != null) AudioManager.Instance.PlaySoundEffect(deathSound);

        Destroy(gameObject);
    }

    private void TryDropLoot()
    {
        // Validaciones
        if (lootPrefab == null) return;
        if (possibleDrops == null || possibleDrops.Count == 0) return;

        // --- TIRAR DADOS ---
        // Generar un número entre 0 y 100. Si es menor que dropChance, dropea.
        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= dropChance)
        {
            // 1. Elegir un buff al azar de la lista
            BuffEffect selectedBuff = possibleDrops[Random.Range(0, possibleDrops.Count)];

            // 2. Instanciar el objeto en la posición del enemigo
            GameObject lootObj = Instantiate(lootPrefab, transform.position, Quaternion.identity);

            Rigidbody2D lootRb = lootObj.GetComponent<Rigidbody2D>();
            if (lootRb != null)
            {
                Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                lootRb.AddForce(randomDir * 3f, ForceMode2D.Impulse);
            }

            // 3. Inicializarlo
            // Pone el icono y el buffeffect
            LootPickup pickupScript = lootObj.GetComponent<LootPickup>();
            if (pickupScript != null)
            {
                pickupScript.Initialize(selectedBuff);
            }
        }
    }
}
