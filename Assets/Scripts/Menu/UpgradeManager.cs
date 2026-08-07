using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class UpgradeItem
{
    public string upgradeKey;
    public string displayName;
    public int currentLevel;
    public int maxLevel = 10;
    public int baseCost = 100;
    public int costIncreasePerLevel = 50;

    [Header("UI References")]
    public TextMeshProUGUI levelText;
    public Button upgradeButton;
    public TextMeshProUGUI buttonText;

    public int GetCurrentCost()
    {
        return baseCost + (currentLevel * costIncreasePerLevel);
    }
}

public class UpgradeManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public UpgradeItem[] upgrades;

    private int gold;

    void Start()
    {
        gold = PlayerPrefs.GetInt("PlayerGold", 500);

        foreach (var upgrade in upgrades)
        {
            upgrade.currentLevel = PlayerPrefs.GetInt("Upgrade_" + upgrade.upgradeKey, 0);
            upgrade.upgradeButton.onClick.AddListener(() => TryUpgrade(upgrade));
        }

        RefreshUI();
    }

    void TryUpgrade(UpgradeItem upgrade)
    {
        if (upgrade.currentLevel >= upgrade.maxLevel) return;

        int cost = upgrade.GetCurrentCost();

        if (gold >= cost)
        {
            gold -= cost;
            upgrade.currentLevel++;

            PlayerPrefs.SetInt("PlayerGold", gold);
            PlayerPrefs.SetInt("Upgrade_" + upgrade.upgradeKey, upgrade.currentLevel);

            RefreshUI();
        }
    }

    void RefreshUI()
    {
        goldText.text = "Vàng: " + gold;

        foreach (var upgrade in upgrades)
        {
            bool isMaxLevel = upgrade.currentLevel >= upgrade.maxLevel;
            upgrade.levelText.text = "Lv " + upgrade.currentLevel + "/" + upgrade.maxLevel;

            if (isMaxLevel)
            {
                upgrade.buttonText.text = "MAX";
                upgrade.upgradeButton.interactable = false;
            }
            else
            {
                upgrade.buttonText.text = upgrade.GetCurrentCost() + " Vàng";
                upgrade.upgradeButton.interactable = gold >= upgrade.GetCurrentCost();
            }
        }
    }
}