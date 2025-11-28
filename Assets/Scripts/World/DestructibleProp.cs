using System.Collections.Generic;
using UnityEngine;

public class DestructibleProp : MonoBehaviour, IDamageable
{
    [Header("Configuración")]
    [SerializeField] private GameObject destroyVFX;
    [SerializeField] private GameObject lootPrefab; // Loot drop
    [SerializeField] private List<BuffEffect> possibleDrops;
    [SerializeField] private float dropChance = 40f;

    private SystemHealth healthComponent;
    private bool isDestroyed = false;
    private void Awake()
    {
        healthComponent = GetComponent<SystemHealth>();
    }

    public int CurrentHealth
    {
        get
        {
            if (healthComponent != null) return healthComponent.GetCurrentHealth();
            return 0;
        }
    }

    // Implementación de IDamageable
    public void TakeDamage(int damageAmount)
    {
        if (isDestroyed) return;

        // Se rompe de un solo golpe
        Die();
    }

    public void Die()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        // 1. Efecto visual
        if (destroyVFX != null)
            Instantiate(destroyVFX, transform.position, Quaternion.identity);

        // 2. Chance de soltar Loot
        TryDropLoot();


        // 3. Sonido
        // TODO: AudioManager.Instance.PlaySoundEffect(...);

        Destroy(gameObject);
    }

    private void TryDropLoot()
    {
        // Validaciones
        if (lootPrefab == null) return;
        if (possibleDrops == null || possibleDrops.Count == 0) return;

        // --- TIRAR DADOS ---
        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= dropChance)
        {
            // 1. Elegir un buff al azar
            BuffEffect selectedBuff = possibleDrops[Random.Range(0, possibleDrops.Count)];

            // 2. Instanciar
            GameObject lootObj = Instantiate(lootPrefab, transform.position, Quaternion.identity);

            // Física del loot (salto)
            Rigidbody2D lootRb = lootObj.GetComponent<Rigidbody2D>();
            if (lootRb != null)
            {
                Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                lootRb.AddForce(randomDir * 3f, ForceMode2D.Impulse);
            }

            // 3. Inicializar
            LootPickup pickupScript = lootObj.GetComponent<LootPickup>();
            if (pickupScript != null)
            {
                pickupScript.Initialize(selectedBuff);
            }
        }
    }
}