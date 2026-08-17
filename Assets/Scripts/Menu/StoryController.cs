using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

[System.Serializable]
public class StoryScene
{
    public Sprite sceneImage;
    [TextArea(3, 5)]
    public string storyText;
}

public class StoryController : MonoBehaviour
{
    [Header("Panel Cot Truyen")]
    public GameObject storyPanel;

    [Header("Danh sach cac canh (theo dung thu tu)")]
    public StoryScene[] scenes;

    [Header("UI References")]
    public Image storyImage;
    public TextMeshProUGUI storyText;
    public Button clickCatcher;

    [Header("Scene se vao sau khi het truyen")]
    public string nextSceneName = "Manchoi1";

    private int currentSceneIndex = 0;

    void Start()
    {
        if (clickCatcher != null)
            clickCatcher.onClick.AddListener(NextScene);
    }

    public void OpenStory()
    {
        storyPanel.SetActive(true);
        currentSceneIndex = 0;
        ShowScene(0);
    }

    void ShowScene(int index)
    {
        if (index < 0 || index >= scenes.Length) return;

        currentSceneIndex = index;
        StoryScene scene = scenes[index];

        if (storyImage != null && scene.sceneImage != null)
            storyImage.sprite = scene.sceneImage;

        if (storyText != null)
            storyText.text = scene.storyText;
    }

    void NextScene()
    {
        if (!storyPanel.activeSelf) return; // tranh bam khi Panel dang an

        currentSceneIndex++;

        if (currentSceneIndex >= scenes.Length)
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            ShowScene(currentSceneIndex);
        }
    }
}