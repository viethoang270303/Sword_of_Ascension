using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void GoToRegister()
    {
        SceneManager.LoadScene("Register");
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene("Login");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}