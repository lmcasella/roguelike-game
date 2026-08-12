using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sprite))]
public class EnemyMelee : EnemyAI
{
    //[SerializeField] private int minDamage = 8;
    //[SerializeField] private int maxDamage = 12;

    //int rolledDamage;

    protected override void AttackTarget()
    {
        SystemHealth targetHealth = target.GetComponent<SystemHealth>();
        if (targetHealth != null)
        {
            int damageToApply = GetRolledDamage();

            targetHealth.DealDamage(damageToApply);
            Debug.Log($"EnemyMelee attacked {target.name} for {damageToApply} damage.");
        }
    }
}
