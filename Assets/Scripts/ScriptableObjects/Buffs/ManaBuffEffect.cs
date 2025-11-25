using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Infinite Mana Effect")]
public class ManaBuffEffect : BuffEffect
{
    public float duration = 5f;

    public override bool Apply(GameObject target)
    {
        var mana = target.GetComponent<PlayerMana>();
        if (mana != null)
        {
            mana.StartCoroutine(mana.ActivateInfiniteMana(duration));
            GameEvents.ReportBuffApplied(this, duration);
            Debug.Log($"Buff Maná Infinito aplicado por {duration}s");
        }

        return true;
    }

    public override float GetDuration() => duration;
}
