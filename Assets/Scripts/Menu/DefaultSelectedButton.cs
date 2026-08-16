using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DefaultSelectedButton : MonoBehaviour
{
    public GameObject firstSelected;

    // Đổi Start() thành IEnumerator Start() để dùng được lệnh chờ (yield)
    IEnumerator Start()
    {
        // 1. CHỜ 1 FRAME: Đảm bảo toàn bộ MenuItemHover và giao diện đã khởi tạo xong 100%
        yield return null;

        // 2. Kiểm tra xem EventSystem có tồn tại không rồi mới chọn nút (chống sập code)
        if (EventSystem.current != null && firstSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy EventSystem hoặc chưa gán firstSelected!");
        }
    }
}