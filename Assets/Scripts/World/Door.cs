using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))] // Necesita un collider para bloquear al jugador
[RequireComponent(typeof(SpriteRenderer))] // Para cambiar su aspecto (abierta/cerrada)
public class Door : MonoBehaviour
{
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite cloneSprite;

    private Collider2D doorCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Open()
    {
        doorCollider.enabled = false;
        spriteRenderer.sprite = openSprite;
    }

    public void Close()
    {
        doorCollider.enabled = true;
        spriteRenderer.sprite = cloneSprite;
    }
}
