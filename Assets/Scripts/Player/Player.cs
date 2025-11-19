using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SystemHealth))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour, IDamageable
{
    [Header("Health")]
    private SystemHealth healthComponent;

    // Getter
    // '=>' reemplaza '{ get { return ...; } }'
    public int CurrentHealth => healthComponent.GetCurrentHealth();

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [Tooltip("Multiplicador de velocidad mientras ataca (ej: 0.5 = 50% más lento)")]
    [SerializeField] private float attackSlowdownMultiplier = 0.5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Aiming & Animation")]
    [SerializeField] private float attackLookDuration = 0.4f; // Cuánto tiempo mira al mouse

    [Header("Audio")]
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip deathSound;

    [Header("Visual Feedback")]
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Camera mainCam;
    private bool isAttacking = false;
    private float attackLookTimer = 0f;
    private bool isWalking = false;
    private Color originalColor;
    private Animator animator;

    private void Awake()
    {
        healthComponent = GetComponent<SystemHealth>();
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCam = Camera.main;

        animator = GetComponent<Animator>();

        if (healthComponent == null )
        {
            Debug.LogError("Player is missing a Health component");
        }
    }

    private void Start() // Asegúrate de tener Start
    {
        originalColor = spriteRenderer.color;
    }

    // Suscribirse a eventos
    private void OnEnable()
    {
        GameEvents.OnPlayerAttack += HandleAttackAim;
    }

    // Desuscribirse
    private void OnDisable()
    {
        GameEvents.OnPlayerAttack -= HandleAttackAim;
    }

    // Update is called once per frame
    // Implementar aca inputs
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveInput = new Vector2(moveX, moveY).normalized;

        // Chequeo para animación de caminar
        isWalking = moveInput.magnitude > 0.1f;

        // --- Lógica de Estado (Atacando vs. Moviendo) ---
        if (isAttacking)
        {
            // 1. SI ESTÁ ATACANDO
            attackLookTimer -= Time.deltaTime;
            if (attackLookTimer <= 0)
            {
                isAttacking = false;
            }
            // (La orientación se maneja en HandleAttackAim y se mantiene)
        }
        else
        {
            // 2. SI NO ESTÁ ATACANDO (caminando o quieto)
            // Mirar en la dirección del movimiento (solo L/R)
            if (isWalking)
            {
                UpdateSpriteFlip(moveInput);
            }

            animator.SetBool("IsWalking", isWalking);
        }
    }

    // Implementar aca fisicas
    private void FixedUpdate()
    {
        float currentSpeed = moveSpeed;
        if (isAttacking)
        {
            currentSpeed = moveSpeed * attackSlowdownMultiplier;
        }

        rb.velocity = new Vector2(moveInput.x * currentSpeed, moveInput.y * currentSpeed);
    }

    // Se llama cada vez que PlayerAbilities lanza un ataque
    private void HandleAttackAim()
    {
        isAttacking = true;
        attackLookTimer = attackLookDuration; // Setea el timer

        // 1. Obtener dirección del mouse
        Vector2 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - rb.position;

        // 2. Orientar el sprite
        UpdateSpriteFlip(lookDir);

        // 3. Animacion de ataque
        // NOTE: Deberia hacerse en un Animator
        StartCoroutine(AttackSquashStretch());
    }

    private void UpdateSpriteFlip(Vector2 direction)
    {
        if (direction.x > 0.01f)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x < -0.01f)
        {
            spriteRenderer.flipX = false;
        }
    }

    // --- Animaciones "Falsas" (Coroutines) ---

    // "Estirarse" al atacar
    private IEnumerator AttackSquashStretch()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 stretchScale = new Vector3(originalScale.x * 1.1f, originalScale.y * 0.9f, originalScale.z);
        Vector3 squashScale = new Vector3(originalScale.x * 0.9f, originalScale.y * 1.1f, originalScale.z);

        float duration = 0.1f;

        // Squash (anticipación)
        transform.localScale = squashScale;
        yield return new WaitForSeconds(duration);

        // Stretch (el golpe)
        transform.localScale = stretchScale;
        yield return new WaitForSeconds(duration);

        // Volver a la normalidad
        transform.localScale = originalScale;
    }

    private IEnumerator DamageFlash()
    {
        // 1. Cambiar al color de daño
        spriteRenderer.color = damageColor;

        // 2. Esperar un instante
        yield return new WaitForSeconds(flashDuration);

        // 3. Volver al color normal
        spriteRenderer.color = originalColor;
    }

    // --- Implementacion del IDamageable ---
    public void TakeDamage(int damageAmount)
    {
        // Logica especifica del Player
        Debug.Log("Player took damage");

        // Notificar la nueva vida a los suscriptores
        GameEvents.ReportPlayerHealthChanged(healthComponent.GetCurrentHealth(), healthComponent.GetMaxHealth());

        if (hurtSound != null) AudioManager.Instance.PlaySoundEffect(hurtSound);

        StartCoroutine(DamageFlash());
    }

    public void Die()
    {
        // Logica especifica de muerte del Player
        Debug.Log("Player has died");
        GameEvents.ReportPlayerDied();

        if (deathSound != null) AudioManager.Instance.PlaySoundEffect(deathSound);
        
        Destroy(gameObject);
    }
}
