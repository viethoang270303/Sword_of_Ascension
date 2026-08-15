using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    [Header("UI References")]

    // Ô này có thể nhập:
    // 1. username: khanh
    // 2. email cũ: khanh@gmail.com
    public TMP_InputField emailInput;

    public TMP_InputField passwordInput;
    public TMP_Text messageText;

    // =========================
    // ADMIN
    // =========================
    public static bool isAdmin = false;

    // Tài khoản username sẽ được chuyển thành email nội bộ
    // Ví dụ:
    // toan -> toan@mygame.local
    private const string AccountDomain = "@mygame.local";

    // Email admin cũ của bạn
    private const string AdminEmail = "khanh@gmail.com";


    // =========================================================
    // NÚT LOGIN
    // =========================================================

    public void OnLoginButtonClicked()
    {
        string usernameOrEmail = emailInput.text.Trim();
        string password = passwordInput.text;

        // -------------------------
        // Kiểm tra input
        // -------------------------

        if (string.IsNullOrEmpty(usernameOrEmail))
        {
            ShowMessage("Vui lòng nhập Tên tài khoản.");
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            ShowMessage("Vui lòng nhập Mật khẩu.");
            return;
        }


        // -------------------------
        // Kiểm tra Firebase
        // -------------------------

        if (FirebaseManager.Instance == null)
        {
            ShowMessage("Không tìm thấy FirebaseManager.");
            return;
        }

        if (!FirebaseManager.Instance.IsFirebaseReady)
        {
            ShowMessage("Firebase chưa sẵn sàng, vui lòng thử lại.");
            return;
        }


        // -------------------------
        // Chuyển username -> email
        // -------------------------

        string firebaseEmail;

        if (usernameOrEmail.Contains("@"))
        {
            // Nếu người dùng nhập email thật
            // Ví dụ:
            // khanh@gmail.com

            firebaseEmail = usernameOrEmail.ToLower();
        }
        else
        {
            // Nếu người dùng nhập username
            // Ví dụ:
            // toan

            firebaseEmail = usernameOrEmail.ToLower() + AccountDomain;
        }


        Debug.Log("[Login] Input: " + usernameOrEmail);
        Debug.Log("[Login] Firebase Email: " + firebaseEmail);


        // -------------------------
        // Login Firebase
        // -------------------------

        Login(firebaseEmail, password);
    }


    // =========================================================
    // LOGIN FIREBASE
    // =========================================================

    private void Login(string email, string password)
    {
        ShowMessage("Đang đăng nhập...");

        FirebaseAuth auth = FirebaseManager.Instance.Auth;

        auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWithOnMainThread(task =>
            {
                // =================================================
                // LOGIN THẤT BẠI
                // =================================================

                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.LogError("===== LOGIN FIREBASE ERROR =====");

                    if (task.Exception != null)
                    {
                        Debug.LogError(task.Exception);
                    }

                    string errorMessage =
                        FirebaseErrorHelper.GetErrorMessage(task.Exception);

                    ShowMessage(errorMessage);

                    return;
                }


                // =================================================
                // LOGIN THÀNH CÔNG
                // =================================================

                AuthResult result = task.Result;

                FirebaseUser user = result.User;

                if (user == null)
                {
                    ShowMessage("Không lấy được thông tin tài khoản.");
                    return;
                }


                Debug.Log("=================================");
                Debug.Log("[Login] Đăng nhập thành công!");
                Debug.Log("[Login] Firebase Email: " + user.Email);
                Debug.Log("[Login] User ID: " + user.UserId);
                Debug.Log("=================================");


                // =================================================
                // ADMIN
                // =================================================

                if (user.Email != null &&
                    user.Email.ToLower() == AdminEmail.ToLower())
                {
                    isAdmin = true;

                    ShowMessage("Đăng nhập ADMIN thành công!");

                    Debug.Log("[Login] ADMIN");
                    Debug.Log("[Login] Load scene: Main Menu Admin");

                    SceneManager.LoadScene("Main Menu Admin");

                    return;
                }


                // =================================================
                // USER THƯỜNG
                // =================================================

                isAdmin = false;

                ShowMessage("Đăng nhập thành công!");

                Debug.Log("[Login] USER");
                Debug.Log("[Login] Load scene: Main Menu");

                SceneManager.LoadScene("Main Menu");
            });
    }


    // =========================================================
    // HIỂN THỊ MESSAGE
    // =========================================================

    private void ShowMessage(string msg)
    {
        if (messageText != null)
        {
            messageText.text = msg;
        }

        Debug.Log("[Login] " + msg);
    }
}