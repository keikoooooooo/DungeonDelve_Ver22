using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheatsManager : MonoBehaviour, IGUI
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_InputField inputField;
    [Space]
    [SerializeField] private TextBar textBarPrefab;
    [SerializeField] private Transform textContent;
    [Space]
    [SerializeField] private AutoScrollScrollView scrollView;
    [SerializeField] private RectTransform frameScale;

    //
    private readonly Vector2 posScale = new(294f, 433.0027f);
    private readonly Vector2 sizeScale = new(550f, 701.0052f);
    private readonly Vector2 posNotScale = new(294f, 265f);
    private readonly Vector2 sizeNotScale = new(550f, 365f);
    [HideInInspector] private UserData _userData;
    [HideInInspector] private PlayerController _player;
    private Inputs _input;
    private ItemNameCode[] itemCodes;
    private Coroutine _handleCoroutine;
    private readonly YieldInstruction _yieldReturn = new WaitForSeconds(.1f);
    private readonly int _maxLimit = 50;
    //
    [HideInInspector] private bool isOpen;
    private bool _enter;
    private bool _isStarted;
    
    private bool _checkType;
    private int _selectType;
    private bool _checkChild;
    private int _selectChild;
    
    
    // Default String Code
    private readonly string _pattern_ENTER = "enter";
    private readonly string _pattern_RESET = "reset";
    private readonly string _pattern_RETURN = "/b";
    private readonly string _pattern_SET_ITEM = @"\[KEY:(\d+),VAL:(\d+)]";
    
    
    
    private void OnEnable()
    {
        GUI_Manager.Add(this);
        _input = new Inputs();
        _input.Enable();
        _input.TESTER.Enter.performed += OnEnterChat;
        _input.TESTER.ScalePanel.started += OnScalePanel;
        _input.TESTER.ScalePanel.canceled += OnScalePanel;
    }
    private void Start()
    {
        _enter = false;
        panel.SetActive(false);
        itemCodes = (ItemNameCode[])Enum.GetValues(typeof(ItemNameCode));
    }
    private void OnDisable()
    {
        GUI_Manager.Remove(this);
        _input.TESTER.Enter.performed -= OnEnterChat;
        _input.TESTER.ScalePanel.started -= OnScalePanel;
        _input.TESTER.ScalePanel.canceled -= OnScalePanel;
        _input.Disable();
    }
    public void GetRef(GameManager _gameManager)
    {
        _userData = _gameManager.UserData;
        _player = _gameManager.Player;
    }
    public void UpdateData() { }
    
    
    private void OnScalePanel(InputAction.CallbackContext _context)
    {
        if (_context.ReadValueAsButton())
        {
            frameScale.sizeDelta = sizeScale;
            frameScale.anchoredPosition = posScale;
            inputField.DeactivateInputField();   
            scrollView.SetScroll(false);
        }
        else
        {
            scrollView.SetScroll(true);
            inputField.ActivateInputField();
            frameScale.sizeDelta = sizeNotScale;
            frameScale.anchoredPosition = posNotScale;
        }
    }
    private void OnEnterChat(InputAction.CallbackContext _context)
    {
        isOpen = !isOpen;
        if (!isOpen && string.IsNullOrEmpty(inputField.text))
        {
            _enter = false;
            ClosePanel();
            return;
        }
        
        _enter = inputField.text.Length != 0;
        OpenPanel();
        HandlerCheats();
    }
    private void OpenPanel()
    {          
        if (panel.activeSelf) return;
        panel.SetActive(true);
        CursorHandle.NoneLocked();
        GUI_Inputs.InputAction.UI.Disable();
        _player.input.PlayerInput.Disable();
        _player.cinemachineFreeLook.enabled = false;
        //
        _isStarted = false;
        _checkType = false;
        frameScale.sizeDelta = sizeNotScale;
        frameScale.anchoredPosition = posNotScale;
    }
    private void ClosePanel()
    {
        panel.SetActive(false);
        CursorHandle.Locked();
        GUI_Inputs.InputAction.UI.Enable();
        _player.input.PlayerInput.Enable();
        _player.cinemachineFreeLook.enabled = true;
        inputField.text = "";
    }
    private async void HandlerCheats()
    {
        inputField.ActivateInputField();
        if (string.IsNullOrEmpty(inputField.text)) return;
        if (CheckPattern(inputField.text, _pattern_RESET))
        {
            _isStarted = false;
            _checkType = false;
            ReleaseTextBox();
            SpawnText("<color=green>Reset Success!!!</color>");
            return;
        }
        
        if (!_isStarted)
        {
            if (!CheckPattern(inputField.text, _pattern_ENTER))
            {
                SpawnText("<color=#FF3434>Unable to start. Please try again. !</color>");
                return;
            } 
            
            _isStarted = true;
            SpawnText("<color=#2AFF37>Started....</color>");
            await Task.Delay(100);
            SpawnText("Please select the option you wish to execute.");
            await Task.Delay(100);
            SpawnText("1. Characters");
            await Task.Delay(100);
            SpawnText("2. Item");
            return;
        }
        OptionSelect();
        CheckLimit();
    }
    
    
    // Selected Option Handle
    private void OptionSelect()
    {
        if (!int.TryParse(inputField.text, out var result) && !_checkType)
        {
            SpawnText($"The syntax is invalid. Error option: <color=#FF3434>{inputField.text}</color>");
            return;
        }
        _checkType = true;
        switch (result)
        {
            case 1:
                _handleCoroutine ??= StartCoroutine(ShowPlayerConfig());
                break;
                
            case 2:
                _handleCoroutine ??= StartCoroutine(ShowItemCode());
                break;
        }
    }
    private IEnumerator ShowPlayerConfig()
    {
        yield return _yieldReturn;
    }
    private IEnumerator ShowItemCode()
    {
        SpawnText("<color=#0BFF7D>--------- ITEM ---------</color>");
        yield return _yieldReturn;
        SpawnText("Would you like to get information or set information?");
        yield return _yieldReturn;
        SpawnText("0. GETTER");
        SpawnText("1. SETTER");
        while (true)
        {
            if (_enter && inputField.text.Length != 0)
            {
                if (!int.TryParse(inputField.text, out var _result))
                {
                    SpawnText($"The syntax is invalid. Error option: <color=#FF3434>{inputField.text}</color>");
                    continue;
                }
                switch (_result)
                {
                    case 0:
                        SpawnText("<color=#2F6CFF>NAMECODE");
                        var _count = 0;
                        foreach (var itemCode in itemCodes)
                        {
                            SpawnText($"{_count}. {itemCode}");
                            yield return _yieldReturn;
                            _count++;
                        }
                        SpawnText("<color=#0BFF7D>--------- ITEM ---------</color>");
                        SpawnText("0. GETTER");
                        SpawnText("1. SETTER");
                        continue;
                        
                    case 1:
                        SpawnText($"Please enter in the following format: [KEY:x,VAL:x] to SET.");
                        while (true)
                        {
                            if (_enter)
                            {
                                if (inputField.text.Equals("/b", StringComparison.OrdinalIgnoreCase))
                                {
                                    SpawnText($"{inputField.text} <color=#0BFF7D>Return</color>");
                                    SpawnText("0. GETTER");
                                    SpawnText("1. SETTER");
                                    break;
                                }
                                
                                if (Regex.IsMatch(inputField.text, _pattern_SET_ITEM, RegexOptions.IgnoreCase))
                                {
                                    var _match = Regex.Match(inputField.text, _pattern_SET_ITEM, RegexOptions.IgnoreCase);
                                    var x = Convert.ToInt32(_match.Groups[1].Value);
                                    var y = Convert.ToInt32(_match.Groups[2].Value);
                                    
                                    SpawnText($"{itemCodes[x]}: VAL = {y}");
                                    _userData.IncreaseItemValue(itemCodes[x], y);
                                    yield return new WaitForSeconds(.5f);
                                    SpawnText("<color=#0BFF7D>SET Data Success...</color>");
                                }
                                else
                                {
                                    SpawnText($"<color=#FF3434>The syntax is invalid!!!</color>");
                                }
                            }
                            yield return null;
                        }
                        continue;
                    
                    default:
                        SpawnText($"Error option: <color=#FF3434>{inputField.text}</color>"); 
                        continue;
                }
            }
            yield return null;
        }
    }



    private void ReleaseTextBox()
    {
        inputField.text = "";
        for (var i = 0; i < textContent.childCount; i++)
        {
            Destroy(textContent.GetChild(i).gameObject);
        }
    }
    private void SpawnText(string _value)
    {
        var _textBar = Instantiate(textBarPrefab, textContent);
        _textBar.SetTitleText(FormatTitle(DateTime.Now));
        _textBar.SetValueText(_value);
        inputField.text = "";
        _enter = false;
    }
    private bool CheckPattern(string _valueCheck, string _pattern) => string.Equals(_valueCheck, _pattern, StringComparison.OrdinalIgnoreCase);
    private void CheckLimit()
    {
        var _count = textContent.childCount;
        if (_count <= _maxLimit) return;
        for (var i = 0; i < _count - _maxLimit; i++)
        {
            Destroy(textContent.GetChild(i).gameObject);
        }
    }
    private static string FormatTitle(DateTime _time)
    {
        var _hour = _time.Hour;
        var _minute = _time.Minute;
        var _second = _time.Second;
        var _day = _time.Day;
        var _month = _time.Month;
        return "[" + _hour.ToString("00") + ":" + _minute.ToString("00") + ":" + _second.ToString("00") + $" {_day}/{_month}]:";
    }
}
