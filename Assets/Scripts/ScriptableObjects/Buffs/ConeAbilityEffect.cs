using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeAbilityEffect : MonoBehaviour
{
    [Header("Configuración del Cono")]
    [SerializeField] private float range = 5f;
    [SerializeField] private float angle = 45f; // Ángulo del cono
    [SerializeField] private float fearDuration = 3f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Visual")]
    [SerializeField] private GameObject particleEffect; // Opcional

    // Al instanciarse, calcular el cono inmediatamente
    private void Start()
    {
        CheckConeArea();

        // Destruir el objeto detector después de un frame
        Destroy(gameObject, 0.5f);
    }

    private void CheckConeArea()
    {
        // 1. Obtener todos los enemigos en radio circular primero
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

        foreach (var hit in hits)
        {
            Transform enemyTransform = hit.transform;

            // 2. Calcular dirección hacia el enemigo
            Vector2 directionToEnemy = (enemyTransform.position - transform.position).normalized;

            // 3. Calcular ángulo: 'transform.right' es hacia donde mira el "Grito"
            float angleToEnemy = Vector2.Angle(transform.right, directionToEnemy);

            // 4. Si está dentro de la mitad del ángulo
            if (angleToEnemy < angle / 2f)
            {
                // 5. Aplicar FEAR
                EnemyAI enemyAI = hit.GetComponent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.ApplyFear(fearDuration);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Dibujar el cono en el editor para ajustarlo
        Gizmos.color = Color.cyan;
        Vector3 directionUp = Quaternion.Euler(0, 0, angle / 2) * transform.right;
        Vector3 directionDown = Quaternion.Euler(0, 0, -angle / 2) * transform.right;

        Gizmos.DrawLine(transform.position, transform.position + directionUp * range);
        Gizmos.DrawLine(transform.position, transform.position + directionDown * range);
        Gizmos.DrawWireSphere(transform.position, range); // El alcance máximo
    }
}
