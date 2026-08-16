using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    // Tên scene menu chính
    public string menuSceneName = "Main Menu";

    // Hàm gọi khi bấm nút X hoặc nút Về Menu
    public void GoToMenu()
    {
        // THÊM DÒNG NÀY: Mở khóa thời gian trước khi chuyển Scene
        Time.timeScale = 1f;

        SceneManager.LoadScene(menuSceneName);
    }
}