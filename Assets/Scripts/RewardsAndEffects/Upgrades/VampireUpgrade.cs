using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Vampire Effect")]
public class VampireUpgrade : UpgradeEffect
{
    [SerializeField] private int healOnKillAmount;

    public override void Apply(GameObject target)
    {
        var behavior = target.GetComponent<VampireBehaviour>();

        if (behavior == null)
        {
            behavior = target.AddComponent<VampireBehaviour>();
            behavior.Initialize(healOnKillAmount);
            Debug.Log("Vampirismo activado");
        }
        else
        {
            Debug.Log("Ya tenes vampirismo");
        }
    }
}