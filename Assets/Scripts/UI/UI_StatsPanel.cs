using UnityEngine;
using TMPro;

public class UI_StatsPanel : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("Asignar los 4 textos correspondientes a Básico, Habilidad 1, 2 y 3")]
    [SerializeField] private TextMeshProUGUI[] abilityTexts = new TextMeshProUGUI[4];

    [Header("Colores")]
    [SerializeField] private string hexDamageColor = "#00FF00"; // Verde
    [SerializeField] private string hexCDColor = "#00FFFF";     // Cyan / Azul claro

    private void OnEnable()
    {
        // Cuando el panel se activa, empieza a escuchar
        GameEvents.OnPlayerStatsChanged += UpdateUI;
    }

    private void OnDisable()
    {
        // Cuando el panel se apaga o se destruye, deja de escuchar por seguridad
        GameEvents.OnPlayerStatsChanged -= UpdateUI;
    }

    // Método que deberás llamar desde un evento global (ej: GameEvents.OnStatsChanged)
    public void UpdateUI(PlayerAbilities playerAbilities, PlayerStats playerStats)
    {
        // Creamos un array con el orden exacto de los slots que queremos mostrar
        AbilitySlot[] slotsToDisplay = new AbilitySlot[]
        {
            AbilitySlot.Basic,
            AbilitySlot.Ability1,
            AbilitySlot.Ability2,
            AbilitySlot.Dash
        };

        for (int i = 0; i < abilityTexts.Length; i++)
        {
            // Nos aseguramos de no salirnos de los límites
            if (i >= slotsToDisplay.Length) break;

            AbilitySlot currentSlot = slotsToDisplay[i];

            // 1. Obtenemos la habilidad usando el nuevo método
            Ability ability = playerAbilities.GetEquippedAbility(currentSlot);

            if (ability != null)
            {
                // 2. Obtenemos el contenedor de stats para ese slot
                AbilityStatsContainer slotStats = playerStats.GetContainerForSlot(currentSlot);

                // 3. Calculamos los valores finales (igual que en TryUseAbility)
                int finalMin = ability.minDamage;
                int finalMax = ability.maxDamage;

                float finalCD = ability.cooldown;

                if (slotStats != null)
                {
                    finalMin += slotStats.damageBonus;
                    finalMax += slotStats.damageBonus;

                    finalCD -= slotStats.cooldownReduction;
                    if (finalCD < 0) finalCD = 0; // Evitar tiempos negativos
                }

                // 4. Formateamos el texto
                abilityTexts[i].text = $"{ability.abilityName}\n" +
                                       $"Daño: <color={hexDamageColor}>{finalMin} - {finalMax}</color>\n" +
                                       $"CD: <color={hexCDColor}>{finalCD}s</color>";
            }
            else
            {
                // Si el slot está vacío, limpiamos el texto o mostramos "Vacío"
                abilityTexts[i].text = $"Slot {currentSlot}\n<color=#888888>Vacío</color>";
            }
        }
    }
}