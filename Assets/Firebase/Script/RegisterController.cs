using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;          // Dùng ô này làm TÊN TÀI KHOẢN
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_Text messageText;

    // Domain nội bộ, người chơi không cần biết
    private const string AccountDomain = "@mygame.local";

    public void OnRegisterButtonClicked()
    {
        string username = emailInput.text.Trim();
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        // Kiểm tra tên tài khoản
        if (string.IsNullOrEmpty(username))
        {
            ShowMessage("Vui lòng nhập tên tài khoản.");
            return;
        }

        // Không cho nhập khoảng trắng
        if (username.Contains(" "))
        {
            ShowMessage("Tên tài khoản không được có khoảng trắng.");
            return;
        }

        // Kiểm tra mật khẩu
        if (string.IsNullOrEmpty(password))
        {
            ShowMessage("Vui lòng nhập mật khẩu.");
            return;
        }

        if (password != confirmPassword)
        {
            ShowMessage("Mật khẩu xác nhận không khớp.");
            return;
        }

        if (password.Length < 6)
        {
            ShowMessage("Mật khẩu phải có ít nhất 6 ký tự.");
            return;
        }

        if (FirebaseManager.Instance == null)
        {
            ShowMessage("Không tìm thấy FirebaseManager.");
            return;
        }

        if (!FirebaseManager.Instance.IsFirebaseReady)
        {
            ShowMessage("Firebase chưa sẵn sàng.");
            return;
        }

        // Tạo email ảo từ username
        string firebaseEmail = username.ToLower() + AccountDomain;

        Register(firebaseEmail, username, password);
    }

    private void Register(string email, string username, string password)
    {
        ShowMessage("Đang tạo tài khoản...");

        FirebaseAuth auth = FirebaseManager.Instance.Auth;

        auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    ShowMessage("Đăng ký đã bị hủy.");
                    return;
                }

                if (task.IsFaulted)
                {
                    Debug.LogError("===== FIREBASE ERROR =====");
                    Debug.LogError(task.Exception);

                    foreach (var ex in task.Exception.Flatten().InnerExceptions)
                    {
                        Debug.LogError(ex);
                    }

                    ShowMessage("Tên tài khoản đã tồn tại hoặc không hợp lệ.");
                    return;
                }

                FirebaseUser user = task.Result.User;

                Debug.Log("Đăng ký thành công: " + username);
                Debug.Log("Firebase account: " + user.Email);

                ShowMessage("Đăng ký thành công!");

                Invoke(nameof(GoToLoginScene), 1.5f);
            });
    }

    private void GoToLoginScene()
    {
        SceneManager.LoadScene("Login");
    }

    private void ShowMessage(string msg)
    {
        if (messageText != null)
            messageText.text = msg;

        Debug.Log("[Register] " + msg);
    }
}