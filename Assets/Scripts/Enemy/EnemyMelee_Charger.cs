using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee_Charger : EnemyAI
{
    [Header("Charge Stats")]
    [SerializeField] private float chargeSpeed = 15f;
    [SerializeField] private float chargeDuration = 0.5f;
    [SerializeField] private float impactRadius = 2f;
    //[SerializeField] private int explosionDamage = 20;
    [SerializeField] private GameObject impactVFX;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float preparationTime = 0.5f;
    [SerializeField] private float recoveryDuration = 1f;

    [Header("Telegraph")]
    [Tooltip("Asignar un GameObject con un SpriteRenderer (circulo)")]
    [SerializeField] private GameObject telegraphCircle;

    private Rigidbody2D chargerRb;

    protected override void Start()
    {
        base.Start();
        chargerRb = GetComponent<Rigidbody2D>();

        if (telegraphCircle != null)
        {
            // Apagamos el círculo al iniciar
            telegraphCircle.SetActive(false);

            // 1. Obtenemos el SpriteRenderer del círculo
            SpriteRenderer sr = telegraphCircle.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                float spriteSize = sr.sprite.bounds.size.x;
                float desiredDiameter = impactRadius * 2f;

                // 2. Calculamos la escala y la dividimos por la escala del padre
                float baseScale = desiredDiameter / spriteSize;
                float finalScale = baseScale / transform.lossyScale.x;

                telegraphCircle.transform.localScale = new Vector3(finalScale, finalScale, 1f);
            }
        }
    }

    protected override IEnumerator AttackSequence()
    {
        isPreparingAttack = true;

        // 1. PREPARACIÓN (Anticipación para el jugador)
        if (animator != null) animator.enabled = false;

        SpriteRenderer chargerSprite = GetComponent<SpriteRenderer>();
        if (chargerSprite != null) chargerSprite.color = Color.magenta;

        // Bloquear movimiento normal durante la preparación
        isOverrideMovement = true;
        chargerRb.velocity = Vector2.zero;

        // Calcular la dirección fija y punto de impacto
        Vector2 chargeDirection = (target.position - transform.position).normalized;
        Vector2 expectedLandingPosition = (Vector2)transform.position + (chargeDirection * chargeSpeed * chargeDuration);

        // Activar Telegraphing Visual
        if (telegraphCircle != null)
        {
            // Movemos el círculo a la posición de impacto y lo encendemos
            telegraphCircle.transform.position = expectedLandingPosition;
            telegraphCircle.SetActive(true);
        }

        //yield return new WaitForSeconds(preparationTime);
        float prepTimer = 0f;
        while (prepTimer < preparationTime)
        {
            chargerRb.velocity = Vector2.zero;
            prepTimer += Time.deltaTime;
            yield return null; // Espera al siguiente frame
        }

        // Apagar linea
        if (telegraphCircle != null) telegraphCircle.SetActive(false);

        // 2. EJECUCIÓN DE LA CARGA
        if (animator != null) animator.enabled = true;
        //if (chargerSprite != null) chargerSprite.color = Color.white;

        // Aplicar la velocidad de impacto directo
        chargerRb.velocity = chargeDirection * chargeSpeed;

        // Esperar el tiempo que dura la embestida en movimiento
        //yield return new WaitForSeconds(chargeDuration);
        float chargeTimer = 0f;
        while (chargeTimer < chargeDuration)
        {
            chargerRb.velocity = chargeDirection * chargeSpeed;
            chargeTimer += Time.deltaTime;
            yield return null;
        }

        // 3. IMPACTO Y DAÑO EN ÁREA
        chargerRb.velocity = Vector2.zero;
        DoAreaDamage();

        if (chargerSprite != null) chargerSprite.color = Color.white;

        // 4. RECUPERACIÓN (Aturdimiento/Stun del enemigo post-carga)
        float recoveryTimer = 0f;
        while (recoveryTimer < recoveryDuration)
        {
            chargerRb.velocity = Vector2.zero;
            recoveryTimer += Time.deltaTime;
            yield return null;
        }

        // Devolver el control al comportamiento de persecución base (Seek)
        isOverrideMovement = false;
        isPreparingAttack = false;
    }

    private void DoAreaDamage()
    {
        if (impactVFX != null) Instantiate(impactVFX, transform.position, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, impactRadius, playerLayer);
        List<SystemHealth> damagedVictims = new List<SystemHealth>();

        foreach (var hit in hits)
        {
            var health = hit.GetComponentInParent<SystemHealth>();
            if (health != null && !damagedVictims.Contains(health))
            {
                int damageToApply = GetRolledDamage();

                health.DealDamage(damageToApply);
                damagedVictims.Add(health);
            }
        }
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyMelee_Charger : EnemyAI
//{
//    [Header("Charge Stats")]
//    [SerializeField] private float chargeSpeed = 15f;
//    [SerializeField] private float chargeDuration = 0.5f;
//    [SerializeField] private float impactRadius = 2f;
//    [SerializeField] private int explosionDamage = 20;
//    [SerializeField] private GameObject impactVFX;
//    [SerializeField] private LayerMask playerLayer;

//    //protected override void update()
//    //{

//    //}

//    protected override void FixedUpdate()
//    {
//        // Si estamos en medio de la carga, NO ejecutamos la lógica del padre para que no nos frene la velocidad a 0
//        if (isPreparingAttack)
//        {
//            return;
//        }

//        // Si no estamos cargando, que se comporte como un enemigo normal
//        base.FixedUpdate();
//    }

//    protected override IEnumerator AttackSequence()
//    {
//        // 1. PREPARACIÓN
//        if (animator != null) animator.enabled = false;

//        // Se pone rojo y mira al jugador
//        GetComponent<SpriteRenderer>().color = Color.magenta;
//        Vector2 lockDir = (target.position - transform.position).normalized;

//        isOverrideMovement = true;
//        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

//        yield return new WaitForSeconds(0.5f); // Tiempo de aviso

//        // 2. CARGA
//        float timer = 0;
//        while (timer < chargeDuration)
//        {
//            // Movemos el Rigidbody manualmente ignorando el Steering normal
//            GetComponent<Rigidbody2D>().velocity = lockDir * chargeSpeed;
//            timer += Time.deltaTime;
//            yield return null;
//        }

//        // 3. IMPACTO (Frenar y explotar)
//        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
//        DoAreaDamage();

//        // 4. RECUPERACIÓN
//        GetComponent<SpriteRenderer>().color = Color.white; // Color original

//        if (animator != null) animator.enabled = true;

//        float recoveryTimer = 0f;
//        float recoveryDuration = 1f;

//        while (recoveryTimer < recoveryDuration)
//        {
//            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Anular empuje al estar parado el enemigo
//            recoveryTimer += Time.deltaTime;
//            yield return null;
//        }

//        isOverrideMovement = false;
//    }

//    private void DoAreaDamage()
//    {
//        // Visual
//        if (impactVFX != null) Instantiate(impactVFX, transform.position, Quaternion.identity);

//        // Lógica de área
//        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, impactRadius, playerLayer);

//        // Para no hacer daño al jugador varias veces
//        List<SystemHealth> damagedVictims = new List<SystemHealth>();

//        foreach (var hit in hits)
//        {
//            // Saltar si ya dañó este objeto
//            //if (damagedObjects.Contains(hit.gameObject)) continue;

//            var health = hit.GetComponentInParent<SystemHealth>();
//            if (health != null)
//            {
//                if (damagedVictims.Contains(health)) continue;

//                health.DealDamage(explosionDamage);

//                // Marcar como dañado
//                damagedVictims.Add(health);
//            }
//        }

//        // Debug visual
//        Debug.Log("Impacto de carga");
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, impactRadius);
//    }
//}