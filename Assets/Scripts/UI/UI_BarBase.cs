using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UI_BarBase : MonoBehaviour
{
    [Header("Configuración Visual")]
    [Tooltip("Velocidad a la que la barra baja visualmente (opcional)")]
    [SerializeField] protected bool smoothTransition = true;
    [SerializeField] protected float transitionSpeed = 5f;
    [SerializeField] protected float visibilityThreshold = 0.01f;

    protected Slider slider;
    protected Image fillImage;

    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();

        if (slider.fillRect != null)
        {
            fillImage = slider.fillRect.GetComponent<Image>();
        }
    }

    // Logica generica para actualiar valores
    protected void UpdateBarValue(int currentValue, int maxValue)
    {
        // Actualizar el maximo por si sube de nivel en un futuro
        slider.maxValue = maxValue;

        if (smoothTransition)
        {
            // Iniciar la corrutina para animar la barra
            StopAllCoroutines();
            StartCoroutine(AnimateBar(currentValue));
        }
        else
        {
            slider.value = currentValue;
            UpdateFillVisibility();
        }
    }

    // Corrutina para efecto visual suave
    private IEnumerator AnimateBar(float targetValue)
    {
        while (Mathf.Abs(slider.value - targetValue) > 0.01f)
        {
            // Time.unscaleDeltaTime ignora el Time.timeScale = 0 del Game Over. Esto es para que no se pare la barra al pararse el juego
            slider.value = Mathf.Lerp(slider.value, targetValue, Time.unscaledDeltaTime * transitionSpeed);
            yield return null;
        }
        slider.value = targetValue;
        UpdateFillVisibility();
    }

    // Método auxiliar para prender/apagar el sprite
    private void UpdateFillVisibility()
    {
        if (fillImage != null)
        {
            // Para no mostrar el sprite aunque el value sea 0
            fillImage.enabled = slider.value > visibilityThreshold;
        }
    }
}
