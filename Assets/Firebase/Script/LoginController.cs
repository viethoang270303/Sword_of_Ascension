using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;      // Dùng ô này làm TÊN TÀI KHOẢN
    public TMP_InputField passwordInput;
    public TMP_Text messageText;

    public static bool isAdmin = false;

    private const string AccountDomain = "@mygame.local";

    public void OnLoginButtonClicked()
    {
        string username = emailInput.text.Trim();
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowMessage("Vui lòng nhập Tên tài khoản và Mật khẩu.");
            return;
        }

        if (username.Contains(" "))
        {
            ShowMessage("Tên tài khoản không được có khoảng trắng.");
            return;
        }

        if (!FirebaseManager.Instance.IsFirebaseReady)
        {
            ShowMessage("Firebase chưa sẵn sàng, vui lòng thử lại.");
            return;
        }

        // Chuyển username thành email nội bộ
        string firebaseEmail = username.ToLower() + AccountDomain;

        Login(firebaseEmail, username, password);
    }

    private void Login(string email, string username, string password)
    {
        ShowMessage("Đang đăng nhập...");

        FirebaseAuth auth = FirebaseManager.Instance.Auth;

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    string errorMessage =
                        FirebaseErrorHelper.GetErrorMessage(task.Exception);

                    ShowMessage(errorMessage);
                    return;
                }

                AuthResult result = task.Result;
                FirebaseUser user = result.User;

                Debug.Log("[Login] Đăng nhập thành công: " + username);

                // =========================
                // ADMIN
                // =========================
                if (username.ToLower() == "khanh")
                {
                    isAdmin = true;

                    ShowMessage("Đăng nhập ADMIN thành công!");

                    Debug.Log("Load scene: Main Menu Admin");

                    SceneManager.LoadScene("Main Menu Admin");
                }

                // =========================
                // USER THƯỜNG
                // =========================
                else
                {
                    isAdmin = false;

                    ShowMessage("Đăng nhập thành công!");

                    Debug.Log("Load scene: Main Menu");

                    SceneManager.LoadScene("Main Menu");
                }
            });
    }

    private void ShowMessage(string msg)
    {
        if (messageText != null)
            messageText.text = msg;

        Debug.Log("[Login] " + msg);
    }
}