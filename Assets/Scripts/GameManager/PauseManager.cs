using UnityEngine;
using UnityEngine.UI; // Thư viện bắt buộc để can thiệp vào ảnh của nút
using UnityEngine.SceneManagement; // Thư viện để chuyển Scene (Menu)

public class PauseManager : MonoBehaviour
{
    [Header("Bảng Cài Đặt (Kéo Panel vào đây)")]
    public GameObject settingPanel;

    [Header("--- Nút Tạm Dừng ---")]
    public Image pauseButtonImage; // Kéo Image của nút Tạm Dừng vào đây
    public Sprite pauseSprite;     // Kéo ảnh 2 GẠCH vào đây
    public Sprite playSprite;      // Kéo ảnh TAM GIÁC vào đây
    private bool isPaused = false;

    [Header("--- Nút Âm Thanh ---")]
    public Image soundButtonImage; // Kéo Image của nút Cái Loa vào đây
    public Sprite soundOnSprite;   // Kéo ảnh CÁI LOA vào đây
    public Sprite soundOffSprite;  // Kéo ảnh DẤU X ĐỎ vào đây
    private bool isMuted = false;

    void Start()
    {
        // Ẩn bảng Setting (Panel) khi mới vào game
        if (settingPanel != null)
        {
            settingPanel.SetActive(false);
        }
    }

    // 1. Hàm cho nút TẠM DỪNG
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Dừng game
            if (pauseButtonImage != null) pauseButtonImage.sprite = playSprite; // Đổi sang ảnh Tam Giác
        }
        else
        {
            Time.timeScale = 1f; // Tiếp tục game
            if (pauseButtonImage != null) pauseButtonImage.sprite = pauseSprite; // Đổi lại ảnh 2 Gạch
        }
    }

    // 2. Hàm cho nút BÁNH RĂNG
    public void ToggleSettingsPanel()
    {
        if (settingPanel != null)
        {
            // Kiểm tra xem bảng đang bật hay tắt để đảo ngược lại
            bool isActive = settingPanel.activeSelf;
            settingPanel.SetActive(!isActive);
        }
    }

    // 3. Hàm cho nút CÁI LOA
    public void ToggleSound()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            AudioListener.volume = 0f; // Tắt toàn bộ tiếng
            if (soundButtonImage != null) soundButtonImage.sprite = soundOffSprite; // Đổi sang ảnh X Đỏ
        }
        else
        {
            AudioListener.volume = 1f; // Bật lại tiếng
            if (soundButtonImage != null) soundButtonImage.sprite = soundOnSprite; // Đổi lại ảnh Cái Loa
        }
    }

    // (Dự phòng) Hàm cho nút VỀ MENU nếu bạn gắn trong Setting Panel
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Bắt buộc mở khóa thời gian trước khi chuyển cảnh
        SceneManager.LoadScene("MainMenu"); // Đổi "MainMenu" thành tên file Scene menu của bạn
    }
}