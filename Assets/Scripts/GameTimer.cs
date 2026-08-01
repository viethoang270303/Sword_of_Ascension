using UnityEngine;
using TMPro; // Đổi sang thư viện TextMeshPro

public class GameTimer : MonoBehaviour
{
    // Đổi biến Text thành TextMeshProUGUI
    public TextMeshProUGUI timerText;

    void Update()
    {
        float t = Time.timeSinceLevelLoad;
        int min = Mathf.FloorToInt(t / 60);
        int sec = Mathf.FloorToInt(t % 60);

        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", min, sec);
        }
    }
}