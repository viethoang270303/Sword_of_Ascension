using UnityEngine;

public class SettingsPanelController : MonoBehaviour
{
    [Header("Quản lý Panel")]
    public GameObject settingsPanel;
    public GameObject confirmResetPanel; // Gắn bảng hỏi Yes/No vào đây

    public void OpenSettings()
    {
        // 1. Mở bảng Settings
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        // 2. CHẶN ĐỨNG BẢNG XÁC NHẬN: Ép nó ẩn đi, chỉ chờ lúc ấn Reset mới được hiện
        if (confirmResetPanel != null)
            confirmResetPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // ==========================================
    // CÁC HÀM XỬ LÝ BẢNG XÁC NHẬN RESET
    // ==========================================

    // Gắn vào nút "Reset dữ liệu"
    public void HienThiBangXacNhanReset()
    {
        if (confirmResetPanel != null) confirmResetPanel.SetActive(true);
    }

    // Gắn vào nút "Không"
    public void AnBangXacNhanReset()
    {
        if (confirmResetPanel != null) confirmResetPanel.SetActive(false);
    }

    // Gắn vào nút "Đồng ý"
    public void XacNhanResetDuLieu()
    {
        // 1. Reset dữ liệu về Màn 1
        PlayerPrefs.SetInt("UnlockedLevelCount", 1);
        PlayerPrefs.Save();

        // 2. Tìm Script quản lý chọn màn và tự động gọi hàm Cập nhật lại Ổ khóa
        LevelCharacterSelectController levelUI = FindFirstObjectByType<LevelCharacterSelectController>();
        if (levelUI != null)
        {
            // Dùng SendMessage để ép gọi hàm RefreshLevelLockState dù nó đang là private
            levelUI.SendMessage("RefreshLevelLockState", SendMessageOptions.DontRequireReceiver);
        }

        // 3. Tắt bảng hỏi đi
        AnBangXacNhanReset();

        Debug.Log("Đã Reset tiến trình thành công từ Cài Đặt!");
    }
}