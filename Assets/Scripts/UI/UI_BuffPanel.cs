using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BuffPanel : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject buffIconPrefab; // Los iconos de buff
    [SerializeField] private Transform container; // El objeto padre con VerticalLayoutGroup

    private void OnEnable()
    {
        GameEvents.OnBuffApplied += AddBuffIcon;
    }

    private void OnDisable()
    {
        GameEvents.OnBuffApplied -= AddBuffIcon;
    }

    private void AddBuffIcon(BuffEffect buff, float duration)
    {
        // Si la duración es muy corta (ej. curación instantánea), no mostramos icono
        if (duration < 0.1f) return;

        // Crear el icono dentro del contenedor
        GameObject newIconObj = Instantiate(buffIconPrefab, container);

        // Configurarlo
        UI_BuffIcon iconScript = newIconObj.GetComponent<UI_BuffIcon>();
        if (iconScript != null)
        {
            iconScript.Initialize(buff, duration);
        }
    }
}
