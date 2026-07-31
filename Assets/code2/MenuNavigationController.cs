using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigationController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject menuPanel;
    public GameObject levelSelectPanel;
    public GameObject characterSelectPanel;

    // Gắn vào nút "Bắt đầu"
    public void OpenLevelSelect()
    {
        menuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    // Gắn vào từng nút chọn màn (Màn 1, Màn 2...)
    public void SelectLevel(string levelName)
    {
        GameSession.SelectedLevelName = levelName;
        levelSelectPanel.SetActive(false);
        characterSelectPanel.SetActive(true);
    }

    // Gắn vào từng nút chọn nhân vật
    public void SelectCharacter(int index)
    {
        GameSession.SelectedCharacterIndex = index;
        SceneManager.LoadScene(GameSession.SelectedLevelName);
    }

    // Nút quay lại (tùy chọn, không bắt buộc)
    public void BackToMenu()
    {
        levelSelectPanel.SetActive(false);
        characterSelectPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void BackToLevelSelect()
    {
        characterSelectPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }
}
