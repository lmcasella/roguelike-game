using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RewardScreen : MonoBehaviour
{
    [Header("Animación")]
    [Tooltip("Arrastrar el propio RewardPanel que ahora tiene el CanvasGroup")]
    [SerializeField] private CanvasGroup rewardCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

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

        // Clonamos la lista asegurándonos de ignorar espacios vacíos (null)
        List<UpgradeEffect> pool = new List<UpgradeEffect>();
        foreach (var upgrade in posibleUpgrades)
        {
            if (upgrade != null)
            {
                pool.Add(upgrade);
            }
        }

        // Nos aseguramos de no intentar sacar mas cartas de las que existen
        int cardsToDraw = Mathf.Min(3, pool.Count);

        // Lógica simple para elegir 3 al azar
        for (int i = 0; i < cardsToDraw; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            displayedUpgrades.Add(pool[randomIndex]);

            // Lo removemos del pool temporal para que no vuelva a salir
            pool.RemoveAt(randomIndex);
        }

        // Setup de las cartas, y ocultar las que no se usen si hay menos de 3
        card1.gameObject.SetActive(false);
        card2.gameObject.SetActive(false);
        card3.gameObject.SetActive(false);

        if (displayedUpgrades.Count > 0)
        {
            card1.gameObject.SetActive(true);
            card1.Setup(displayedUpgrades[0], this);
        }
        if (displayedUpgrades.Count > 1)
        {
            card2.gameObject.SetActive(true);
            card2.Setup(displayedUpgrades[1], this);
        }
        if (displayedUpgrades.Count > 2)
        {
            card3.gameObject.SetActive(true);
            card3.Setup(displayedUpgrades[2], this);
        }

        // Animacion Fade-In
        if (rewardCanvasGroup != null)
        {
            StartCoroutine(FadeInPanel());
        }
    }

    private IEnumerator FadeInPanel()
    {
        // Bloqueamos clics desde el inicio y ponemos la opacidad en 0
        rewardCanvasGroup.interactable = false;
        rewardCanvasGroup.alpha = 0f;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            // Usamos unscaledDeltaTime porque el timeScale está en 0
            timer += Time.unscaledDeltaTime;

            // Interpolar (Lerp) de 0 a 1 según el progreso del tiempo
            rewardCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);

            yield return null;
        }

        // Aseguramos que termine exactamente en 1 y habilitamos la interacción
        rewardCanvasGroup.alpha = 1f;
        rewardCanvasGroup.interactable = true;
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
