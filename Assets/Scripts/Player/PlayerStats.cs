using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AbilityStatsContainer
{
    public int damageBonus = 0;
    public int manaCostReduction = 0;
    public int extraProjectiles = 0;
    public float cooldownReduction = 0f;
    public float moveSpeedBonus = 0f;

    // Esta función procesa la matemática según el tipo de stat
    public void AddModifier(StatModifier mod)
    {
        switch (mod.statType)
        {
            case StatType.Damage:
                damageBonus += Mathf.RoundToInt(mod.amount);
                break;
            case StatType.ManaCost:
                manaCostReduction += Mathf.RoundToInt(mod.amount);
                break;
            case StatType.ExtraProjectiles:
                extraProjectiles += Mathf.RoundToInt(mod.amount);
                break;
            case StatType.CooldownReduction:
                cooldownReduction += mod.amount;
                break;
            case StatType.MoveSpeed:
                moveSpeedBonus += mod.amount;
                break;
        }
    }
}

//NOTE: Guardar todos los stats del Player
public class PlayerStats : MonoBehaviour
{
    [Header("Estadísticas Globales")]
    public float extraMoveSpeed = 0f;
    public int extraMaxHealth = 0;
    public int extraMaxMana = 0;

    [Header("Estadísticas por Habilidad")]
    public AbilityStatsContainer basicStats = new AbilityStatsContainer();
    public AbilityStatsContainer ability1Stats = new AbilityStatsContainer();
    public AbilityStatsContainer ability2Stats = new AbilityStatsContainer();

    // Este es el método que recibe la lista desde AbilityUpgrade
    public void ApplyModifiers(AbilitySlot slot, List<StatModifier> modifiers)
    {
        AbilityStatsContainer targetContainer = GetContainerForSlot(slot);

        if (targetContainer == null) return;

        // Iteramos sobre todos los modificadores de la carta que eligió el jugador
        foreach (StatModifier mod in modifiers)
        {
            targetContainer.AddModifier(mod);
        }

        // Buscamos las habilidades en este mismo objeto y avisamos a la UI
        PlayerAbilities abilities = GetComponent<PlayerAbilities>();
        if (abilities != null)
        {
            GameEvents.ReportStatsChanged(abilities, this);
        }
    }

    // Método auxiliar para aislar la lógica de selección y mantener limpio el código
    public AbilityStatsContainer GetContainerForSlot(AbilitySlot slot)
    {
        switch (slot)
        {
            case AbilitySlot.Basic: return basicStats;
            case AbilitySlot.Ability1: return ability1Stats;
            case AbilitySlot.Ability2: return ability2Stats;
            default: return null;
        }
    }
}
