using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static BuffChest;

public class BuffChest : MonoBehaviour, IInteractable
{
    public enum BuffType { ManaFrenzy, SpeedBoost, Healing}

    [Header("Configuración")]
    [SerializeField] private BuffType buffType;
    [SerializeField] private float duration = 10f;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private Sprite openSprite;

    private bool isOpen = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Interact(Player player)
    {
        if (isOpen) return; // Si ya está abierto, no hacemos nada

        OpenChest();
        ApplyBuff(player);
    }

    private void OpenChest()
    {
        isOpen = true;
        spriteRenderer.sprite = openSprite; // Cambia al sprite abierto

        // Aquí podrías disparar un trigger de Animator si tienes animación completa
        // GetComponent<Animator>().SetTrigger("Open");

        if (openSound != null)
        {
            AudioManager.Instance.PlaySoundEffect(openSound);
        }

        // Desactivamos el collider para que el detector del Player ya no lo vea
        // Opcional: Si quieres que siga siendo obstáculo físico, cambia el Layer a "Default"
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    private void ApplyBuff(Player player)
    {
        Debug.Log($"¡Buff aplicado: {buffType}!");

        switch (buffType)
        {
            case BuffType.ManaFrenzy:
                PlayerMana mana = player.GetComponent<PlayerMana>();
                if (mana) StartCoroutine(mana.ActivateInfiniteMana(duration));
                break;

            case BuffType.SpeedBoost:
                // Llamamos a la corrutina en el script del Player
                player.StartCoroutine(player.ActivateSpeedBoost(1.5f, duration));
                break;

            case BuffType.Healing:
                SystemHealth health = player.GetComponent<SystemHealth>();
                // Curamos un 30% de la vida total
                if (health) health.Heal((int)(health.GetMaxHealth() * 0.3f));
                break;
        }
    }
}
