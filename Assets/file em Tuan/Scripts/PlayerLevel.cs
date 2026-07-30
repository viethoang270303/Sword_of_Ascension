using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    public int playerDamage = 1;
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;
    public Slider expBar;

    private SkillManager skillManager;

    void Start()
    {
        // Đã sửa lệnh cũ thành lệnh mới FindFirstObjectByType để fix cảnh báo vàng
        skillManager = Object.FindFirstObjectByType<SkillManager>();
        UpdateUI();
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        if (currentExp >= expToNextLevel) LevelUp();
        UpdateUI();
    }

    void LevelUp()
    {
        currentLevel++;
        currentExp -= expToNextLevel;
        expToNextLevel += 50;

        if (skillManager != null) skillManager.ShowLevelUpUI();
    }

    void UpdateUI()
    {
        if (expBar != null)
        {
            expBar.maxValue = expToNextLevel;
            expBar.value = currentExp;
        }
    }
}