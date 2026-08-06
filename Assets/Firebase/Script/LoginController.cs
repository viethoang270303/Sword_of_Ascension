using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;

    [Header("Scene sau khi đăng nhập thành công")]
    public string gameSceneName = "MainGame";

    public void OnLoginButtonClicked()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowMessage("Vui lòng nhập Email và Mật khẩu.");
            return;
        }

        if (!FirebaseManager.Instance.IsFirebaseReady)
        {
            ShowMessage("Firebase chưa sẵn sàng, vui lòng thử lại.");
            return;
        }

        Login(email, password);
    }

    private void Login(string email, string password)
    {
        ShowMessage("Đang đăng nhập...");

        FirebaseAuth auth = FirebaseManager.Instance.Auth;
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                string errorMessage = FirebaseErrorHelper.GetErrorMessage(task.Exception);
                ShowMessage(errorMessage);
                return;
            }

            AuthResult result = task.Result;
            FirebaseUser user = result.User;
            Debug.Log($"[Login] Đăng nhập thành công: {user.Email}");

            ShowMessage("Đăng nhập thành công!");

            // Chuyển sang scene chính của game
            SceneManager.LoadScene(gameSceneName);
        });
    }

    private void ShowMessage(string msg)
    {
        if (messageText != null) messageText.text = msg;
        Debug.Log($"[Login] {msg}");
    }
}