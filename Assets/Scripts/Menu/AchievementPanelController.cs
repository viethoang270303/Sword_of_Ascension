using UnityEngine;

public class AchievementPanelController : MonoBehaviour
{
    public GameObject achievementPanel;

    public void OpenAchievement()
    {
        achievementPanel.SetActive(true);
    }

    public void CloseAchievement()
    {
        achievementPanel.SetActive(false);
    }
}