using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState { MainMenu, Playing, Paused, GameOver };
    public GameState currentState;

    protected override void Awake()
    {
        // Esto ejecuta la logica del Singleton
        base.Awake();

        // No eliminar GameManager al cargar una nueva escena
        DontDestroyOnLoad(this.gameObject);

        currentState = GameState.MainMenu;

        Debug.Log("GameManager Initialized");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
        currentState = GameState.GameOver;
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
