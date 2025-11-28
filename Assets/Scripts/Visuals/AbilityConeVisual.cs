using UnityEngine;

// Este script ajusta la escala de un sprite para representar visualmente el área de efecto cónica de una habilidad
public class AbilityConeVisual : MonoBehaviour
{
    [Header("Referencia")]
    [Tooltip("Arrastra aquí el ScriptableObject de la habilidad (ej. Dragon Roar)")]
    [SerializeField] private Ability abilityData;

    private void Start()
    {
        UpdateVisuals();
    }

    // Puedes llamar a esto manualmente o desde el botón del inspector (Click derecho en el componente)
    [ContextMenu("Update Visuals")]
    public void UpdateVisuals()
    {
        // Validaciones de seguridad
        if (abilityData == null || abilityData.projectilePrefab == null) return;

        // 1. Buscamos el script en el prefab de la habilidad
        ConeAbilityEffect effect = abilityData.projectilePrefab.GetComponent<ConeAbilityEffect>();

        if (effect != null)
        {
            float range = effect.GetRange();
            float angle = effect.GetAngle();

            // 2. Calcular Escala Matemática

            // LARGO (Eje X): Es directo el rango.
            float scaleX = range;

            // ANCHO (Eje Y): Trigonometría
            // Tangente(ángulo/2) = Opuesto (mitad del ancho) / Adyacente (rango)
            // Ancho Total = 2 * Rango * Tan(ángulo/2)
            float halfAngleRad = (angle / 2f) * Mathf.Deg2Rad;
            float scaleY = 2f * range * Mathf.Tan(halfAngleRad);

            // 3. Aplicar al Transform
            // Asume que tu sprite es un cuadrado de 1x1 con pivote en la izquierda (X=0) o centro.
            // Si el sprite por defecto de Unity (pivote centro), necesitarás ajustar la posición local también.
            transform.localScale = new Vector3(scaleX, scaleY, 1f);
            // Ajuste de posición si usas el sprite cuadrado por defecto (Pivote Centro)
            // Si usas un sprite con pivote a la izquierda, comenta la siguiente línea:
            transform.localPosition = new Vector3(scaleX / 2f, 0, 0);
        }
    }
}