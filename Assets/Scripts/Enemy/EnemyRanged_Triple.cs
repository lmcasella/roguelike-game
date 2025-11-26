using UnityEngine;

public class EnemyRanged_Triple : EnemyAI
{
    [Header("Triple Shot Config")]
    [SerializeField] private int projectileDamage = 15;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float angleSpread = 15f; // Grados de separación
    [SerializeField] private Transform firePoint;

    protected override void AttackTarget()
    {
        if (target == null) return;

        Vector3 spawnPosition = (firePoint != null) ? firePoint.position : transform.position;

        // Dirección hacia el jugador desde el FirePoint
        Vector2 direction = (target.position - spawnPosition).normalized;

        // Disparar 3 proyectiles
        SpawnProjectile(spawnPosition, direction, 0);              // Centro
        SpawnProjectile(spawnPosition, direction, angleSpread);    // Derecha
        SpawnProjectile(spawnPosition, direction, -angleSpread);   // Izquierda
    }

    private void SpawnProjectile(Vector3 spawnPos, Vector2 baseDir, float angleOffset)
    {
        // Rotar el vector de dirección
        Vector2 finalDir = Quaternion.Euler(0, 0, angleOffset) * baseDir;

        // Calcular rotación para el sprite de la flecha/bala
        float rotZ = Mathf.Atan2(finalDir.y, finalDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, rotZ);

        GameObject proj = Instantiate(projectilePrefab, spawnPos, rotation);

        // Inicializar bala
        var projScript = proj.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.Initialize(projectileDamage, this.gameObject);
        }
    }
}