using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TextMeshPro textMesh; // Asignar en el inspector

    [Header("Configuración de Animación")]
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float fadeDuration = 1f;

    private float fadeTimer;
    private Color textColor;

    // Se llama cada vez que el Pool "despierta" a este número
    public void Setup(int damageAmount, Color newColor)
    {
        textMesh.text = damageAmount.ToString();

        // Reiniciamos el color y la opacidad al 100%
        textColor = newColor;
        textColor.a = 1f;
        textMesh.color = textColor;

        // Reiniciamos el temporizador
        fadeTimer = fadeDuration;
    }

    private void Update()
    {
        // 1. Movimiento constante hacia arriba
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // 2. Lógica de desvanecimiento
        fadeTimer -= Time.deltaTime;

        if (fadeTimer <= 0)
        {
            // En lugar de Destroy(), lo devolvemos al Pool
            DamageNumberManager.Instance.ReturnToPool(this);
        }
        else
        {
            // Interpolar la opacidad (alpha) basada en el tiempo restante
            textColor.a = fadeTimer / fadeDuration;
            textMesh.color = textColor;
        }
    }
}