using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems; // Thư viện bắt buộc để dùng phím/tay cầm

public class PauseManager : MonoBehaviour
{
    [Header("--- Các Bảng UI ---")]
    public GameObject settingPanel;   // Kéo bảng SettingPanel vào đây
    public GameObject tutorialPanel;  // Kéo bảng TutorialPanel (Hướng dẫn) vào đây

    [Header("--- Nút Tạm Dừng (Ngoài màn hình) ---")]
    public Image pauseButtonImage;
    public Sprite pauseSprite;
    public Sprite playSprite;

    [Header("--- Hỗ trợ Tay cầm / Bàn phím ---")]
    public GameObject firstSelectedButton; // Kéo nút "Tiếp Tục" vào đây để mặc định chọn khi mở bảng

    void Start()
    {
        // Đảm bảo các bảng luôn ẩn khi mới vào game
        if (settingPanel != null) settingPanel.SetActive(false);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
    }

    void Update()
    {
        // Bấm Esc (Bàn phím) hoặc phím Cancel/Start (Tay cầm) để Bật/Tắt Setting
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel"))
        {
            // Nếu bảng Hướng dẫn đang mở -> Ưu tiên đóng bảng Hướng dẫn trước
            if (tutorialPanel != null && tutorialPanel.activeSelf)
            {
                CloseTutorial();
                return;
            }

            // Nếu bảng Setting đang mở -> Đóng nó lại và tiếp tục game
            if (settingPanel != null && settingPanel.activeSelf)
            {
                ResumeGame();
            }
            // Nếu chưa mở gì -> Mở bảng Setting và tạm dừng
            else
            {
                OpenSettingsAndPause();
            }
        }
    }

    // --- 1. MỞ BẢNG SETTING (Tạm dừng game) ---
    public void OpenSettingsAndPause()
    {
        Time.timeScale = 0f; // Đóng băng thời gian

        if (settingPanel != null) settingPanel.SetActive(true); // Hiện bảng Setting

        // Đổi icon Tạm Dừng thành icon Play
        if (pauseButtonImage != null) pauseButtonImage.sprite = playSprite;

        // Hỗ trợ tay cầm: Tự động trỏ Focus vào nút đầu tiên (Nút Tiếp Tục)
        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    // --- 2. NÚT: TIẾP TỤC GAME ---
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Mở khóa thời gian

        if (settingPanel != null) settingPanel.SetActive(false); // Ẩn bảng Setting
        if (tutorialPanel != null) tutorialPanel.SetActive(false); // Ẩn luôn bảng Hướng dẫn (nếu đang mở)

        // Trả lại icon 2 gạch cho nút Tạm Dừng
        if (pauseButtonImage != null) pauseButtonImage.sprite = pauseSprite;
    }

    // --- 3. THANH TRƯỢT: VOLUME ---
    public void ChangeVolume(float volumeValue)
    {
        AudioListener.volume = volumeValue;
    }

    // --- 4. HƯỚNG DẪN ---
    public void OpenTutorial()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(true); // Hiện bảng hướng dẫn
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false); // Ẩn bảng hướng dẫn

        // Trả focus về lại nút "Tiếp Tục" trên SettingPanel để người chơi dùng tay cầm không bị đơ
        if (firstSelectedButton != null && settingPanel.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    // --- 5. NÚT: VỀ MENU CHÍNH ---
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // BẮT BUỘC phải mở khóa thời gian trước khi chuyển Scene

        // Đã sửa lại có dấu cách chuẩn xác theo tên file của bạn
        SceneManager.LoadScene("Main Menu");
    }
}