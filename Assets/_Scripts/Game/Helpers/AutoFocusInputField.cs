using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoFocusInputField : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    private Coroutine _fieldCoroutine;

    private void OnEnable() => Focus();
    
    /// <summary>
    /// Tự động chọn được nhập vào Field mỗi khi hàm này được gọi
    /// </summary>
    public void Focus()
    {
        if (_fieldCoroutine != null) StopCoroutine(_fieldCoroutine);
        _fieldCoroutine = StartCoroutine(SelectInputFieldCoroutine());
    }
    private IEnumerator SelectInputFieldCoroutine()
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
        inputField.OnPointerClick(new PointerEventData(EventSystem.current));
        inputField.text = "";
    }

}
