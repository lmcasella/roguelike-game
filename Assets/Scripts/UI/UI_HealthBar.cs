using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HealthBar : UI_BarBase
{
    private void OnEnable()
    {
        // Suscribirse a eventos de cambio de salud
        GameEvents.OnPlayerHealthChanged += UpdateBarValue;
    }

    private void OnDisable()
    {
        // Desuscribirse de eventos para evitar fugas de memoria
        GameEvents.OnPlayerHealthChanged -= UpdateBarValue;
    }
}
