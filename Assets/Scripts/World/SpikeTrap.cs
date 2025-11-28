using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageCooldown = 1f; // Para no matar instantáneamente
    [SerializeField] private Sprite spikeOn;
    [SerializeField] private Sprite spikeOff;
    private float nextDamageTime = 0f;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time < nextDamageTime) return;

        // Busca daño en Player o Enemigos
        IDamageable target = collision.GetComponent<IDamageable>();

        if (target != null)
        {
            sr.sprite = spikeOn;
            target.TakeDamage(damage);
            nextDamageTime = Time.time + damageCooldown;
            Debug.Log($"{collision.name} pisó pinchos!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        sr.sprite = spikeOff;
    }
}