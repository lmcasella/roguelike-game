using UnityEngine;

public abstract class UpgradeEffect : ScriptableObject
{
    [Header("Info General")]
    //public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;

    // Método abstracto: Qué hace la mejora cuando se elige
    public abstract void Apply(GameObject target);
}