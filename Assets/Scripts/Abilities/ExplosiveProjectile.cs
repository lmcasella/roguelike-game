using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : Projectile
{
    [Header("Explosion Settings")]
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private GameObject explosionVFX;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. (Opcional) Efecto visual
        if (explosionVFX != null)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }

        // 2. Detectar todo en el área
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                SystemHealth health = hitCollider.GetComponent<SystemHealth>();
                if (health != null)
                {
                    health.DealDamage(damage);
                }
            }
        }

        // 3. Destruir la bala
        Destroy(gameObject);
    }

    // FIXME: No se visualiza
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
