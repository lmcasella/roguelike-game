using UnityEngine;

public class UI_InventoryToggle : MonoBehaviour
{
    [SerializeField] private GameObject statsPanel;
    [SerializeField] private PlayerAbilities playerAbilities;
    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        // Nos aseguramos de que empiece apagado
        statsPanel.SetActive(false);

        // Buena práctica: Auto-asignación de referencias
        if (playerAbilities == null || playerStats == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerAbilities = player.GetComponent<PlayerAbilities>();
                playerStats = player.GetComponent<PlayerStats>();
            }
            else
            {
                Debug.LogWarning("UI_InventoryToggle: No se encontró un objeto con el Tag 'Player'.");
            }
        }
    }

    private void Update()
    {
        // Teclas
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.I))
        {
            TogglePanel();
        }
    }

    private void TogglePanel()
    {
        bool isActive = statsPanel.activeSelf;

        // Invertimos el estado (si estaba apagado, lo prende, y viceversa)
        statsPanel.SetActive(!isActive);

        if (!isActive) // Si el panel se está ABRIENDO
        {
            // Pausamos el juego mediante tu GameManager
            GameManager.Instance.PauseGame();

            // Forzamos la carga de datos
            if (playerAbilities != null && playerStats != null)
            {
                GameEvents.ReportStatsChanged(playerAbilities, playerStats);
            }
        }
        else // Si el panel se está CERRANDO
        {
            // Reanudamos el juego
            GameManager.Instance.ResumeGame();
        }
    }
}