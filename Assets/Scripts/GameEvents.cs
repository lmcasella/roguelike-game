using System;

public static class GameEvents
{
    // --- Eventos de Player ---
    // Este evento enviará un int (nuevo valor de vida)
    public static event Action<int, int> OnPlayerHealthChanged;
    // Metodo que otros scripts van a llamar para triggerear el evento
    // Checkea con '?' si OnPlayerHealthChanged es nulo, o sea si nadie está escuchando
    public static void ReportPlayerHealthChanged(int currentHealth, int maxHealth) => OnPlayerHealthChanged?.Invoke(currentHealth, maxHealth);

    public static event Action OnPlayerDied;
    public static void ReportPlayerDied() => OnPlayerDied?.Invoke();

    // --- Eventos de Room y game ---
    public static event Action OnRoomCleared;
    public static void ReportRoomCleared() => OnRoomCleared?.Invoke();

    public static event Action<int> OnEnemyKilled;
    public static void ReportEnemyKilled(int scoreValue) => OnEnemyKilled?.Invoke(scoreValue);
}
