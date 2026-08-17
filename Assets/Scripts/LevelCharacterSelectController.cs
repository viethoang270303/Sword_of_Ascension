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
    public static LevelCharacterSelectController instance;

    [Header("Nut chon man (theo dung thu tu)")]
    public GameObject[] levelHighlights;
    public string[] levelSceneNames = { "Manchoi1", "Manchoi2", "Manchoi3" };
    public Button[] levelButtons;
    public GameObject[] levelLockIcons;

    [Header("Nut chon nhan vat (theo dung thu tu)")]
    public Button[] characterButtons;
    public GameObject[] characterHighlights;
    public PlayerData[] playerDataList;

    [Header("Panel hien thi thong tin Player")]
    public GameObject playerInfoPanel;
    public TextMeshProUGUI playerInfoNameText;
    public TextMeshProUGUI playerInfoDescText;
    public Image playerInfoPortraitImage;

    [Header("Nut Bat Dau Choi")]
    public Button startGameButton;

    private string selectedLevel = "";
    private int selectedCharacter = -1;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            instance.TruyenDuLieuUIChoBanGoc(this);
        }
    }

    void Start()
    {
        if (instance == this)
        {
            KhoiTaoMenu();
        }
    }

    public void TruyenDuLieuUIChoBanGoc(LevelCharacterSelectController clone)
    {
        this.levelHighlights = clone.levelHighlights;
        this.levelButtons = clone.levelButtons;
        this.levelLockIcons = clone.levelLockIcons;
        this.characterButtons = clone.characterButtons;
        this.characterHighlights = clone.characterHighlights;
        this.playerInfoPanel = clone.playerInfoPanel;
        this.playerInfoNameText = clone.playerInfoNameText;
        this.playerInfoDescText = clone.playerInfoDescText;
        this.playerInfoPortraitImage = clone.playerInfoPortraitImage;

        this.startGameButton = clone.startGameButton;

        this.KhoiTaoMenu();
    }

    void KhoiTaoMenu()
    {
        RefreshLevelLockState();
        if (playerInfoPanel != null)
            playerInfoPanel.SetActive(false);

        // --- BẢN VÁ: Tự động gắn hàm cho các nút Chọn Màn Chơi (Chống đứt link) ---
        if (levelButtons != null)
        {
            for (int i = 0; i < levelButtons.Length; i++)
            {
                int index = i;
                if (levelButtons[i] != null)
                {
                    levelButtons[i].onClick.RemoveAllListeners();
                    levelButtons[i].onClick.AddListener(() => ChonMan(index));
                }
            }
        }

        if (characterButtons != null)
        {
            for (int i = 0; i < characterButtons.Length; i++)
            {
                int index = i;
                if (characterButtons[i] != null)
                {
                    characterButtons[i].onClick.RemoveAllListeners();
                    characterButtons[i].onClick.AddListener(() => ChonNhanVat(index));
                }
            }
        }

        if (startGameButton != null)
        {
            startGameButton.onClick.RemoveAllListeners();
            startGameButton.onClick.AddListener(BatDauChoi);
        }
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
        if (playerInfoNameText != null) playerInfoNameText.text = data.playerName;
        if (playerInfoDescText != null) playerInfoDescText.text = data.description;
        if (playerInfoPortraitImage != null && data.portrait != null) playerInfoPortraitImage.sprite = data.portrait;
    }

    public void BatDauChoi()
    {
        if (string.IsNullOrEmpty(selectedLevel)) { Debug.LogWarning("Chưa chọn màn chơi!"); return; }
        if (selectedCharacter == -1) { Debug.LogWarning("Chưa chọn nhân vật!"); return; }

        GameSession.SelectedLevelName = selectedLevel;
        GameSession.SelectedCharacterIndex = selectedCharacter;

        StoryController storyController = FindFirstObjectByType<StoryController>();
        if (storyController != null)
        {
            storyController.OpenStory();
        }
        else
        {
            SceneManager.LoadScene(selectedLevel);
        }
    }

    // ===============================================
    // HÀM RESET GIAO DIỆN (Bác gắn cái này vào nút X đóng Menu nhé)
    // ===============================================
    public void ResetGiaoDienKhiDong()
    {
        if (playerInfoPanel != null) playerInfoPanel.SetActive(false);

        selectedCharacter = -1;
        if (characterHighlights != null)
        {
            foreach (var hl in characterHighlights)
            {
                if (hl != null) hl.SetActive(false);
            }
        }

        selectedLevel = "";
        if (levelHighlights != null)
        {
            foreach (var hl in levelHighlights)
            {
                if (hl != null) hl.SetActive(false);
            }
        }
    }
}