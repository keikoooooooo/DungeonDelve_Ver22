using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_ForgotPW : MonoBehaviour
{
    public TextMeshProUGUI ErrorText;
    public TMP_InputField EnterEmailField;
    public Button VerificationBtt;

    private void OnEnable() => SetDefaultErrorText();
    public void SetErrorText(string _errorText)
    {
        ErrorText.text = _errorText;
        Invoke(nameof(SetDefaultErrorText), 2.5f);
    }
    private void SetDefaultErrorText() => ErrorText.text = "";
}
