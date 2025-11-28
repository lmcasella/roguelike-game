using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AbilitySlot : MonoBehaviour
{
    [Header("Configuración del Slot")]
    [SerializeField] private AbilitySlot targetSlot; // Configurar en Inspector: Basic, Ability1, etc.

    [Header("Referencias UI")]
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownOverlay; // Imagen negra con transparencia encima del icono

    private void Awake()
    {
        // Limpiamos el icono si no hay nada, o ponemos uno default transparente
        iconImage.enabled = false;
        cooldownOverlay.fillAmount = 0;
    }

    private void OnEnable()
    {
        GameEvents.OnAbilityEquipped += HandleAbilityEquipped;
        GameEvents.OnAbilityCooldownStarted += HandleCooldown;
    }

    private void OnDisable()
    {
        GameEvents.OnAbilityEquipped -= HandleAbilityEquipped;
        GameEvents.OnAbilityCooldownStarted -= HandleCooldown;
    }

    // 1. Cuando se equipa una habilidad
    private void HandleAbilityEquipped(AbilitySlot slot, Ability ability)
    {
        // Solo nos importa si es para mi slot
        if (slot != targetSlot) return;

        if (ability != null && ability.icon != null)
        {
            iconImage.sprite = ability.icon;
            iconImage.enabled = true;

            // Reseteamos el cooldown visual
            cooldownOverlay.fillAmount = 0;
        }
        else
        {
            iconImage.enabled = false;
        }
    }

    // 2. Cuando empieza el cooldown
    private void HandleCooldown(AbilitySlot slot, float duration)
    {
        if (slot != targetSlot) return;

        // Iniciamos la animación visual
        StartCoroutine(CooldownRoutine(duration));
    }

    private IEnumerator CooldownRoutine(float duration)
    {
        float timer = 0f;
        cooldownOverlay.fillAmount = 1f; // Llenamos el reloj negro

        while (timer < duration)
        {
            timer += Time.deltaTime;
            // Se va vaciando el reloj: (1 - porcentaje completado)
            cooldownOverlay.fillAmount = 1f - (timer / duration);
            yield return null;
        }

        cooldownOverlay.fillAmount = 0f; // Asegurar que quede limpio
    }
}
