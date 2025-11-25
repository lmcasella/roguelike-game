using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Speed Effect")]
public class SpeedBuffEffect : BuffEffect
{
    public float speedMultiplier = 1.5f;
    public float duration = 5f;

    public override bool Apply(GameObject target)
    {
        var player = target.GetComponent<Player>();
        if (player != null)
        {
            player.StartCoroutine(player.ActivateSpeedBoost(speedMultiplier, duration));
            GameEvents.ReportBuffApplied(this, duration);
            Debug.Log($"Buff Velocidad aplicado: x{speedMultiplier} por {duration}s");
        }

        return true;
    }

    public override float GetDuration() => duration;
}
