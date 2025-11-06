using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Abilities/Upgrade")]
public class AbilityUpgrade : ScriptableObject
{
    public string description;
    public AbilitySlot targetAbility; // A qué habilidad afecta

    // Qué stat mejora
    public int damageIncrease = 0;
    public int manaCostReduction = 0;
    public int extraProjectiles = 0;
}
