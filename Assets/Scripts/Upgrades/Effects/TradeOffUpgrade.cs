using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Trade-Off (Risk vs Reward)")]
public class TradeOffUpgrade : UpgradeEffect
{
    [Header("Beneficio")]
    public int damageBonus = 0;
    public int healthBonus = 0;
    public float speedMultiplier = 1f;

    [Header("Sacrificio")]
    public int manaPenalty = 0;
    public int maxHealthPenalty = 0;
    public float cooldownPenaltyMultiplier = 1f;

    private string upgradeName;

    public override void Apply(GameObject target)
    {
        var stats = target.GetComponent<PlayerStats>();
        var health = target.GetComponent<SystemHealth>();
        var mana = target.GetComponent<PlayerMana>();

        // 1. Aplicar Beneficios
        if (stats) stats.basicDamageBonus += damageBonus;
        if (health && healthBonus > 0) health.Heal(healthBonus); // O subir max health

        // 2. Aplicar Sacrificios
        if (mana)
        {
            // Restar maná máximo (necesitarás implementar ModifyMaxMana si no existe)
            // mana.ModifyMaxMana(-manaPenalty); 
        }

        if (health && maxHealthPenalty > 0)
        {
            // Restar vida máxima (necesitarás implementar ModifyMaxHealth)
            // health.ModifyMaxHealth(-maxHealthPenalty);
        }

        Debug.Log($"Trade-off aplicado: {upgradeName}");
    }
}