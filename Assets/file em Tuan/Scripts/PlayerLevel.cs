using UnityEngine;
using UnityEngine.UI; // Cần dòng này để điều khiển UI

public class PlayerLevel : MonoBehaviour
{
    [Header("Thông số Cấp độ")]
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    [Header("Giao diện (UI)")]
    public Slider expBar; // Biến này để chứa thanh UI EXP

    void Start()
    {
        // Khi bắt đầu game, cài đặt thanh Exp
        if (expBar != null)
        {
            expBar.maxValue = expToNextLevel;
            expBar.value = currentExp;
        }
    }

    public void AddExp(int amount)
    {
        currentExp += amount;

        // Cập nhật thanh UI mỗi khi ăn Exp
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
        currentExp -= expToNextLevel; // Giữ lại số Exp bị thừa
        expToNextLevel += 50;         // Tăng mốc Exp của cấp tiếp theo

        // Cập nhật lại thanh UI sau khi lên cấp
        if (expBar != null)
        {
            expBar.maxValue = expToNextLevel; // Kéo dài ống Exp ra
            expBar.value = currentExp;        // Đổ phần Exp thừa vào ống mới
        }

        Debug.Log("CHÚC MỪNG! LÊN CẤP: " + currentLevel);
    }
}