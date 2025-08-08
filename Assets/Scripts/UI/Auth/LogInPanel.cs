using Firebase.Auth;
using Firebase.Extensions;
using Google;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogInPanel : MonoBehaviour 
{
    [SerializeField] TMP_InputField emialInputField;
    [SerializeField] TMP_InputField passwordInputField;

    [SerializeField] NickNamePanel nickNamePanel;
    [SerializeField] VerifyPanel verifyPanel;

    private GoogleSignInConfiguration _googleConfig;
    public const string GOOGLE_WEB_API = "40377125664-1r1e1doid7jb1gggf5r2dm6q16od6dlp.apps.googleusercontent.com";
    private void Awake()
    {
        InitGoogleConfigu();
    }

    public void CheckUserInfo()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)
            return;

        Debug.Log($"Display Name : {user.DisplayName}");
        Debug.Log($"Email : {user.Email}");
        Debug.Log($"Email Verified : {user.IsEmailVerified}");
        Debug.Log($"User ID : {user.UserId}");

        if (user.IsEmailVerified == false)
        {
            verifyPanel.gameObject.SetActive(true);
        }
        else if (user.DisplayName == "")
        {
            nickNamePanel.gameObject.SetActive(true);
        }

        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Email�� Password�� �̿��Ͽ� �α����ϴ� �Լ�
    /// </summary>
    public void LogIn()
    {
        string email = emialInputField.text;
        string password = passwordInputField.text;

        BackendManager.Auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;
            Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");
            CheckUserInfo();
        });
    }

    /// <summary>
    /// Google �α��� �Լ�
    /// </summary>
    #region ���� �α���
    private void InitGoogleConfigu()
    {
        _googleConfig = new GoogleSignInConfiguration
        {
            WebClientId = GOOGLE_WEB_API,
            RequestIdToken = true,
            RequestEmail = true
        };

        GoogleSignIn.Configuration = _googleConfig;
    }

    public void Button_GoolgeSignIn()
    {
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("����");
            }

            GoogleLogin(task);
        });

    }

    private void GoogleLogin(Task<GoogleSignInUser> userTask)
    {
        Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(userTask.Result.IdToken, null);
        BackendManager.Auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("����");
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat($"����: {result.User.DisplayName}, {result.User.UserId}");

            BackendManager.User = BackendManager.Auth.CurrentUser;
        });
    }
    #endregion
}