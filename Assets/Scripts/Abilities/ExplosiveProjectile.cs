using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    [Header("Configuración Explosiva")]
    [SerializeField] private float explosionRadius = 2.5f;
    [SerializeField] private GameObject explosionVFX; // Sistema de particulas
    [SerializeField] private LayerMask damageLayer; // Capas que recibiran daño

    // Variable para evitar explotar dos veces en el mismo frame
    private bool hasExploded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded) return;

        // Lógica de impacto: Chocar con enemigo o con obstáculo (paredes)
        // Ignorar al Player y Triggers
        if (collision.CompareTag("Player") || collision.isTrigger) return;

        Explode();
    }

    private void Explode()
    {
        hasExploded = true;

        // 1. Efecto Visual
        if (explosionVFX != null)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }

        // 2. Detectar todo en el área circular
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayer);

        foreach (var hit in hits)
        {
            // Buscamos enemigos (o cualquier cosa rompible)
            IDamageable target = hit.GetComponent<IDamageable>();

            // Evitamos dañarnos a nosotros mismos si el player está cerca
            if (target != null && !hit.CompareTag("Player"))
            {
                target.TakeDamage(damage); // 'damage' viene heredado de Projectile
            }
        }

        // 3. Destruir la bala
        Destroy(gameObject);
    }

    // Dibujar el radio en el editor para verlo
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
