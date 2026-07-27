using UnityEngine;
using UnityEngine.UI;

public class FPSSettingsController : MonoBehaviour
{
    public Toggle highFpsToggle;

    [Header("FPS Settings")]
    public int normalFPS = 60;
    public int highFPS = 120;

    void Start()
    {
        // Lấy trạng thái đã lưu lần trước (mặc định false = FPS thường)
        bool savedState = PlayerPrefs.GetInt("HighFPS", 0) == 1;
        highFpsToggle.isOn = savedState;
        ApplyFPS(savedState);

        // Lắng nghe khi người dùng bấm tick
        highFpsToggle.onValueChanged.AddListener(ApplyFPS);
    }

    public void ApplyFPS(bool isHigh)
    {
        QualitySettings.vSyncCount = 0; // tắt VSync để targetFrameRate có tác dụng
        Application.targetFrameRate = isHigh ? highFPS : normalFPS;

        PlayerPrefs.SetInt("HighFPS", isHigh ? 1 : 0);

        Debug.Log("FPS hiện tại đặt: " + Application.targetFrameRate);
    }
}