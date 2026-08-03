using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    [Header("--- Các Bảng UI ---")]
    public GameObject settingPanel;
    public GameObject tutorialPanel;

    [Header("--- Nút Tạm Dừng ---")]
    public Image pauseButtonImage;
    public Sprite pauseSprite;
    public Sprite playSprite;

    [Header("--- Hỗ trợ Tay cầm / Bàn phím ---")]
    public GameObject firstSelectedButton;

    // Biến này để lưu lại nút cuối cùng bạn vừa click hoặc vừa bôi đen
    private GameObject lastSelected;

    void Start()
    {
        if (settingPanel != null) settingPanel.SetActive(false);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
    }

    void Update()
    {
        // 1. TẮT/MỞ BẢNG SETTING (Bằng phím Esc hoặc nút Start tay cầm)
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (tutorialPanel != null && tutorialPanel.activeSelf)
            {
                CloseTutorial();
                return;
            }

            if (settingPanel != null && settingPanel.activeSelf)
                ResumeGame();
            else
                OpenSettingsAndPause();
        }

        // 2. CƠ CHẾ BẢO VỆ CHUYỂN ĐỔI CHUỘT <-> TAY CẦM
        if (settingPanel != null && settingPanel.activeSelf)
        {
            // Nếu người chơi lỡ click chuột ra ngoài không khí làm mất Focus (bằng null)
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                // Khi người chơi chạm lại vào bất kỳ phím nào trên tay cầm/bàn phím
                if (Input.anyKeyDown || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    // Tự động khôi phục lại Focus về nút cuối cùng (hoặc nút đầu tiên)
                    EventSystem.current.SetSelectedGameObject(lastSelected != null ? lastSelected : firstSelectedButton);
                }
            }
            else
            {
                // Liên tục cập nhật nút đang được trỏ vào
                lastSelected = EventSystem.current.currentSelectedGameObject;
            }
        }
    }

    // Các hàm bên dưới giữ nguyên y hệt như cũ
    public void OpenSettingsAndPause()
    {
        Time.timeScale = 0f;
        if (settingPanel != null) settingPanel.SetActive(true);
        if (pauseButtonImage != null) pauseButtonImage.sprite = playSprite;

        if (firstSelectedButton != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (settingPanel != null) settingPanel.SetActive(false);
        if (tutorialPanel != null) tutorialPanel.SetActive(false);
        if (pauseButtonImage != null) pauseButtonImage.sprite = pauseSprite;
    }

    public void ChangeVolume(float volumeValue)
    {
        AudioListener.volume = volumeValue;
    }

    public void OpenTutorial()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(true);
    }

    public void CloseTutorial()
    {
        if (tutorialPanel != null) tutorialPanel.SetActive(false);

        if (firstSelectedButton != null && settingPanel.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}