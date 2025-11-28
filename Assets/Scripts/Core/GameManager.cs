using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { MainMenu, Playing, Paused, GameOver };
    public GameState currentState;

    // Variables para persistencia
    private bool hasSavedData = false;

    private int savedCurrentHealth;
    private int savedMaxHealth;

    private int savedCurrentMana;
    private int savedMaxMana;

    // Stats de PlayerStats
    private int savedDamageBonus;
    private int savedExtraProjectiles;

    public bool hasVampirePerk = false;

    protected override void Awake()
    {
        // Esto ejecuta la logica del Singleton
        base.Awake();

        // No eliminar GameManager al cargar una nueva escena
        DontDestroyOnLoad(this.gameObject);

        currentState = GameState.MainMenu;

        Debug.Log("GameManager Initialized");
    }

    // Llamar a esto justo antes de cambiar de escena (desde LevelExit)
    public void SavePlayerState(int currentHp, int maxHp, int currentMp, int maxMp, PlayerStats stats)
    {
        savedCurrentHealth = currentHp;
        savedMaxHealth = maxHp;

        savedCurrentMana = currentMp;
        savedMaxMana = maxMp;

        if (stats != null)
        {
            savedDamageBonus = stats.basicDamageBonus;
            savedExtraProjectiles = stats.basicExtraProjectiles;
        }

        // Verificar si el jugador tiene el perk de vampiro
        hasVampirePerk = (stats.GetComponent<VampireBehaviour>() != null);

        hasSavedData = true;
        Debug.Log("Progreso guardado correctamente.");
    }

    // Llamar a esto desde el Player al iniciar (Start)
    public bool LoadPlayerState(out int currentHp, out int maxHp, out int currentMp, out int maxMp, out int dmgBonus, out int extraProj)
    {
        if (hasSavedData)
        {
            currentHp = savedCurrentHealth;
            maxHp = savedMaxHealth;
            currentMp = savedCurrentMana;
            maxMp = savedMaxMana;
            dmgBonus = savedDamageBonus;
            extraProj = savedExtraProjectiles;
            return true;
        }

        // Valores por defecto si es partida nueva
        currentHp = 100; maxHp = 100;
        currentMp = 100; maxMp = 100;
        dmgBonus = 0; extraProj = 0;
        return false;
    }

    public void ResetRun()
    {
        // Llamar a esto en game over o main menu para reiniciar todo
        savedCurrentHealth = -1;
        savedCurrentMana = -1;
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        currentState = GameState.Paused;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        currentState = GameState.Playing;
    }

    public void GameOver()
    {
        Time.timeScale = 1f;
        this.ResetRun();
        currentState = GameState.GameOver;
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
