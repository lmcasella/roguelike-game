using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomSprite : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Arrastrar aca todas las variantes visuales")]
    [SerializeField] private Sprite[] possibleSprites;

    [Header("Opcional")]
    [Tooltip("Si es true, también volteará el sprite horizontalmente al azar para más variedad")]
    [SerializeField] private bool randomFlipX = false;

    private void Awake() { Apply(); }
    private void OnValidate() { Apply(); }

    private void Apply()
    {
        // Validación de seguridad
        if (possibleSprites == null || possibleSprites.Length == 0) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // 1. Elegir uno al azar
        int randomIndex = Random.Range(0, possibleSprites.Length);
        sr.sprite = possibleSprites[randomIndex];

        // 2. Espejar
        if (randomFlipX)
        {
            sr.flipX = (Random.value > 0.5f);
        }
    }
}