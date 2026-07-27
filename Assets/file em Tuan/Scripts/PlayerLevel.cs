using UnityEngine;
using UnityEngine.UI;

public class PlayerLevel : MonoBehaviour
{
    [Header("Thông số Cấp độ")]
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    [Header("Chỉ số Sức mạnh")]
    public int playerDamage = 1; // Mặc định ban đầu bắn mất 1 máu

    [Header("Giao diện (UI)")]
    public Slider expBar;

    void Start()
    {
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
        expToNextLevel += 50;

        // --- TĂNG SÁT THƯƠNG MỖI KHI LÊN CẤP ---
        playerDamage += 1;
        // ---------------------------------------

        if (expBar != null)
        {
            expBar.maxValue = expToNextLevel;
            expBar.value = currentExp;
        }

        Debug.Log("CHÚC MỪNG! LÊN CẤP: " + currentLevel + " | Sát thương hiện tại: " + playerDamage);
    }
}