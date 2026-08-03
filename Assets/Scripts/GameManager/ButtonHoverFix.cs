using UnityEngine;
using UnityEngine.EventSystems;

// Gắn script này vào tất cả các nút bấm của bạn
public class ButtonHoverFix : MonoBehaviour, IPointerEnterHandler
{
    // Hàm này tự động chạy khi CHUỘT lướt vào phạm vi của nút
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Ép hệ thống chuyển tiêu điểm (Focus) của tay cầm vào nút này
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }
}