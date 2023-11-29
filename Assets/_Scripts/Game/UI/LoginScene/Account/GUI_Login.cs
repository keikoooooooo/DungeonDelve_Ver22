using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Login : MonoBehaviour
{
    public TextMeshProUGUI ErrorText;
    public TMP_InputField EnterEmailField;
    public TMP_InputField EnterPWField;
    public Button StartgameBtt;
    
    private void OnEnable() => SetDefaultErrorText();
    public void SetErrorText(string _errorText)
    {
        ErrorText.text = _errorText;
        Invoke(nameof(SetDefaultErrorText), 2.5f);
    }
    private void SetDefaultErrorText() => ErrorText.text = "";
}
