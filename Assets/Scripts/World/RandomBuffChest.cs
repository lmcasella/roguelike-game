using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class RandomBuffChest : MonoBehaviour, IInteractable
{
    [Header("Configuración de Loot")]
    [Tooltip("Arrastrar todos los ScriptableObjects de buffs posibles")]
    [SerializeField] private List<BuffEffect> possibleBuffs;

    [Header("Visuals")]
    [SerializeField] private Sprite openSprite;
    [SerializeField] private AudioClip openSound;

    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact(Player player)
    {
        if (isOpen) return;

        // Validación de seguridad
        if (possibleBuffs == null || possibleBuffs.Count == 0)
        {
            Debug.LogWarning("¡Este cofre no tiene buffs asignados en el inspector!");
            return;
        }

        OpenChest(player);
    }

    private void OpenChest(Player player)
    {
        isOpen = true;
        spriteRenderer.sprite = openSprite;

        // --- LOGICA ALEATORIA ---
        // 1. Elegir un índice al azar
        int randomIndex = Random.Range(0, possibleBuffs.Count);
        BuffEffect selectedBuff = possibleBuffs[randomIndex];

        // 2. Aplicar el buff seleccionado
        // Le pasamos el gameObject del player para que el SO busque los componentes
        selectedBuff.Apply(player.gameObject);

        // --- FEEDBACK ---
        if (openSound != null)
        {
            AudioManager.Instance.PlaySoundEffect(openSound);
        }

        // TODO: Mostrar icono de efecto temporal en pantalla (UI)
        Debug.Log($"¡Obtuviste: {selectedBuff.buffName}!");

        // Desactivamos interacción
        GetComponent<Collider2D>().enabled = false;
    }
}
