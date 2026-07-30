using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("Referencias Visuales")]
    [SerializeField] private Image fillImage;
    [SerializeField] private Canvas myCanvas;

    [Header("Configuración")]
    [SerializeField] private SystemHealth targetHealth;

    [Tooltip("Altura de la barra respecto al centro del enemigo")]
    [SerializeField] private float yOffset = 0.7f;

    private void Awake()
    {
        // Si no asignaste el SystemHealth en el inspector, lo busca en el objeto padre (el enemigo)
        if (targetHealth == null)
        {
            // 1. Busca en el objeto actual y hacia arriba (padres)
            targetHealth = GetComponentInParent<SystemHealth>();

            // 2. Si no lo encuentra, sube al padre principal (Enemy_Base) y busca hacia abajo en todos los hijos/nietos
            if (targetHealth == null && transform.parent != null)
            {
                targetHealth = transform.parent.GetComponentInChildren<SystemHealth>();
            }
        }

        if (myCanvas == null)
        {
            myCanvas = GetComponent<Canvas>();
        }
    }

    private void OnEnable()
    {
        if (targetHealth != null)
        {
            targetHealth.OnHealthChanged += UpdateHealthBar;
        }
    }

    private void OnDisable()
    {
        if (targetHealth != null)
        {
            targetHealth.OnHealthChanged -= UpdateHealthBar;
        }
    }

    private void Start()
    {
        transform.localPosition = new Vector3(0, yOffset, 0);

        // Apagamos solo el componente Canvas. El GameObject (y este script) siguen encendidos.
        myCanvas.enabled = false;
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        // 1. Calcular el porcentaje (es importante castear a float para que no redondee a 0)
        float healthPercentage = (float)currentHealth / maxHealth;

        // 2. Actualizar la imagen
        fillImage.fillAmount = healthPercentage;

        // 3. Lógica de visibilidad: Mostrar solo si recibió daño y sigue vivo
        bool shouldShow = currentHealth < maxHealth && currentHealth > 0;
        myCanvas.enabled = shouldShow;
    }
}