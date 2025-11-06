using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject optionsPanel;

    [SerializeField] private string firstLevelSceneName = "Level_1";

    public void OnStartButtonPressed()
    {
        Debug.Log("Starting game...");
        GameManager.Instance.LoadScene(firstLevelSceneName);
    }

    public void OnOptionsButtonPressed()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        mainPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void OnQuitButtonPressed()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
