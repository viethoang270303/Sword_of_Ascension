using UnityEngine;
using TMPro; // Bắt buộc để dùng TextMeshPro

public class GameTimer : MonoBehaviour
{
    [Header("Giao diện Đồng hồ")]
    public TextMeshProUGUI timerText;

    private float timePassed; // Biến đếm số giây đã trôi qua

    void Update()
    {
        // Cộng dồn thời gian thực
        timePassed += Time.deltaTime;

        // Đổi ra số Phút và Giây
        int minutes = Mathf.FloorToInt(timePassed / 60);
        int seconds = Mathf.FloorToInt(timePassed % 60);

        // Hiển thị lên màn hình theo chuẩn 00:00
        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}