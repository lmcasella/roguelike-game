using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMana))]
public class PlayerAbilities : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private Ability startingBasicAttack;
    [SerializeField] private Ability startingAbility1;
    [SerializeField] private Ability startingAbility2;
    [SerializeField] private Ability startingAbility3;

    private PlayerMana playerMana;
    private PlayerStats playerStats;

    // Diccionarios para guardar las habilidades equipadas y sus cooldowns
    private Dictionary<AbilitySlot, Ability> equippedAbilities = new Dictionary<AbilitySlot, Ability>();
    private Dictionary<AbilitySlot, float> abilityCooldowns = new Dictionary<AbilitySlot, float>();

    private void Awake()
    {
        playerMana = GetComponent<PlayerMana>();
        playerStats = GetComponent<PlayerStats>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Equipar habilidades
        if (startingBasicAttack != null)
        {
            EquipAbility(startingBasicAttack);
        }
        if (startingAbility1 != null)
        {
            EquipAbility(startingAbility1);
        }
        if (startingAbility2 != null)
        {
            EquipAbility(startingAbility2);
        }
        if (startingAbility3 != null)
        {
            EquipAbility(startingAbility3);
        }

        // Inicializar cooldowns para todos los slots
        //NOTE: System.Enum.GetValues(typeof(AbilitySlot)) -> Get de todos los valores de tipo AbilitySlot (Basic, Ability1, Ability2, Ability3)
        foreach (AbilitySlot slot in System.Enum.GetValues(typeof(AbilitySlot)))
        {
            // Ya existen habilidades asignadas?
            if (!abilityCooldowns.ContainsKey(slot))
            {
                // No? -> Se crea y se asigna -1f
                abilityCooldowns[slot] = -1f; // -1 == Listo
            }
        }
    }

    //NOTE:
    // Equipar una nueva habilidad, reemplazar cualquiera que esté en ese slot
    // Se llama al inicio, y se va a llamar al terminar una sala/nivel para mejorar habilidades
    public void EquipAbility(Ability ability)
    {
        if (ability == null) return;

        // La habilidad que se pasa por parametro tiene un slot asignado. Se asigna al crear el ScriptableObject desde Unity
        // con equippedAbilities[ability.slot] = ability; le decimos el slot que 'ability' tiene asignado buscalo en el diccionario 'equippedAbilities' y asigna la habilidad a esa key
        equippedAbilities[ability.slot] = ability;
        abilityCooldowns[ability.slot] = Time.time;
        Debug.Log($"Equipped {ability.abilityName} in {ability.slot}");
    }

    // Update is called once per frame
    void Update()
    {
        // --- Input ---

        // Basic Attack (Fire1)
        if (Input.GetButton("Fire1"))
        {
            TryUseAbility(AbilitySlot.Basic);
        }

        //IMPORTANT: GetKeyDown es mejor para no spammear teclas
        // Ability 1 (Q)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryUseAbility(AbilitySlot.Ability1);
        }

        // Ability 2 (W)
        if (Input.GetKeyDown(KeyCode.W))
        {
            TryUseAbility(AbilitySlot.Ability2);
        }

        // Ability 3 (E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryUseAbility(AbilitySlot.Ability3);
        }
    }

    private void TryUseAbility(AbilitySlot slot)
    {
        // 1. Tengo la habilidad equipada?
        if (!equippedAbilities.ContainsKey(slot))
        {
            Debug.Log($"No ability in slot {slot}");
            return;
        }

        // 2. Ya no está en cooldown?
        if (Time.time < abilityCooldowns[slot])
        {
            Debug.Log($"Ability {slot} on cooldown");
            return;
        }

        // 3. Get habilidad mediante su key (slot)
        Ability ability = equippedAbilities[slot];


        // --- Calcular Stats Finales ---
        // 4. Valores base de la habilidad
        int finalDamage = ability.damage;
        int finalManaCost = ability.manaCost;
        int finalProjectiles = 1;

        // 5. Switch para aplicar los bonus de PlayerStats
        switch (slot)
        {
            case AbilitySlot.Basic:
                finalDamage += playerStats.basicDamageBonus;
                finalProjectiles += playerStats.basicExtraProjectiles;
                break;

            case AbilitySlot.Ability1:
                finalDamage += playerStats.ability1DamageBonus;
                finalManaCost += playerStats.ability1ManaCostBonus; // ej. 20 + (-5) = 15
                break;

            case AbilitySlot.Ability2:
                finalDamage += playerStats.ability2DamageBonus;
                finalManaCost += playerStats.ability2ManaCostBonus;
                break;

            case AbilitySlot.Ability3:
                finalDamage += playerStats.ability3DamageBonus;
                finalManaCost += playerStats.ability3ManaCostBonus;
                break;
        }

        // 6. Asegurarse de que el maná nunca sea negativo
        if (finalManaCost < 0) finalManaCost = 0;

        // 7. Chequeamos Maná usando el costo final
        if (!playerMana.HasEnoughMana(finalManaCost))
        {
            Debug.Log("Not enough mana!");
            return;
        }

        // --- Si está todo okay, se lanza la habilidad ---

        // 1. Gastar mana
        playerMana.SpendMana(ability.manaCost);

        // 2. Set cooldown
        abilityCooldowns[slot] = Time.time + ability.cooldown;

        // 3. Lanzar habilidad
        if (ability.projectilePrefab != null)
        {
            GameObject projObj = Instantiate(ability.projectilePrefab, firePoint.position, firePoint.rotation);

            // Le pasamos el daño final al proyectil
            Projectile projScript = projObj.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.Initialize(finalDamage);
            }
        }

        Debug.Log($"Used ability: {ability.abilityName} (Dmg: {finalDamage}, Cost: {finalManaCost})");
    }
}
