using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
public class batdau : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Hover Settings")]
    public TextMeshProUGUI label;
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(1f, 0.85f, 0.3f);
    [Header("Click Action")]
    public MenuAction actionType = MenuAction.None;
    public string sceneToLoad;
    public enum MenuAction
    {
        None,
        LoadScene,
        QuitGame
    }
    void Start()
    {
        if (label == null)
            label = GetComponentInChildren<TextMeshProUGUI>();
        label.color = normalColor;
    }
    public void OnPointerEnter(PointerEventData eventData) { label.color = hoverColor; }
    public void OnPointerExit(PointerEventData eventData) { label.color = normalColor; }
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (actionType)
        {
            case MenuAction.LoadScene:
                if (!string.IsNullOrEmpty(sceneToLoad)) SceneManager.LoadScene(sceneToLoad);
                else Debug.LogWarning("Chưa điền tên scene ở ô Scene To Load!");
                break;
            case MenuAction.QuitGame:
                Application.Quit();
                break;
            case MenuAction.None:
                break;
        }
    }
}