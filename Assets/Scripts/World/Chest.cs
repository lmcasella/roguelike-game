using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private bool isOpen = false;
    // TODO: Item drop

    [Header("Visuals")]
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closedSprite;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisuals();
    }

    public void Interact(Player player)
    {
        if (isOpen) return;

        Debug.Log("Abriste el cofre");

        isOpen = true;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isOpen ? openSprite : closedSprite;
        }
    }
}
