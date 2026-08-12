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

    // Biến dùng chung cho toàn game
    public static bool isAdmin = false;

    // Nút Log In
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

    // Đăng nhập Firebase
    private void Login(string email, string password)
    {
        ShowMessage("Đang đăng nhập...");

        FirebaseAuth auth = FirebaseManager.Instance.Auth;

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                // Nếu lỗi
                if (task.IsCanceled || task.IsFaulted)
                {
                    string errorMessage = FirebaseErrorHelper.GetErrorMessage(task.Exception);
                    ShowMessage(errorMessage);
                    return;
                }

                // Thành công
                AuthResult result = task.Result;
                FirebaseUser user = result.User;

                Debug.Log("[Login] Đăng nhập thành công: " + user.Email);

                // ===== ADMIN =====
                if (user.Email == "khanh@gmail.com")
                {
                    isAdmin = true;
                    ShowMessage("Đăng nhập ADMIN thành công!");

                    Debug.Log("Load scene: Main Menu Admin");

                    // LOAD TRỰC TIẾP SCENE ADMIN
                    SceneManager.LoadScene("Main Menu Admin");
                }
                // ===== USER THƯỜNG =====
                else
                {
                    isAdmin = false;
                    ShowMessage("Đăng nhập thành công!");

                    Debug.Log("Load scene: Main Menu");

                    // LOAD TRỰC TIẾP SCENE THƯỜNG
                    SceneManager.LoadScene("Main Menu");
                }
            });
    }

    // Hiển thị thông báo
    private void ShowMessage(string msg)
    {
        if (messageText != null)
            messageText.text = msg;

        Debug.Log("[Login] " + msg);
    }
}