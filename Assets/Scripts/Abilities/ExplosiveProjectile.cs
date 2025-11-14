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
                    // Usamos la variable 'damage' que heredamos del padre Projectile
                    // (Nota: 'damage' en Projectile.cs debe ser 'protected', no 'private' para verlo aquí.
                    // Si es private, tendrás que usar una función pública o cambiarlo).

                    // Si 'damage' es privado en el padre, usa Initialize() al crearlo y asume que ya tiene el valor,
                    // pero para aplicarlo necesitarías acceso.
                    // SOLUCIÓN RÁPIDA: Cambia 'private float damage' a 'protected float damage' en Projectile.cs

                    health.DealDamage(damage); // O el daño que quieras
                }
            }
        }

        // 3. Destruir la bala
        Destroy(gameObject);
    }

    // Para ver el rango en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
