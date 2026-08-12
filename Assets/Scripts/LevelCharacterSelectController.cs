using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelCharacterSelectController : MonoBehaviour
{
    [Header("Nut chon man (theo dung thu tu)")]
    public GameObject[] levelHighlights;
    public string[] levelSceneNames = { "Manchoi1", "Manchoi2", "Manchoi3" };
    [Header("Nut chon nhan vat (theo dung thu tu)")]
    public GameObject[] characterHighlights;
    private string selectedLevel = "";
    private int selectedCharacter = -1;

    public void ChonMan(int index)
    {
        if (index < 0 || index >= levelSceneNames.Length) return;
        selectedLevel = levelSceneNames[index];
        for (int i = 0; i < levelHighlights.Length; i++)
            if (levelHighlights[i] != null) levelHighlights[i].SetActive(i == index);
    }

    public void ChonNhanVat(int index)
    {
        selectedCharacter = index;
        for (int i = 0; i < characterHighlights.Length; i++)
            if (characterHighlights[i] != null) characterHighlights[i].SetActive(i == index);
    }

    public void BatDauChoi()
    {
        if (string.IsNullOrEmpty(selectedLevel)) { Debug.LogWarning("Chưa chọn màn chơi!"); return; }
        if (selectedCharacter == -1) { Debug.LogWarning("Chưa chọn nhân vật!"); return; }

        GameSession.SelectedLevelName = selectedLevel;
        GameSession.SelectedCharacterIndex = selectedCharacter;

        SceneManager.LoadScene(selectedLevel); // load thẳng, không cần additive/Persistent
    }
}