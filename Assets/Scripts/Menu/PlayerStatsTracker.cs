using UnityEngine;

public class PlayerStatsTracker : MonoBehaviour
{
    public static PlayerStatsTracker Instance;

    void Awake()
    {
        Instance = this;
    }

    // ====== GHI NHẬN HÀNH ĐỘNG NGƯỜI CHƠI ======

    public void AddKill()
    {
        int kills = PlayerPrefs.GetInt("TotalKills", 0) + 1;
        PlayerPrefs.SetInt("TotalKills", kills);
        CheckAchievements();
    }

    public void AddGold(int amount)
    {
        int totalGold = PlayerPrefs.GetInt("TotalGoldEarned", 0) + amount;
        PlayerPrefs.SetInt("TotalGoldEarned", totalGold);
        CheckAchievements();
    }

    public void CompleteLevel(float timeTaken)
    {
        int levelsWon = PlayerPrefs.GetInt("LevelsWon", 0) + 1;
        PlayerPrefs.SetInt("LevelsWon", levelsWon);

        float bestTime = PlayerPrefs.GetFloat("BestLevelTime", 999999f);
        if (timeTaken < bestTime)
            PlayerPrefs.SetFloat("BestLevelTime", timeTaken);

        CheckAchievements();
    }

    public void OnUpgradeMaxed()
    {
        int maxedUpgrades = PlayerPrefs.GetInt("MaxedUpgrades", 0) + 1;
        PlayerPrefs.SetInt("MaxedUpgrades", maxedUpgrades);
        CheckAchievements();
    }

    // ====== KIỂM TRA ĐIỀU KIỆN MỞ KHOÁ ======

    void CheckAchievements()
    {
        AchievementManager am = FindObjectOfType<AchievementManager>();
        if (am == null) return;

        if (PlayerPrefs.GetInt("LevelsWon", 0) >= 1)
            am.UnlockAchievement("FirstWin");

        if (PlayerPrefs.GetInt("TotalKills", 0) >= 100)
            am.UnlockAchievement("Killer100");

        if (PlayerPrefs.GetInt("TotalGoldEarned", 0) >= 10000)
            am.UnlockAchievement("RichMan");

        if (PlayerPrefs.GetFloat("BestLevelTime", 999999f) <= 300f) // 5 phút = 300 giây
            am.UnlockAchievement("SpeedRun");

        if (PlayerPrefs.GetInt("MaxedUpgrades", 0) >= 1)
            am.UnlockAchievement("MaxUpgrade");
    }
}