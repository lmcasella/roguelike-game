using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definir todos los stats posibles del juevo en un Enum
public enum StatType
{
    Damage,
    ManaCost,
    ExtraProjectiles,
    CooldownReduction,
    MoveSpeed
}

// Crear estructura que relacione un Stat con un Valor
[System.Serializable]
public struct StatModifier
{
    public StatType statType;
    public float amount; // float para daño o CD
}

// Mejoras a las habilidades
[CreateAssetMenu(fileName = "New Stat Upgrade", menuName = "Upgrades/Stat Upgrade")]
public class AbilityUpgrade : UpgradeEffect
{
    //public string description;
    public AbilitySlot targetAbility; // A qué habilidad afecta

    [Header("Modificadores")]
    [Tooltip("Agrega a la lista solo los stats que esta carta va a modificar")]
    public List<StatModifier> modifiers = new List<StatModifier>();

    public override void Apply(GameObject target)
    {
        // Buscamos los stats en el objetivo (Player)
        PlayerStats stats = target.GetComponent<PlayerStats>();

        if (stats != null)
        {
            stats.ApplyModifiers(targetAbility, modifiers);
            Debug.Log($"Mejora de Stats aplicada: {name}");
        }
    }
}
