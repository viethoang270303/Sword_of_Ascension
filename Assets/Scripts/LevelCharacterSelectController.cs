using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class PlayerData
{
    public string playerName;
    [TextArea(2, 3)]
    public string description;
    public Sprite portrait;
}

public class LevelCharacterSelectController : MonoBehaviour
{
    [Header("Nut chon man (theo dung thu tu)")]
    public GameObject[] levelHighlights;
    public string[] levelSceneNames = { "Manchoi1", "Manchoi2", "Manchoi3" };
    public Button[] levelButtons;
    public GameObject[] levelLockIcons;

    [Header("Nut chon nhan vat (theo dung thu tu)")]
    public GameObject[] characterHighlights;
    public PlayerData[] playerDataList; // thong tin tung player

    [Header("Panel hien thi thong tin Player")]
    public GameObject playerInfoPanel;
    public TextMeshProUGUI playerInfoNameText;
    public TextMeshProUGUI playerInfoDescText;
    public Image playerInfoPortraitImage;

    private string selectedLevel = "";
    private int selectedCharacter = -1;

    void Start()
    {
        RefreshLevelLockState();
        if (playerInfoPanel != null)
            playerInfoPanel.SetActive(false);
    }

    void RefreshLevelLockState()
    {
        int unlockedCount = PlayerPrefs.GetInt("UnlockedLevelCount", 1);

        for (int i = 0; i < levelSceneNames.Length; i++)
        {
            bool isUnlocked = i < unlockedCount;

            if (levelButtons != null && i < levelButtons.Length && levelButtons[i] != null)
                levelButtons[i].interactable = isUnlocked;

            if (levelLockIcons != null && i < levelLockIcons.Length && levelLockIcons[i] != null)
                levelLockIcons[i].SetActive(!isUnlocked);
        }
    }
     
    public void ChonMan(int index)
    {
        if (index < 0 || index >= levelSceneNames.Length) return;

        int unlockedCount = PlayerPrefs.GetInt("UnlockedLevelCount", 1);
        if (index >= unlockedCount)
        {
            Debug.LogWarning("Màn chơi này chưa được mở khoá!");
            return;
        }

        selectedLevel = levelSceneNames[index];
        for (int i = 0; i < levelHighlights.Length; i++)
            if (levelHighlights[i] != null) levelHighlights[i].SetActive(i == index);
    }

    public void ChonNhanVat(int index)
    {
        selectedCharacter = index;

        for (int i = 0; i < characterHighlights.Length; i++)
            if (characterHighlights[i] != null) characterHighlights[i].SetActive(i == index);

        ShowPlayerInfo(index);
    }

    void ShowPlayerInfo(int index)
    {
        if (playerDataList == null || index < 0 || index >= playerDataList.Length) return;
        if (playerInfoPanel == null) return;

        PlayerData data = playerDataList[index];

        playerInfoPanel.SetActive(true);

        if (playerInfoNameText != null)
            playerInfoNameText.text = data.playerName;

        if (playerInfoDescText != null)
            playerInfoDescText.text = data.description;

        if (playerInfoPortraitImage != null && data.portrait != null)
            playerInfoPortraitImage.sprite = data.portrait;
    }

    public void BatDauChoi()
    {
        if (string.IsNullOrEmpty(selectedLevel)) { Debug.LogWarning("Chưa chọn màn chơi!"); return; }
        if (selectedCharacter == -1) { Debug.LogWarning("Chưa chọn nhân vật!"); return; }

        GameSession.SelectedLevelName = selectedLevel;
        GameSession.SelectedCharacterIndex = selectedCharacter;

        SceneManager.LoadScene(selectedLevel);
    }
}