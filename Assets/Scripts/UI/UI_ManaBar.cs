using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UI_ManaBar : MonoBehaviour
{
    private Slider manaSlider;

    private void Awake()
    {
        manaSlider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        GameEvents.OnPlayerManaChanged += UpdateManaBar;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerManaChanged -= UpdateManaBar;
    }

    // Actualiza la barra de maná
    private void UpdateManaBar(int currentMana, int maxMana)
    {
        // 1. Setea el valor máximo del Slider (por si el maná máximo cambia)
        manaSlider.maxValue = maxMana;

        // 2. Setea el valor actual
        manaSlider.value = currentMana;
    }
}
