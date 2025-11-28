using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class LootPickup : MonoBehaviour
{
    [Header("Datos")]
    [SerializeField] private BuffEffect buffEffect; // ScriptableObject del efecto
    [SerializeField] private AudioClip pickupSound;

    // Para hacer que el ítem "flote"
    private float floatSpeed = 2f;
    private float floatAmplitude = 0.1f;
    private Vector3 startPos;
    private bool isCollected = false;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPos = transform.position;
    }

    // Este método se llama al morir un enemigo o romper un objeto
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
        // FIXME: Animacion desde Animator
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;

        if (collision.CompareTag("Player"))
        {
            if (buffEffect != null)
            {
                // Se intenta aplicar el buff
                bool wasApplied = buffEffect.Apply(collision.gameObject);

                // Solo si se aplicó, destruir el objeto del suelo
                if (wasApplied)
                {
                    isCollected = true;

                    AudioManager.Instance.PlaySoundEffect(pickupSound);
                    Destroy(gameObject);
                }
            }
        }
    }
}