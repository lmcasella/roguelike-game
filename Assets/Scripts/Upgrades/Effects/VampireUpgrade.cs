using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Vampire Effect")]
public class VampireUpgrade : UpgradeEffect
{
    public override void Apply(GameObject target)
    {
        var behavior = target.GetComponent<VampireBehaviour>();

        if (behavior == null)
        {
            target.AddComponent<VampireBehaviour>();
            Debug.Log("¡Ahora eres un vampiro!");
        }
        else
        {
            Debug.Log("Ya tienes vampirismo.");
        }
    }
}