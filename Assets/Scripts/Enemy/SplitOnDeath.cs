using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class SplitOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject miniEnemyPrefab;
    [SerializeField] private int amountToSpawn = 2;

    private Enemy enemyComponent;

    private void Awake()
    {
        enemyComponent = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        if (enemyComponent != null)
        {
            // Nos suscribimos al evento
            enemyComponent.OnBeforeDeath += SpawnMinis;
        }
    }

    private void OnDisable()
    {
        if (enemyComponent != null)
        {
            // Nos desuscribimos por seguridad
            enemyComponent.OnBeforeDeath -= SpawnMinis;
        }
    }

    private void SpawnMinis()
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Instantiate(miniEnemyPrefab, transform.position + (Vector3)randomOffset, Quaternion.identity);
        }
    }
}