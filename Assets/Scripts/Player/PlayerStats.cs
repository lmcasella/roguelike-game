using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE: Guardar todos los stats del Player
public class PlayerStats : MonoBehaviour
{
    [Header("Basic Attack (Fire1)")]
    public int basicDamageBonus = 0;
    public int basicExtraProjectiles = 0;

    [Header("Ability 1 (Q)")]
    public int ability1DamageBonus = 0;
    public int ability1ManaCostBonus = 0;

    [Header("Ability 2 (W)")]
    public int ability2DamageBonus = 0;
    public int ability2ManaCostBonus = 0;

    [Header("Ability 3 (E)")]
    public int ability3DamageBonus = 0;
    public int ability3ManaCostBonus = 0;

    // Se utiliza en la UI_RewardScreen.cs
    public void ApplyUpgrade(AbilityUpgrade upgrade)
    {
        switch (upgrade.targetAbility)
        {
            case AbilitySlot.Basic:
                basicDamageBonus += upgrade.damageIncrease;
                basicExtraProjectiles += upgrade.extraProjectiles;
                break;
            case AbilitySlot.Ability1:
                ability1DamageBonus += upgrade.damageIncrease;
                ability1ManaCostBonus += upgrade.manaCostReduction;
                break;
            case AbilitySlot.Ability2:
                ability2DamageBonus += upgrade.damageIncrease;
                ability2ManaCostBonus += upgrade.manaCostReduction;
                break;
            case AbilitySlot.Ability3:
                ability3DamageBonus += upgrade.damageIncrease;
                ability3ManaCostBonus += upgrade.manaCostReduction;
                break;
        }
    }
}
