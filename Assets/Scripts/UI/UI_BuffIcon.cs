using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuffIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownOverlay; // La imagen oscura "filled"
    [SerializeField] private TextMeshProUGUI title;

    public void Initialize(BuffEffect buff, float duration)
    {
        // 1. Configurar imagen
        if (buff.icon != null)
        {
            iconImage.sprite = buff.icon;
            title.text = buff.buffName;
        }

        // 2. Iniciar la animación de desaparición
        StartCoroutine(DurationRoutine(duration));
    }

    private IEnumerator DurationRoutine(float duration)
    {
        float timer = duration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            // Actualizar el reloj (Fill Amount va de 1 a 0)
            if (cooldownOverlay != null)
            {
                cooldownOverlay.fillAmount = timer / duration;
            }

            yield return null;
        }

        // 3. Al terminar el tiempo, destruir este objeto de la UI
        Destroy(gameObject);
    }
}
