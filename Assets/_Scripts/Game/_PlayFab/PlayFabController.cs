using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

public class PlayFabController : Singleton<PlayFabController>
{
    [Space]
    public UnityEvent OnLoginSuccessEvent;
    public UnityEvent OnRegisterSuccessEvent;
    public UnityEvent OnMailSendForgotPWSuccessEvent;
    public UnityEvent<string>  OnAccountHandleFailureEvent;
    
    
    private readonly string TitleID = "59B82";
    private readonly string USERNAME_Key = "USERNAME";
    private readonly string EMAIL_Key = "EMAIL";
    private readonly string PW_Key = "PASSWORD";
    public string username { get; private set; }
    public string userEmail { get; private set; }
    public string userPassword { get; private set; }
    
    
    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = TitleID;
        }

        if (!PlayerPrefs.HasKey(EMAIL_Key))
        {
            OnAccountHandleFailureEvent?.Invoke("Please to login");
            return;
        }

        LoadAccount();
    }

    public void OnLogin()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, 
            result =>
            {
                SaveAccount();
                Debug.Log("OnLoginSuccess");
                OnLoginSuccessEvent?.Invoke();
            } ,
            error =>
            {
                OnAccountHandleFailureEvent?.Invoke(ErrorCallback(error));
                Debug.LogWarning(error.Error);
            }
        );
    }    
    public void OnRegister()
    {
        var request = new RegisterPlayFabUserRequest { Username = username, Email = userEmail, Password = userPassword };
        PlayFabClientAPI.RegisterPlayFabUser(request, 
            result =>
            {
                Debug.Log("OnRegisterSuccess");
                PlayerPrefs.SetString(USERNAME_Key, username);
                OnRegisterSuccessEvent?.Invoke();
            } ,
            error =>
            {
                Debug.LogWarning(error.Error);
                OnAccountHandleFailureEvent?.Invoke(ErrorCallback(error));
            } 
        );
    }
    public void OnForgotPW()
    {
        var request = new SendAccountRecoveryEmailRequest { Email = userEmail , TitleId = TitleID};
        PlayFabClientAPI.SendAccountRecoveryEmail(request,
            result =>
            {
                Debug.Log("OnMailSendSuccess");
                OnMailSendForgotPWSuccessEvent?.Invoke();
            } ,
            error =>
            {
                Debug.LogWarning(error.Error);
                OnAccountHandleFailureEvent?.Invoke(ErrorCallback(error));
            }
        );
    }
    private static string ErrorCallback(PlayFabError _error)
    {
        var keyMessage = _error.Error switch
        {
            PlayFabErrorCode.InvalidParams => "Invalid Parameters !",
            PlayFabErrorCode.UsernameNotAvailable => "Username Not Available !",
            PlayFabErrorCode.EmailAddressNotAvailable => "Email Address Not Available !",
            PlayFabErrorCode.InvalidEmailAddress => "Invalid Email Address !",
            PlayFabErrorCode.InvalidUsernameOrPassword or 
                PlayFabErrorCode.InvalidUsername or
                PlayFabErrorCode.InvalidPassword or
                PlayFabErrorCode.AccountNotFound
                => "Invalid Username Or Password",
            
            _ => $"{_error.Error}"
        };
        return keyMessage;
    }
    
    
    
    /// <summary>
    /// Lưu tài khoản vào bộ nhớ đệm
    /// </summary>
    private void SaveAccount()
    {
        PlayerPrefs.SetString(EMAIL_Key, userEmail);
        PlayerPrefs.SetString(PW_Key, userPassword);
    }
    
    /// <summary>
    /// Tải tài khoản từ bộ nhớ đệm 
    /// </summary>
    private void LoadAccount()
    {
        username = PlayerPrefs.GetString(USERNAME_Key);
        userEmail = PlayerPrefs.GetString(EMAIL_Key);
        userPassword = PlayerPrefs.GetString(PW_Key);
    }
    
    /// <summary>
    /// Xóa tài khoản đang lưu trong script
    /// </summary>
    public void ClearAccountTemp()
    {
        username = "";
        userEmail = "";
        userPassword = "";
        PlayerPrefs.DeleteKey(EMAIL_Key);
        PlayerPrefs.DeleteKey(PW_Key);
    }
    
    
    // OnInputFieldEvent
    public void GetUserName(string _nameIn) => username = _nameIn;
    public void GetUserEmail(string _emailIn) => userEmail = _emailIn;
    public void GetUserPassword(string _passwordIn) => userPassword = _passwordIn;
}
