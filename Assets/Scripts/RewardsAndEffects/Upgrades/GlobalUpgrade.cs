using System.Collections;
using UnityEngine;

public enum GlobalStatType { MoveSpeed, MaxHealth, MaxMana }

[CreateAssetMenu(menuName = "Upgrades/Global Stat Upgrade")]
public class GlobalUpgrade : UpgradeEffect
{
    public GlobalStatType statType;
    public float amount;

    public override void Apply(GameObject target)
    {
        PlayerStats stats = target.GetComponent<PlayerStats>();
        SystemHealth health = target.GetComponent<SystemHealth>();
        PlayerMana mana = target.GetComponent<PlayerMana>();

        if (stats != null)
        {
            switch (statType)
            {
                case GlobalStatType.MaxHealth:
                    if (health != null)
                    {
                        health.IncreaseMaxHealth(Mathf.RoundToInt(amount));
                    }
                    break;
                case GlobalStatType.MaxMana:
                    if (mana != null )
                    {
                        mana.IncrementMana(Mathf.RoundToInt(amount));
                    }
                    break;
                case GlobalStatType.MoveSpeed:
                    stats.extraMoveSpeed += amount;
                    break;
            }
            Debug.Log($"Mejora Global aplicada: {statType} aumentada en {amount}");
        }
    }
}