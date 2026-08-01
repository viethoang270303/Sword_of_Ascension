using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCharacterSelectController : MonoBehaviour
{
    [Header("Nut chon man (theo dung thu tu)")]
    public GameObject[] levelHighlights; // keo child "Highlight" cua tung nut Man vao day
    public string[] levelSceneNames = { "Manchoi1", "Manchoi2", "Manchoi3" };

    [Header("Nut chon nhan vat (theo dung thu tu)")]
    public GameObject[] characterHighlights; // keo child "Highlight" cua tung nut Nhan Vat vao day

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

        StartCoroutine(ChuyenVaoManChoi());
    }

    System.Collections.IEnumerator ChuyenVaoManChoi()
    {
        if (!SceneManager.GetSceneByName("Persistent").isLoaded)
            yield return SceneManager.LoadSceneAsync("Persistent", LoadSceneMode.Additive);

        yield return SceneManager.LoadSceneAsync(GameSession.SelectedLevelName, LoadSceneMode.Additive);

        Scene levelScene = SceneManager.GetSceneByName(GameSession.SelectedLevelName);
        SceneManager.SetActiveScene(levelScene);

        yield return SceneManager.UnloadSceneAsync("Main Menu");
    }
}