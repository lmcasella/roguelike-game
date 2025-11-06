using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RewardScreen : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject rewardPanel; // Panel que se muestra/oculta
    [SerializeField] private PlayerStats playerStats; // Prefab de Player

    [Header("Logica de Recompensa")]
    [SerializeField] private List<AbilityUpgrade> posibleUpgrades; // Todas las mejoras

    // Cards para seleccionar la mejora
    [SerializeField] private UI_UpgradeCard card1;
    [SerializeField] private UI_UpgradeCard card2;
    [SerializeField] private UI_UpgradeCard card3;

    // Lista de las 3 opciones de mejora que luego se aplican a los botones individualmente
    private List<AbilityUpgrade> displayedUpgrades = new List<AbilityUpgrade>();

    private void OnEnable()
    {
        // Suscribirse al evento
        GameEvents.OnRoomCleared += ShowRewardOptions;
    }

    private void OnDisable()
    {
        // Desuscribirse del evento
        GameEvents.OnRoomCleared -= ShowRewardOptions;
    }

    // Start is called before the first frame update
    void Start()
    {
        rewardPanel.SetActive(false);

        // Buscar al PlayerStats si no esta asignado
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<PlayerStats>();
        }
    }

    // Mostrar panel de recompensas
    private void ShowRewardOptions()
    {
        Time.timeScale = 0f; // Pausar juego
        rewardPanel.SetActive(true); // Mostrar panel

        displayedUpgrades.Clear();

        //FIXME: Seleccion aleatoria de mejoras. Deberia haber una logica para que nunca se repitan?
        //NOTE: Agregar a la pantalla de mejoras una mejora de la lista disponible, que esté entre la posicion 0 y el total de la lista
        displayedUpgrades.Add(posibleUpgrades[Random.Range(0, posibleUpgrades.Count)]);
        displayedUpgrades.Add(posibleUpgrades[Random.Range(0, posibleUpgrades.Count)]);
        displayedUpgrades.Add(posibleUpgrades[Random.Range(0, posibleUpgrades.Count)]);

        // Configurar las cards de la UI con las mejoras asignadas antes
        card1.Setup(posibleUpgrades[0], this);
        card2.Setup(posibleUpgrades[1], this);
        card3.Setup(posibleUpgrades[2], this);
    }

    // Funcion que se ejecuta al tocar alguna de las cards de mejoras
    public void OnRewardChosen(AbilityUpgrade chosenUpgrade)
    {
        // Aplicar mejora
        playerStats.ApplyUpgrade(chosenUpgrade);

        // Ocultar panel de recompensas
        rewardPanel.SetActive(true);

        // Reanudar juego
        Time.timeScale = 1f;
    }
}
