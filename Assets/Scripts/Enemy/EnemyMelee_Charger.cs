using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee_Charger : EnemyAI
{
    [Header("Charge Stats")]
    [SerializeField] private float chargeSpeed = 15f;
    [SerializeField] private float chargeDuration = 0.5f;
    [SerializeField] private float impactRadius = 2f;
    [SerializeField] private int explosionDamage = 20;
    [SerializeField] private GameObject impactVFX;
    [SerializeField] private LayerMask playerLayer;

    protected override void FixedUpdate()
    {
        // Si estamos en medio de la carga, NO ejecutamos la lógica del padre para que no nos frene la velocidad a 0.
        if (isPreparingAttack)
        {
            return;
        }

        // Si no estamos cargando, que se comporte como un enemigo normal
        base.FixedUpdate();
    }

    protected override IEnumerator AttackSequence()
    {
        // 1. PREPARACIÓN
        // Se pone rojo y mira al jugador
        GetComponent<SpriteRenderer>().color = Color.magenta;
        Vector2 lockDir = (target.position - transform.position).normalized;

        isOverrideMovement = true;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

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

        float recoveryTimer = 0f;
        float recoveryDuration = 1f;

        while (recoveryTimer < recoveryDuration)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Anular empuje al estar parado el enemigo
            recoveryTimer += Time.deltaTime;
            yield return null;
        }

        isOverrideMovement = false;
    }

    private void DoAreaDamage()
    {
        // Visual
        if (impactVFX != null) Instantiate(impactVFX, transform.position, Quaternion.identity);

        // Lógica de área
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, impactRadius, playerLayer);

        // Para no hacer daño al jugador varias veces
        List<SystemHealth> damagedVictims = new List<SystemHealth>();

        foreach (var hit in hits)
        {
            // Saltar si ya daño este objeto
            //if (damagedObjects.Contains(hit.gameObject)) continue;

            var health = hit.GetComponentInParent<SystemHealth>();
            if (health != null)
            {
                if (damagedVictims.Contains(health)) continue;

                health.DealDamage(explosionDamage);

                // Marcar como dañado
                damagedVictims.Add(health);
            }
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