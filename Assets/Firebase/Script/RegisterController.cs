using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_Text messageText;

    public void OnRegisterButtonClicked()
    {
        string email = emailInput.text.Trim();
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            ShowMessage("Vui lòng nhập Email và Mật khẩu.");
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

        Register(email, password);
    }

    private void Register(string email, string password)
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

                    ShowMessage(task.Exception.Flatten().InnerExceptions[0].Message);
                    return;
                }

                FirebaseUser user = task.Result.User;

                Debug.Log("Đăng ký thành công: " + user.Email);

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