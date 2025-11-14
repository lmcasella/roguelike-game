using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sprite))]
public class EnemyMelee : EnemyAI
{
    [SerializeField] private int damage = 10;

    protected override void AttackTarget()
    {
        SystemHealth targetHealth = target.GetComponent<SystemHealth>();
        if (targetHealth != null)
        {
            targetHealth.DealDamage(damage);
            Debug.Log($"EnemyMelee attacked {target.name} for {damage} damage.");
        }
    }
}
