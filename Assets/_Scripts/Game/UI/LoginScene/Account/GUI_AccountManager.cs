using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GUI_AccountManager : MonoBehaviour
{
    [SerializeField] private Button logoutBtt;
    [SerializeField] private Button accountBtt;
    [Space] 
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject panelAnimatedLoading;
    [SerializeField] private GameObject panelClickToBegin;
    [Space]
    [SerializeField] private GUI_Login guiLogin;
    [SerializeField] private GUI_Register guiRegister;
    [SerializeField] private GUI_ForgotPW guiForgotPw;

    private Coroutine _handleCoroutine;
    
    private void OnEnable()
    {
        OpenPanelLogin();
        
        panelClickToBegin.gameObject.SetActive(false);
        logoutBtt.onClick.AddListener(LogoutAccount);
        accountBtt.onClick.AddListener(OpenPanelLogin);
        accountBtt.gameObject.SetActive(true);
        logoutBtt.gameObject.SetActive(false);
        
        if(!PlayFabController.Instance) return;
        
        #region Login
        guiLogin.StartgameBtt.onClick.AddListener(PlayFabController.Instance.OnLogin);
        guiLogin.EnterEmailField.onEndEdit.AddListener(PlayFabController.Instance.GetUserEmail);
        guiLogin.EnterPWField.onEndEdit.AddListener(PlayFabController.Instance.GetUserPassword);      
        PlayFabController.Instance.OnLoginSuccessEvent.AddListener(HandleLoginSuccess);
        PlayFabController.Instance.OnAccountHandleFailureEvent.AddListener(guiLogin.SetErrorText);
        #endregion

        #region Register
        guiRegister.RegisterBtt.onClick.AddListener(PlayFabController.Instance.OnRegister);
        guiRegister.EnterUsernameField.onEndEdit.AddListener(PlayFabController.Instance.GetUserName);
        guiRegister.EnterEmailField.onEndEdit.AddListener(PlayFabController.Instance.GetUserEmail);
        guiRegister.EnterPWField.onEndEdit.AddListener(PlayFabController.Instance.GetUserPassword);
        PlayFabController.Instance.OnRegisterSuccessEvent.AddListener(HandleRegisterSuccess);
        PlayFabController.Instance.OnAccountHandleFailureEvent.AddListener(guiRegister.SetErrorText);
        #endregion

        #region Forgot password
        guiForgotPw.VerificationBtt.onClick.AddListener(PlayFabController.Instance.OnForgotPW);
        guiForgotPw.EnterEmailField.onEndEdit.AddListener(PlayFabController.Instance.GetUserEmail);
        PlayFabController.Instance.OnMailSendForgotPWSuccessEvent.AddListener(HandleRegisterSuccess);
        PlayFabController.Instance.OnAccountHandleFailureEvent.AddListener(guiForgotPw.SetErrorText);
        #endregion
        
    }
    private void OnDisable()
    {
        logoutBtt.onClick.RemoveListener(LogoutAccount);
        accountBtt.onClick.RemoveListener(OpenPanelLogin);
        
        if(!PlayFabController.Instance) return;
        
        #region Login
        guiLogin.StartgameBtt.onClick.RemoveListener(PlayFabController.Instance.OnLogin);
        guiLogin.EnterEmailField.onEndEdit.RemoveListener(PlayFabController.Instance.GetUserEmail);
        guiLogin.EnterPWField.onEndEdit.RemoveListener(PlayFabController.Instance.GetUserPassword);      
        PlayFabController.Instance.OnLoginSuccessEvent.RemoveListener(HandleLoginSuccess);
        PlayFabController.Instance.OnAccountHandleFailureEvent.RemoveListener(guiLogin.SetErrorText);
        #endregion

        #region Register
        guiRegister.RegisterBtt.onClick.RemoveListener(PlayFabController.Instance.OnRegister);
        guiRegister.EnterUsernameField.onEndEdit.RemoveListener(PlayFabController.Instance.GetUserName);
        guiRegister.EnterEmailField.onEndEdit.RemoveListener(PlayFabController.Instance.GetUserEmail);
        guiRegister.EnterPWField.onEndEdit.RemoveListener(PlayFabController.Instance.GetUserPassword);
        PlayFabController.Instance.OnRegisterSuccessEvent.RemoveListener(HandleRegisterSuccess);
        PlayFabController.Instance.OnAccountHandleFailureEvent.RemoveListener(guiRegister.SetErrorText);
        #endregion

        #region Forgot password
        guiForgotPw.VerificationBtt.onClick.RemoveListener(PlayFabController.Instance.OnForgotPW);
        guiForgotPw.EnterEmailField.onEndEdit.RemoveListener(PlayFabController.Instance.GetUserEmail);
        PlayFabController.Instance.OnMailSendForgotPWSuccessEvent.RemoveListener(HandleRegisterSuccess);
        PlayFabController.Instance.OnAccountHandleFailureEvent.RemoveListener(guiForgotPw.SetErrorText);
        #endregion
    }


    public void OpenPanelLogin()
    {
        animator.Play("Enable");
    }
    public void LogoutAccount()
    {
        ClearAccountTemp();
        accountBtt.gameObject.SetActive(true);
        logoutBtt.gameObject.SetActive(false);
        panelClickToBegin.SetActive(false);
    }
    private void HandleLoginSuccess()
    {
        if(_handleCoroutine != null) StopCoroutine(_handleCoroutine);
        _handleCoroutine = StartCoroutine(LoginSuccessCoroutine());
    }
    private IEnumerator LoginSuccessCoroutine()
    {
        panelAnimatedLoading.SetActive(true);
        yield return new WaitForSeconds(2f);
        animator.Play("Disable");
        logoutBtt.gameObject.SetActive(true);
        accountBtt.gameObject.SetActive(false);
        panelClickToBegin.SetActive(true);
        panelAnimatedLoading.SetActive(false);
    }

    private void HandleRegisterSuccess()
    {
        if(_handleCoroutine != null) StopCoroutine(_handleCoroutine);
        _handleCoroutine = StartCoroutine(RegisterSuccessCoroutine());
    }
    private IEnumerator RegisterSuccessCoroutine()
    {
        panelAnimatedLoading.SetActive(true);
        yield return new WaitForSeconds(1f);
        animator.Play("OpenLoginPanel");
        panelAnimatedLoading.SetActive(false);
    }
    

    public void ClearAccountTemp()
    {
        if(PlayFabController.Instance) PlayFabController.Instance.ClearAccountTemp();
    }
    
    
}