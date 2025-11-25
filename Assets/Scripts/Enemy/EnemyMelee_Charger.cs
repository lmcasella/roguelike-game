using System.Collections;
using UnityEngine;

public class EnemyCharger : EnemyAI
{
    [Header("Charge Stats")]
    [SerializeField] private float chargeSpeed = 15f;
    [SerializeField] private float chargeDuration = 0.5f;
    [SerializeField] private float impactRadius = 2f;
    [SerializeField] private int explosionDamage = 20;
    [SerializeField] private GameObject impactVFX;
    [SerializeField] private LayerMask playerLayer;

    protected override IEnumerator AttackSequence()
    {
        // 1. PREPARACIÓN
        // Se pone rojo y mira al jugador
        GetComponent<SpriteRenderer>().color = Color.magenta;
        Vector2 lockDir = (target.position - transform.position).normalized;

        yield return new WaitForSeconds(0.5f); // Tiempo de aviso

        // 2. CARGA
        float timer = 0;
        while (timer < chargeDuration)
        {
            // Movemos el Rigidbody manualmente ignorando el Steering normal
            GetComponent<Rigidbody2D>().velocity = lockDir * chargeSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        // 3. IMPACTO (Frenar y explotar)
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        DoAreaDamage();

        // 4. RECUPERACIÓN
        GetComponent<SpriteRenderer>().color = Color.white; // Color original
        yield return new WaitForSeconds(4f); // Cooldown interno extra
    }

    private void DoAreaDamage()
    {
        // Visual
        if (impactVFX != null) Instantiate(impactVFX, transform.position, Quaternion.identity);

        // Lógica de área
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, impactRadius, playerLayer);
        foreach (var hit in hits)
        {
            var health = hit.GetComponent<SystemHealth>();
            if (health != null) health.DealDamage(explosionDamage);
        }

        // Debug visual
        Debug.Log("BOOM! Impacto de carga");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}