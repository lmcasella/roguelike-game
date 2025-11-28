using System;

public static class GameEvents
{
    // --- Eventos de Player ---
    // Este evento enviará un int (nuevo valor de vida)
    public static event Action<int, int> OnPlayerHealthChanged;
    // Metodo que otros scripts van a llamar para triggerear el evento
    // Checkea con '?' si OnPlayerHealthChanged es nulo, o sea si nadie está escuchando
    public static void ReportPlayerHealthChanged(int currentHealth, int maxHealth) => OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);

    public static event Action<int, int> OnPlayerManaChanged;
    public static void ReportPlayerManaChanged(int currentMana, int maxMana) => OnPlayerManaChanged?.Invoke(currentMana, maxMana);

    // Cuando el jugador ataca
    public static event Action OnPlayerAttack;
    public static void ReportPlayerAttack() => OnPlayerAttack?.Invoke();

    // Cuando el jugador muere
    public static event Action OnPlayerDied;
    public static void ReportPlayerDied() => OnPlayerDied?.Invoke();

    // --- Eventos de Room y Game ---
    public static event Action OnRoomCleared;
    public static void ReportRoomCleared() => OnRoomCleared?.Invoke();

    public static event Action<Enemy, int> OnEnemyDied;
    public static void ReportEnemyDied(Enemy enemy, int scoreValue) => OnEnemyDied?.Invoke(enemy, scoreValue);

    // --- Eventos de Habilidades ---
    // Avisa que cambió la habilidad en un slot (para poner el icono nuevo)
    public static event Action<AbilitySlot, Ability> OnAbilityEquipped;
    public static void ReportAbilityEquipped(AbilitySlot slot, Ability ability) => OnAbilityEquipped?.Invoke(slot, ability);

    // Avisa que se usó una habilidad y empezó su cooldown
    public static event Action<AbilitySlot, float> OnAbilityCooldownStarted;
    public static void ReportAbilityCooldownStarted(AbilitySlot slot, float duration) => OnAbilityCooldownStarted?.Invoke(slot, duration);

    public static event Action<BuffEffect, float> OnBuffApplied;
    public static void ReportBuffApplied(BuffEffect buff, float duration) => OnBuffApplied?.Invoke(buff, duration);

    // --- Evento de Victoria ---
    public static event Action OnBossDied;
    public static void ReportBossDied() => OnBossDied?.Invoke();
}
