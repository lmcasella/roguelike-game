using UnityEngine;

public class EnemyRanged_Triple : EnemyAI
{
    [Header("Triple Shot Config")]
    [SerializeField] private int projectileDamage = 10;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float angleSpread = 15f; // Grados de separación

    protected override void AttackTarget()
    {
        if (target == null) return;

        // Dirección hacia el jugador
        Vector2 direction = (target.position - transform.position).normalized;

        // Disparar 3 proyectiles
        SpawnProjectile(direction, 0);              // Centro
        SpawnProjectile(direction, angleSpread);    // Derecha
        SpawnProjectile(direction, -angleSpread);   // Izquierda
    }

    private void SpawnProjectile(Vector2 baseDir, float angleOffset)
    {
        // Rotar el vector de dirección
        Vector2 finalDir = Quaternion.Euler(0, 0, angleOffset) * baseDir;

        // Calcular rotación para el sprite de la flecha/bala
        float rotZ = Mathf.Atan2(finalDir.y, finalDir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, rotZ);

        GameObject proj = Instantiate(projectilePrefab, transform.position, rotation);

        // Inicializar bala
        var projScript = proj.GetComponent<Projectile>();
        projScript.Initialize(projectileDamage); 
    }
}