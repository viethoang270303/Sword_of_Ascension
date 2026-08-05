using UnityEngine;
using UnityEngine.SceneManagement; // Bắt buộc phải có thư viện này để chuyển màn hình

public class GameManager : MonoBehaviour
{
    // Tạo cầu nối (Singleton) để file DemonBoss có thể gọi GameManager dễ dàng
    public static GameManager instance;

    [Header("--- Giao diện UI ---")]
    public GameObject victoryPanel; // Kéo thả VictoryPanel vào đây

    [Header("--- Cài đặt Tên màn hình ---")]
    // Đã điền chuẩn xác tên file "Main Menu" của bạn
    public string mainMenuSceneName = "Main Menu";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Hàm này được file DemonBoss gọi đến khi máu Boss = 0
    public void ShowVictoryScreen()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true); // Hiện bảng Victory
            Time.timeScale = 0f;          // Đóng băng toàn bộ thời gian trong game
        }
    }

    // --- HÀM NÀY DÙNG ĐỂ GẮN VÀO NÚT BẤM (BUTTON) ---
    public void GoToMainMenu()
    {
        // Cực kỳ quan trọng: Phải rã đông thời gian (trả về 1) trước khi qua màn mới
        Time.timeScale = 1f;

        // Load về màn hình chính
        SceneManager.LoadScene(mainMenuSceneName);
    }
}