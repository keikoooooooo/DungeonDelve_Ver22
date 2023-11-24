using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

public class PlayFabAccount : MonoBehaviour
    {
        private readonly string TitleID = "59B82";
        private string username, userEmail, userPassword;

        public UnityEvent OnLoginSuccessEvent, OnLoginFailureEvent;
        public UnityEvent OnRegisterSuccessEvent, OnRegisterFailureEvent;
        public UnityEvent OnMailSendSuccessEvent, OnMailSendFailureEvent;
        
        private void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
            {
                PlayFabSettings.TitleId = TitleID;
            }

            if (!PlayerPrefs.HasKey("EMAIL")) return;
            
            Debug.Log("Auto Login");
            LoadAccount();
            OnLogin();
        }

        #region Login
        public void OnLogin()
        {
            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        }
        private void OnLoginSuccess(LoginResult _result)
        {
            OnLoginSuccessEvent?.Invoke();
            Debug.Log("OnLoginSuccess");
            SaveAccount();
        }
        private void OnLoginFailure(PlayFabError _error)
        {
            OnLoginFailureEvent?.Invoke();
            Debug.LogWarning(_error.Error);
        }
        #endregion

        #region Register
        public void OnRegister()
        {
            var request = new RegisterPlayFabUserRequest { Username = username, Email = userEmail, Password = userPassword };
            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterFailure);
        }
        private void OnRegisterSuccess(RegisterPlayFabUserResult _result)
        {
            OnRegisterSuccessEvent?.Invoke();
            Debug.Log("OnRegisterSuccess");
        }
        private void OnRegisterFailure(PlayFabError _error)
        {
            OnRegisterFailureEvent?.Invoke();
            Debug.LogWarning(_error.Error);
        }     
        #endregion

        #region Forgot Password
        public void OnForgotPW()
        {
            var request = new SendAccountRecoveryEmailRequest { Email = userEmail , TitleId = TitleID};
            PlayFabClientAPI.SendAccountRecoveryEmail(request, OnMailSendSuccess, OnMailSendFailure);
        }
        private void OnMailSendSuccess(SendAccountRecoveryEmailResult _result)
        {
            OnMailSendSuccessEvent?.Invoke();
            Debug.Log("OnMailSendSuccess");
        }
        private void OnMailSendFailure(PlayFabError _error)
        {
            OnMailSendFailureEvent?.Invoke();
            Debug.LogWarning(_error.Error);
        }
        #endregion
        
        
        private void SaveAccount()
        {
            PlayerPrefs.SetString("EMAIL", userEmail);
            PlayerPrefs.SetString("PASSWORD", userPassword);
        }
        private void LoadAccount()
        {
            userEmail = PlayerPrefs.GetString("EMAIL");
            userPassword = PlayerPrefs.GetString("PASSWORD");
        }
        
        // OnInputFieldEvent
        public void GetUserName(string _nameIn) => username = _nameIn;
        public void GetUserEmail(string _emailIn) => userEmail = _emailIn;
        public void GetUserPassword(string _passwordIn) => userPassword = _passwordIn;

    }

