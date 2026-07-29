using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [Header("Thông số Cấp độ")]
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    [Header("Chỉ số Sức mạnh")]
    public int playerDamage = 1;

    [Header("Giao diện (UI)")]
    public Slider expBar;

    private SkillManager skillManager;

    void Start()
    {
        // Tự động tìm SkillManager trong màn chơi
        skillManager = FindFirstObjectByType<SkillManager>();

        if (expBar != null)
        {
            expBar.maxValue = expToNextLevel;
            expBar.value = currentExp;
        }
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        if (expBar != null)
        {
            expBar.value = currentExp;
        }

        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevel++;
        currentExp -= expToNextLevel;
        expToNextLevel += 50; // Cấp sau cần nhiều EXP hơn

        if (expBar != null)
        {
            expBar.maxValue = expToNextLevel;
            expBar.value = currentExp;
        }

        Debug.Log("CHÚC MỪNG! LÊN CẤP: " + currentLevel);

        // --- GỌI BẢNG CHỌN KỸ NĂNG HIỆN LÊN ---
        if (skillManager != null)
        {
            skillManager.ShowLevelUpPanel();
        }
        else
        {
            Debug.LogWarning("Chưa tìm thấy SkillManager trong Scene! Hãy kiểm tra lại!");
        }
        // --------------------------------------
    }
}