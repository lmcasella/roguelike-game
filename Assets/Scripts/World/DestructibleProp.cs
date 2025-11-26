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
    public int CurrentHealth => healthComponent.GetCurrentHealth();

    // Implementación de IDamageable
    public void TakeDamage(int damageAmount)
    {
        // Se rompe de un solo golpe
        Die();
    }

    public void Die()
    {
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
        // Generamos un número entre 0 y 100. Si es menor que dropChance, dropea.
        float randomValue = Random.Range(0f, 100f);

        if (randomValue <= dropChance)
        {
            // 1. Elegir un buff al azar de la lista
            BuffEffect selectedBuff = possibleDrops[Random.Range(0, possibleDrops.Count)];

            // 2. Instanciar el objeto en la posición del enemigo
            GameObject lootObj = Instantiate(lootPrefab, transform.position, Quaternion.identity);

            Rigidbody2D lootRb = lootObj.GetComponent<Rigidbody2D>();
            if (lootRb != null) // Asegúrate de que tu Loot_Item tenga un RB (Kinematic o Dynamic con drag alto)
            {
                Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;
                lootRb.AddForce(randomDir * 3f, ForceMode2D.Impulse);
            }

            // 3. Inicializarlo
            // FIXME: Esto cambia el SPRITE al icono del buff. Deberia poner un icono del item
            LootPickup pickupScript = lootObj.GetComponent<LootPickup>();
            if (pickupScript != null)
            {
                pickupScript.Initialize(selectedBuff);
            }
        }
    }
}