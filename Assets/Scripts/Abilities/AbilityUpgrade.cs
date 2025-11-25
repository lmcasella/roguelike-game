using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stat Upgrade", menuName = "Upgrades/Stat Upgrade")]
public class AbilityUpgrade : UpgradeEffect
{
    //public string description;
    public AbilitySlot targetAbility; // A qué habilidad afecta

    // Qué stat mejora
    public int damageIncrease = 0;
    public int manaCostReduction = 0;
    public int extraProjectiles = 0;

    public override void Apply(GameObject target)
    {
        // Buscamos los stats en el objetivo (Player)
        PlayerStats stats = target.GetComponent<PlayerStats>();

        if (stats != null)
        {
            stats.ApplyUpgrade(this);
            Debug.Log($"Mejora de Stats aplicada: {name}");
        }
    }
}
