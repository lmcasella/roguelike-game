using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyRanged : EnemyAI
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int projectileDamage = 10;

    protected override void AttackTarget()
    {
        if (projectilePrefab != null && firePoint != null && target != null)
        {
            // 1. Calcular la dirección hacia el jugador
            Vector2 direction = target.position - firePoint.position;

            // 2. Calcular el ángulo
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            // 3. Convertir el ángulo en una Rotación
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            // 4. Instanciar el proyectil
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, rotation);

            // 5. Inicializar el daño
            Projectile projScript = proj.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.Initialize(projectileDamage);
            }
        }
    }
}
