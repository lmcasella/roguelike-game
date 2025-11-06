using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_UpgradeCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button button;

    private AbilityUpgrade myUpgrade;
    private UI_RewardScreen rewardManager;

    private void Awake()
    {
        button.onClick.AddListener(OnCardClicked);
    }

    // Configurar card de mejora
    public void Setup(AbilityUpgrade upgrade, UI_RewardScreen manager)
    {
        myUpgrade = upgrade;
        rewardManager = manager;
        descriptionText.text = upgrade.description;
    }

    // Cuando se hace click en la carta
    public void OnCardClicked()
    {
        rewardManager.OnRewardChosen(myUpgrade);
    }
}
