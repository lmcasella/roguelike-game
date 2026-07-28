using UnityEngine;

public class SplitOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject miniEnemyPrefab;
    [SerializeField] private int amountToSpawn = 2;

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;

        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 0.5f;
            Instantiate(miniEnemyPrefab, transform.position + (Vector3)randomOffset, Quaternion.identity);
        }
    }
}