using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RewardScreen : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject rewardPanel; // Panel que se muestra/oculta
    [SerializeField] private GameObject playerObject; // Prefab de Player

    [Header("Logica de Recompensa")]
    [SerializeField] private List<UpgradeEffect> posibleUpgrades; // Todas las mejoras

    // Cards para seleccionar la mejora
    [SerializeField] private UI_UpgradeCard card1;
    [SerializeField] private UI_UpgradeCard card2;
    [SerializeField] private UI_UpgradeCard card3;

    // Lista de las 3 opciones de mejora que luego se aplican a los botones individualmente
    private List<UpgradeEffect> displayedUpgrades = new List<UpgradeEffect>();

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
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Mostrar panel de recompensas
    private void ShowRewardOptions()
    {
        Time.timeScale = 0f; // Pausar juego
        rewardPanel.SetActive(true); // Mostrar panel

        Debug.Log("Se muestra el panel");

        displayedUpgrades.Clear();

        // Lógica simple para elegir 3 al azar
        for (int i = 0; i < 3; i++)
        {
            if (posibleUpgrades.Count > 0)
            {
                displayedUpgrades.Add(posibleUpgrades[Random.Range(0, posibleUpgrades.Count)]);
            }
        }

        // Setup de las cartas (verificando que existan)
        if (displayedUpgrades.Count > 0) card1.Setup(displayedUpgrades[0], this);
        if (displayedUpgrades.Count > 1) card2.Setup(displayedUpgrades[1], this);
        if (displayedUpgrades.Count > 2) card3.Setup(displayedUpgrades[2], this);
    }

    // Funcion que se ejecuta al tocar alguna de las cards de mejoras
    public void OnRewardChosen(UpgradeEffect chosenUpgrade)
    {
        if (playerObject != null)
        {
            chosenUpgrade.Apply(playerObject);
        }

        rewardPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
