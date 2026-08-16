using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    [Header("Cài đặt Mở khóa (Chỉnh trong Unity)")]
    public string tenSceneMenu = "Main Menu";

    [Tooltip("Đánh xong màn này thì mở khóa màn số mấy? (Màn 1 điền 2, Màn 2 điền 3)")]
    public int manMuonMoKhoa = 2;

    public void NhanThuongVaVeMenu()
    {
        // 1. MỞ KHÓA MÀN CHƠI TIẾP THEO
        int currentUnlocked = PlayerPrefs.GetInt("UnlockedLevelCount", 1);

        // Nếu số màn đang mở nhỏ hơn mốc muốn mở -> thì mới mở khóa
        if (currentUnlocked < manMuonMoKhoa)
        {
            PlayerPrefs.SetInt("UnlockedLevelCount", manMuonMoKhoa);
            PlayerPrefs.Save();
            Debug.Log("Đã mở khóa Màn " + manMuonMoKhoa + " thành công!");
        }

        // 2. MỞ KHÓA THỜI GIAN
        Time.timeScale = 1f;

        // 3. LOAD VỀ MENU CHÍNH
        SceneManager.LoadScene(tenSceneMenu);
    }
}