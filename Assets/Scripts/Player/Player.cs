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
    private float currentSpeedMultiplier = 1f;
    [Tooltip("Multiplicador de velocidad mientras ataca (ej: 0.5 = 50% más lento)")]
    [SerializeField] private float attackSlowdownMultiplier = 0.5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [Header("Aiming & Animation")]
    [SerializeField] private float attackLookDuration = 0.4f; // Cuánto tiempo mira al mouse

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private bool isInvincibleDuringDash = true;

    [Header("Dash UI")]
    [SerializeField] private Ability dashAbilityData;

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

    private bool canDash = true;
    private bool isDashing = false;

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

    private void Start()
    {
        originalColor = spriteRenderer.color;

        if (GameManager.Instance.LoadPlayerState(
            out int hp,
            out int maxHp,
            out int mp,
            out int maxMp,
            out int dmgBonus,
            out int extraProj))
        {
            // 1. Aplicar Vida
            // Tener SetMaxHealth en SystemHealth
            healthComponent.SetMaxHealth(maxHp);
            healthComponent.SetHealth(hp);

            // 2. Aplicar Maná
            var manaComp = GetComponent<PlayerMana>();
            manaComp.SetMaxMana(maxMp);
            manaComp.SetMana(mp);       

            // 3. Aplicar Stats de Ataque
            var statsComp = GetComponent<PlayerStats>();
            statsComp.basicDamageBonus = dmgBonus;
            statsComp.basicExtraProjectiles = extraProj;

            // 4. Re-aplicar Upgrades especiales
            if (GameManager.Instance.hasVampirePerk)
            {
                gameObject.AddComponent<VampireBehaviour>();
            }

            Debug.Log("Player Stats loaded from GameManager");
        }

        if (dashAbilityData != null)
        {
            GameEvents.ReportAbilityEquipped(AbilitySlot.Dash, dashAbilityData);
        }
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

        // --- Lógica de Estado ---
        if (isAttacking)
        {
            // 1. SI ESTÁ ATACANDO
            attackLookTimer -= Time.deltaTime;
            if (attackLookTimer <= 0)
            {
                isAttacking = false;
            }
        }
        else
        {
            // 2. SI NO ESTÁ ATACANDO (caminando o quieto)
            if (isWalking)
            {
                UpdateSpriteFlip(moveInput);
            }

            animator.SetBool("IsWalking", isWalking);
        }

        // Input de Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && moveInput != Vector2.zero)
        {
            StartCoroutine(DashRoutine());
        }
    }

    // Implementar aca fisicas
    private void FixedUpdate()
    {
        if (isDashing) return;

        float finalSpeed = moveSpeed * currentSpeedMultiplier;

        if (isAttacking)
        {
            finalSpeed *= attackSlowdownMultiplier;
        }

        rb.velocity = moveInput * finalSpeed;
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
        //StartCoroutine(AttackSquashStretch());
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

    // --- Corrutina del Dash ---
    private IEnumerator DashRoutine()
    {
        isDashing = true;
        canDash = false;

        if (dashAbilityData != null)
        {
            GameEvents.ReportAbilityCooldownStarted(AbilitySlot.Dash, dashCooldown);
        }

        // 1. Guardar velocidad del dash
        rb.velocity = moveInput.normalized * dashSpeed;

        // 2. Invulnerabilidad: Cambiar Layer a uno que no choque con proyectiles/enemigos
        int originalLayer = gameObject.layer;
        if (isInvincibleDuringDash) gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        // 3. Esperar duración
        yield return new WaitForSeconds(dashDuration);

        // 4. Terminar Dash
        isDashing = false;
        rb.velocity = Vector2.zero;
        if (isInvincibleDuringDash) gameObject.layer = originalLayer;

        // 5. Cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    // --- Boosts ---
    // FIXME: Conflicto en el cambio de color con el Animator
    public IEnumerator ActivateSpeedBoost(float multiplier, float duration)
    {
        currentSpeedMultiplier = multiplier;

        // Efecto visual: Tinte amarillo
        spriteRenderer.color = Color.yellow;

        yield return new WaitForSeconds(duration);

        currentSpeedMultiplier = 1f;
        spriteRenderer.color = originalColor; // Color normal
    }

    // --- Implementacion del IDamageable ---
    public void TakeDamage(int damageAmount)
    {
        // Logica especifica del Player
        Debug.Log("Player took damage");

        // Notificar la nueva vida a los suscriptores
        GameEvents.ReportPlayerHealthChanged(healthComponent.GetCurrentHealth(), healthComponent.GetMaxHealth());

        if (hurtSound != null) AudioManager.Instance.PlaySoundEffect(hurtSound);

        if (animator != null)
        {
            animator.SetTrigger("OnHit");
        }

        //StartCoroutine(DamageFlash());
    }

    public void Die()
    {
        // Logica de muerte del Player
        Debug.Log("Player has died");
        GameEvents.ReportPlayerDied();

        if (deathSound != null) AudioManager.Instance.PlaySoundEffect(deathSound);
        
        Destroy(gameObject);
    }
}
