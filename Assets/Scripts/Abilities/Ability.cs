using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define diferentes tipos de slots para habilidades
public enum AbilitySlot { Basic, Ability1, Ability2, Dash }

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
    public int damage = 10;
    public int manaCost = 0;
    public float cooldown = 0.5f;

    // TODO: Tipos de elementos...
}