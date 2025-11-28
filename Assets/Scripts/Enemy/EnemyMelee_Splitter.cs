using UnityEngine;

public class EnemyMelee_Splitter : EnemyMelee
{
    [Header("Split Config")]
    [SerializeField] private GameObject miniEnemyPrefab;
    [SerializeField] private int amountToSpawn = 2;

    // Nos suscribimos al evento de muerte de ESTE enemigo
    private void OnDestroy()
    {
        // Solo spawnear si el juego sigue corriendo
        if (!this.gameObject.scene.isLoaded) return;

        SpawnMinis();
    }

    private void SpawnMinis()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            // Posición aleatoria cercana
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Instantiate(miniEnemyPrefab, transform.position + (Vector3)randomOffset, Quaternion.identity);
        }
    }
}