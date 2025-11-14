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

    // --- Máquina de Estados ---
    private enum EnemyState { Idle, Chasing, Attacking, Fleeing }
    private EnemyState currentState;

    // --- Referencias ---
    public Transform target; // El jugador
    private Rigidbody2D rb;
    private Vector2 currentVelocity;
    private Enemy enemyStats; // Para leer el daño
    private float nextAttackTime = 0f; // Para controlar el cooldown

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentState = EnemyState.Idle;
        enemyStats = GetComponent<Enemy>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Buscar al jugador una vez al inicio (Asegúrate de que tu Player tenga el Tag "Player")
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Si no hay jugador, no hacer nada
        if (target == null) return;

        // Distancia al jugador
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // --- Lógica de Transición de Estados ---
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

                // Lógica simple para activar Flee (ejemplo: si activas la casilla en el inspector)
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
                    currentState = EnemyState.Idle; // Ya huyó lo suficiente
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
                // Se queda quieto (o podrías poner patrullaje aquí)
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
                if (Time.time >= nextAttackTime)
                {
                    AttackTarget();
                    // Calcular cuándo es el próximo ataque
                    nextAttackTime = Time.time + (1f / attacksPerSecond);
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

    // FLEE: Lo opuesto exacto a Seek
    private Vector2 Flee(Vector2 targetPos)
    {
        // 1. Vector Deseado: Desde OBJETIVO hasta PLAYER (Al revés)
        Vector2 desiredVelocity = ((Vector2)transform.position - targetPos).normalized * moveSpeed;
        return desiredVelocity;
    }

    private void ApplyForce(Vector2 force)
    {
        // Para un movimiento tipo arcade simple, asignamos la velocidad directamente.
        // Si quisieras física realista ("resbalosa"), usarías rb.AddForce(force).
        rb.velocity = force;
    }

    // --- Debug Visual (Gizmos) ---
    // Esto dibuja círculos en el editor para ver los rangos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
