using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    // Tên scene menu chính
    public string menuSceneName = "Main Menu";

    // Hàm gọi khi bấm nút X
    public void GoToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}