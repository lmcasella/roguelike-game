using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Para reiniciar escena

public class GameUI : MonoBehaviour
{
    [Header("Paneles")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    [Header("Referencias")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        // Asegurarse de que los paneles estén desactivados al inicio
        if (pausePanel) pausePanel.SetActive(false);
        if (gameOverPanel) gameOverPanel.SetActive(false);
        if (victoryPanel) victoryPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    void OnEnable()
    {
        GameEvents.OnPlayerDied += ShowGameOver;
        GameEvents.OnBossDied += ShowVictory;
    }

    void OnDisable()
    {
        GameEvents.OnPlayerDied -= ShowGameOver;
        GameEvents.OnBossDied -= ShowVictory;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pausePanel.SetActive(true);
            GameManager.Instance.PauseGame();
        }
        else
        {
            pausePanel.SetActive(false);
            GameManager.Instance.ResumeGame();
        }
    }

    private void ShowGameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowVictory()
    {
        isGameOver = true;
        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // --- Funciones para los Botones ---

    public void RestartLevel()
    {
        // Reinicia la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
