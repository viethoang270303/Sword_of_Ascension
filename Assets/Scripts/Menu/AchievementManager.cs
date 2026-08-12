using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class AchievementItem
{
    public string achievementKey;   // key duy nhất để lưu, ví dụ "FirstWin"
    public string displayName;
    [TextArea(2, 2)]
    public string description;
    public bool isUnlocked;

    [Header("UI References")]
    public TextMeshProUGUI statusText;
    public Image rowBackground;
}

public class AchievementManager : MonoBehaviour
{
    public AchievementItem[] achievements;

    [Header("Colors")]
    public Color unlockedColor = new Color(0.2f, 0.6f, 0.2f, 0.6f);
    public Color lockedColor = new Color(0.15f, 0.15f, 0.15f, 0.6f);

    void Start()
    {
        foreach (var ach in achievements)
        {
            ach.isUnlocked = PlayerPrefs.GetInt("Achievement_" + ach.achievementKey, 0) == 1;
        }

        RefreshUI();
    }

    public void UnlockAchievement(string key)
    {
        foreach (var ach in achievements)
        {
            if (ach.achievementKey == key && !ach.isUnlocked)
            {
                ach.isUnlocked = true;
                PlayerPrefs.SetInt("Achievement_" + key, 1);
                RefreshUI();
                Debug.Log("Đã mở khoá thành tích: " + ach.displayName);
            }
        }
    }

    void RefreshUI()
    {
        foreach (var ach in achievements)
        {
            if (ach.statusText != null)
                ach.statusText.text = ach.isUnlocked ? "Đã đạt" : "Chưa đạt";

            if (ach.rowBackground != null)
                ach.rowBackground.color = ach.isUnlocked ? unlockedColor : lockedColor;
        }
    }
}