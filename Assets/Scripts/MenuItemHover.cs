using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MenuItemHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI label;
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(1f, 0.85f, 0.3f); // vàng

    void Start()
    {
        if (label == null)
            label = GetComponentInChildren<TextMeshProUGUI>();

        label.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        label.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        label.color = normalColor;
    }
}