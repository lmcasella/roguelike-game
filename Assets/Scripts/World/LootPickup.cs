using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class LootPickup : MonoBehaviour
{
    [Header("Datos")]
    [SerializeField] private BuffEffect buffEffect; // ScriptableObject del efecto

    // Para hacer que el ítem "flote"
    private float floatSpeed = 2f;
    private float floatAmplitude = 0.1f;
    private Vector3 startPos;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    // Este método lo llamaremos al "Spawnear" el ítem desde un cofre o enemigo
    public void Initialize(BuffEffect effect)
    {
        buffEffect = effect;
        if (buffEffect != null && buffEffect.icon != null)
        {
            spriteRenderer.sprite = buffEffect.pickupSprite != null ? buffEffect.pickupSprite : buffEffect.icon;
        }
    }

    private void Update()
    {
        // Pequeña animación de flotación (Juice!)
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (buffEffect != null)
            {
                // INTENTAMOS aplicar el buff
                bool wasApplied = buffEffect.Apply(collision.gameObject);

                // Solo si se aplicó (fue útil), destruimos el objeto del suelo
                if (wasApplied)
                {
                    // Opcional: Sonido de pickup
                    // AudioManager.Instance.PlaySoundEffect(pickupSound);
                    Destroy(gameObject);
                }
            }
        }
    }
}