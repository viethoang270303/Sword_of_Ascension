using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class UpgradeItem
{
    public string upgradeKey;
    public string displayName;
    [TextArea(2, 3)]
    public string description;
    public Sprite icon;
    public int currentLevel;
    public int maxLevel = 10;
    public int baseCost = 100;
    public int costIncreasePerLevel = 50;

    [Header("Bonus Info")]
    public float bonusPerLevel = 10f;
    public string bonusUnit = "%";

    [Header("UI References")]
    public Image iconImage;
    public Button iconButton;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI levelText;
    public Button upgradeButton;
    public TextMeshProUGUI buttonText;

    public int GetCurrentCost()
    {
        return baseCost + (currentLevel * costIncreasePerLevel);
    }

    public float GetCurrentBonus()
    {
        return currentLevel * bonusPerLevel;
    }

    public float GetNextBonus()
    {
        return (currentLevel + 1) * bonusPerLevel;
    }
}

public class UpgradeManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public UpgradeItem[] upgrades;

    [Header("Tooltip")]
    public GameObject tooltipPanel;
    public Image tooltipIcon;
    public TextMeshProUGUI tooltipNameText;
    public TextMeshProUGUI tooltipStatsText;
    public Button closeTooltipButton;

    private int gold;

    void Start()
    {
        gold = PlayerPrefs.GetInt("PlayerGold", 500);

        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);

        if (closeTooltipButton != null)
            closeTooltipButton.onClick.AddListener(HideTooltip);

        foreach (var upgrade in upgrades)
        {
            upgrade.currentLevel = PlayerPrefs.GetInt("Upgrade_" + upgrade.upgradeKey, 0);
            upgrade.upgradeButton.onClick.AddListener(() => TryUpgrade(upgrade));

            if (upgrade.iconButton != null)
                upgrade.iconButton.onClick.AddListener(() => ShowTooltip(upgrade));

            if (upgrade.iconImage != null && upgrade.icon != null)
                upgrade.iconImage.sprite = upgrade.icon;

            if (upgrade.nameText != null)
                upgrade.nameText.text = upgrade.displayName;

            if (upgrade.descriptionText != null)
                upgrade.descriptionText.text = upgrade.description;
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

            // Thêm dòng này: kiểm tra nếu vừa đạt Max Level
            if (upgrade.currentLevel >= upgrade.maxLevel)
            {
                PlayerStatsTracker.Instance.OnUpgradeMaxed();
            }

            RefreshUI();
        }
    }

    void ShowTooltip(UpgradeItem upgrade)
    {
        if (tooltipPanel == null) return;

        tooltipPanel.SetActive(true);

        if (tooltipIcon != null && upgrade.icon != null)
            tooltipIcon.sprite = upgrade.icon;

        if (tooltipNameText != null)
            tooltipNameText.text = upgrade.displayName;

        if (tooltipStatsText != null)
        {
            string content = $"{upgrade.description}\n\n" +
                              $"Hiện tại: +{upgrade.GetCurrentBonus()}{upgrade.bonusUnit}\n";

            if (upgrade.currentLevel < upgrade.maxLevel)
                content += $"Cấp tiếp theo: +{upgrade.GetNextBonus()}{upgrade.bonusUnit}";
            else
                content += "Đã đạt cấp tối đa!";

            tooltipStatsText.text = content;
        }
    }

    void HideTooltip()
    {
        if (tooltipPanel != null)
            tooltipPanel.SetActive(false);
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