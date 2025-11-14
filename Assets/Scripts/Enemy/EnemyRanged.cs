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
            // 1. Calcular dirección hacia el jugador (Target - Origen)
            Vector2 direction = target.position - firePoint.position;

            // 2. Calcular el ángulo (matemáticas de arco tangente)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            // 3. Crear la rotación
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            // Instanciar proyectil
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            
            // Configurar el daño del proyectil si tiene un componente
            Projectile projComponent = projectile.GetComponent<Projectile>();
            if (projComponent != null)
            {
                projComponent.Initialize(projectileDamage);
            }
            Debug.Log($"EnemyRanged fired a projectile at {target.name} for {projectileDamage} damage.");
        }
    }
}
