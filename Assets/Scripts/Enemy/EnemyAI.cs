using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    // Configuracion
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float detectionRange = 10f; // Cuándo empieza a perseguir
    [SerializeField] private float attackRange = 1f;    // Cuándo ataca

    [Header("Combat")]
    [SerializeField] private float attacksPerSecond = 1f;

    [Header("Steering")]
    [SerializeField] private bool useFleeBehavior = false; // Para probar Flee más tarde

    [Header("Combat Feedback")]
    [SerializeField] private float attackChargeTime = 0.5f;
    [SerializeField] private Color chargeColor = Color.yellow;

    // --- Máquina de Estados ---
    private enum EnemyState { Idle, Chasing, Attacking, Fleeing }
    private EnemyState currentState;

    // --- Referencias ---
    public Transform target; // El jugador
    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private Enemy enemyStats; // Para leer el daño
    private float nextAttackTime = 0f; // Para controlar el cooldown
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isPreparingAttack = false;
    private float fearTimer = 0f;
    private bool isFeared = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = EnemyState.Idle;
        enemyStats = GetComponent<Enemy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Buscar al jugador una vez al inicio (Player tiene que tener el tag Player)
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }

        originalColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Si no hay jugador, no hacer nada
        if (target == null) return;

        // Distancia al jugador
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // --- Lógica de Transición de Estados ---
        if (isFeared)
        {
            currentState = EnemyState.Fleeing;
            fearTimer -= Time.deltaTime;

            if (fearTimer <= 0)
            {
                isFeared = false;
                currentState = EnemyState.Chasing; // Vuelve a atacar al terminar
            }
            return;
        }

        switch (currentState)
        {
            case EnemyState.Idle:
                if (distanceToTarget < detectionRange)
                {
                    currentState = EnemyState.Chasing;
                }
                break;

            case EnemyState.Chasing:
                if (distanceToTarget > detectionRange * 1.5f) // *1.5 para evitar que entre y salga rápido
                {
                    currentState = EnemyState.Idle;
                }
                else if (distanceToTarget <= attackRange)
                {
                    currentState = EnemyState.Attacking;
                }

                // Lógica para activar Flee
                if (useFleeBehavior)
                {
                    currentState = EnemyState.Fleeing;
                }
                break;

            case EnemyState.Attacking:
                if (distanceToTarget > attackRange)
                {
                    currentState = EnemyState.Chasing;
                }
                break;

            case EnemyState.Fleeing:
                if (distanceToTarget > detectionRange * 2)
                {
                    currentState = EnemyState.Idle;
                }
                break;
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // --- Ejecutar Comportamiento según Estado ---
        switch (currentState)
        {
            case EnemyState.Idle:
                // IDLE
                // NOTE: Se podria implementar una patrulla
                rb.velocity = Vector2.zero;
                break;

            case EnemyState.Chasing:
                // Steering 1: SEEK (Buscar)
                Vector2 seekForce = Seek(target.position);
                ApplyForce(seekForce);
                break;

            case EnemyState.Attacking:
                // 1. Frenar al enemigo para atacar
                rb.velocity = Vector2.zero;

                // 2. Intentar atacar (con cooldown)
                if (Time.time >= nextAttackTime && !isPreparingAttack)
                {
                    StartCoroutine(AttackSequence());
                }
                break;

            case EnemyState.Fleeing:
                // Steering 2: FLEE (Huir)
                Vector2 fleeForce = Flee(target.position);
                ApplyForce(fleeForce);
                break;
        }
    }

    // Ataque
    protected virtual IEnumerator AttackSequence()
    {
        isPreparingAttack = true;

        // 1. INICIO DE CARGA (Feedback Visual)
        spriteRenderer.color = chargeColor; // Se pone de color (aviso)

        // 2. ESPERA (El tiempo que tiene el jugador para reaccionar)
        yield return new WaitForSeconds(attackChargeTime);

        // 3. EJECUTAR EL ATAQUE REAL
        AttackTarget(); // Llama a la función del hijo (Melee o Ranged)

        // 4. RESTAURAR VISUALES
        spriteRenderer.color = originalColor;

        // 5. CALCULAR COOLDOWN
        nextAttackTime = Time.time + (1f / attacksPerSecond);

        isPreparingAttack = false;
    }

    // virtual -> permite que los hijos cambien el metodo
    protected virtual void AttackTarget() { 
        Debug.Log("EnemyAI attacked the target.");
    }

    // --- Steering Behaviors ---

    // SEEK: Ir directo al objetivo
    private Vector2 Seek(Vector2 targetPos)
    {
        // 1. Vector Deseado: Desde PLAYER hasta OBJETIVO
        Vector2 desiredVelocity = (targetPos - (Vector2)transform.position).normalized * moveSpeed;

        // 2. Steering: Deseado - Actual (para movimientos más suaves)
        return desiredVelocity;
    }

    // FLEE: Lo opuesto a Seek
    private Vector2 Flee(Vector2 targetPos)
    {
        // 1. Vector Deseado: Desde OBJETIVO hasta PLAYER (Al revés)
        Vector2 desiredVelocity = ((Vector2)transform.position - targetPos).normalized * moveSpeed;
        return desiredVelocity;
    }

    private void ApplyForce(Vector2 force)
    {
        rb.velocity = force;
    }

    public void ApplyFear(float duration)
    {
        if (target == null) return;

        isFeared = true;
        fearTimer = duration;
        currentState = EnemyState.Fleeing;

        // Feedback visual
        StartCoroutine(FearFeedbackRoutine(duration));

        Debug.Log($"{gameObject.name} tiene miedo por {duration} segundos!");
    }

    private IEnumerator FearFeedbackRoutine(float duration)
    {
        spriteRenderer.color = Color.blue; // TODO: Un icono de calavera encima
        yield return new WaitForSeconds(duration);
        spriteRenderer.color = originalColor;
    }

    // --- Debug Visual (Gizmos) ---
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
