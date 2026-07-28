using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Mana Incr Effect")]
public class ManaIncBuffEffect : BuffEffect
{
    [SerializeField] private int manaRestoreAmount = 25; // Cantidad de maná que se recupera

    public override bool Apply(GameObject target)
    {
        var mana = target.GetComponent<PlayerMana>();
        if (mana != null)
        {
            // Checkear si el mana del Player esta full
            if (mana.GetCurrentMana() >= mana.GetMaxMana())
            {
                Debug.Log("Mana llena, no se consume la poción.");
                return false;
            }

            // Restaurar el maná instantáneamente
            mana.RestoreMana(manaRestoreAmount);

            Debug.Log($"Maná restaurado: +{manaRestoreAmount}");
            return true;
        }

        return false;
    }

    public override float GetDuration() => 0f;
}
