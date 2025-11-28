using UnityEngine;

public class ConeAbilityEffect : MonoBehaviour
{
    [Header("Configuración del Cono")]
    [SerializeField] private float range = 5f;
    [SerializeField] private float angle = 45f; // Ángulo total de apertura
    [SerializeField] private float fearDuration = 3f;
    [SerializeField] private LayerMask enemyLayer;

    // Getters para que el visualizador sepa cuánto medir
    public float GetRange() => range;
    public float GetAngle() => angle;

    private void Start()
    {
        CheckConeArea();

        // El efecto es instantáneo, así que destruimos el objeto casi de inmediato
        Destroy(gameObject, 0.1f);
    }

    private void CheckConeArea()
    {
        // 1. Detección circular inicial
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

        Debug.Log($"[Fear Logic] Posición: {transform.position} | Enemigos detectados en radio: {hits.Length}");

        foreach (var hit in hits)
        {
            Transform enemyTransform = hit.transform;

            // 2. Dirección hacia el enemigo
            Vector2 directionToEnemy = (enemyTransform.position - transform.position).normalized;

            // 3. Calcular ángulo respecto a donde miramos (transform.right)
            float angleToEnemy = Vector2.Angle(transform.right, directionToEnemy);

            // 4. Si está dentro del cono (Mitad del ángulo hacia cada lado)
            if (angleToEnemy < angle / 2f)
            {
                // 5. Aplicar FEAR
                EnemyAI enemyAI = hit.GetComponentInParent<EnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.ApplyFear(fearDuration);
                }
            }
        }
    }

    // Dibujo para debug en el editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        // Dibujamos las líneas del cono
        Vector3 directionUp = Quaternion.Euler(0, 0, angle / 2) * transform.right;
        Vector3 directionDown = Quaternion.Euler(0, 0, -angle / 2) * transform.right;

        Gizmos.DrawLine(transform.position, transform.position + directionUp * range);
        Gizmos.DrawLine(transform.position, transform.position + directionDown * range);
    }
}