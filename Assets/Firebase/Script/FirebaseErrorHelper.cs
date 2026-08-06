using Firebase;
using Firebase.Auth;
using System;

public static class FirebaseErrorHelper
{
    public static string GetErrorMessage(AggregateException exception)
    {
        if (exception == null)
            return "Đã xảy ra lỗi không xác định.";

        FirebaseException firebaseException =
            exception.Flatten().InnerExceptions[0] as FirebaseException;

        if (firebaseException == null)
            return exception.Message;

        AuthError errorCode = (AuthError)firebaseException.ErrorCode;

        switch (errorCode)
        {
            case AuthError.EmailAlreadyInUse:
                return "Email đã được sử dụng.";

            case AuthError.InvalidEmail:
                return "Email không hợp lệ.";

            case AuthError.WeakPassword:
                return "Mật khẩu quá yếu.";

            case AuthError.MissingEmail:
                return "Vui lòng nhập email.";

            case AuthError.MissingPassword:
                return "Vui lòng nhập mật khẩu.";

            default:
                return errorCode.ToString();
        }
    }
}