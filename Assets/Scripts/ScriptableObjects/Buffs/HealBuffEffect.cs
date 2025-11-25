using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Heal Effect")]
public class HealBuffEffect : BuffEffect
{
    [Range(1, 100)] public float percentPerSecond = 5f;
    public float duration = 5f;

    public override bool Apply(GameObject target)
    {
        var health = target.GetComponent<SystemHealth>();
        if (health != null)
        {
            // Checkear si la vida del Player esta full
            if (health.GetCurrentHealth() >= health.GetMaxHealth())
            {
                Debug.Log("Vida llena, no se consume la poción.");
                return false;
            }

            // 1. Iniciar la regeneración
            health.Regenerate(percentPerSecond, duration);

            // 2. Avisar a la UI
            GameEvents.ReportBuffApplied(this, duration);

            Debug.Log($"Regeneración aplicada: {percentPerSecond}%/seg por {duration}s");
            return true;
        }

        return false;
    }

    public override float GetDuration() => duration;
}
