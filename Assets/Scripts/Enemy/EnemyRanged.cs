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
            // 1. Calculamos la dirección hacia el jugador
            Vector2 direction = target.position - firePoint.position;

            // 2. Calculamos el ángulo. 
            // IMPORTANTE: El "- 90f" asume que tu sprite de flecha apunta hacia ARRIBA.
            // Si tu dibujo de flecha apunta a la DERECHA, quita el "- 90f".
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            // 3. Convertimos el ángulo en una Rotación
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            // 4. ¡Instanciamos CON esa rotación! (Aquí estaba el error seguramente)
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, rotation);

            // 5. Inicializamos el daño
            Projectile projScript = proj.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.Initialize(projectileDamage);
            }
        }
    }
}
