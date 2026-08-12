using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define diferentes tipos de slots para habilidades
public enum AbilitySlot { Basic, Ability1, Ability2, Dash }

public enum AimType { Instant, Cone, Area }

[CreateAssetMenu(fileName = "New Ability", menuName = "Abilities/New Ability")]
public class Ability : ScriptableObject
{
    [Header("Visual")]
    public Sprite icon;
    public AudioClip useSound;

    [Header("Info")]
    public string abilityName;
    [TextArea] public string abilityDescription;
    public AbilitySlot slot;

    [Header("Stats")]
    public GameObject projectilePrefab;
    [Tooltip("Daño mínimo que puede hacer la habilidad")]
    public int minDamage = 10;
    [Tooltip("Daño máximo que puede hacer la habilidad")]
    public int maxDamage = 15;
    public int manaCost = 0;
    public float cooldown = 0.5f;

    [Header("Configuración de Apuntado")]
    public AimType aimType = AimType.Instant;

    // TODO: Tipos de elementos...
}