using UnityEngine;

public abstract class BuffEffect : ScriptableObject
{
    [Header("Visuals")]
    [Tooltip("Icono cuadrado para la UI (Panel izquierdo)")]
    public Sprite icon;
    
    [Tooltip("Sprite para el objeto tirado en el suelo")]
    public Sprite pickupSprite;

    public string buffName;
    [TextArea] public string description;
    // Duracion para que la UI sepa cuanto dura el CD
    public virtual float GetDuration() { return 0f; }

    public abstract bool Apply(GameObject target);
}
