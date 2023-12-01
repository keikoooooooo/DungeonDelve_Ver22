using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_CharacterUpgrade : MonoBehaviour, IGUI
{
    [Header("Infor")]
    [SerializeField] private TextMeshProUGUI charLevelText;
    [SerializeField] private TextMeshProUGUI charEXPText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [Space]
    [SerializeField] private Slider mainProgressSliderBar;
    [SerializeField] private Slider backProgressSliderBar;
    [Space]
    [SerializeField] private SO_ExperienceBuff smallExpBuff;
    [SerializeField] private SO_ExperienceBuff mediumExpBuff;
    [SerializeField] private SO_ExperienceBuff bigExpBuff;
    [Space]
    [SerializeField] private TextMeshProUGUI amountUseText;
    [SerializeField] private TextMeshProUGUI itemEXP_1ValueText;
    [SerializeField] private TextMeshProUGUI itemEXP_2ValueText;
    [SerializeField] private TextMeshProUGUI itemEXP_3ValueText;
    [SerializeField] private GameObject panelActivity_1;
    [SerializeField] private GameObject panelActivity_2;
    [SerializeField] private GameObject panelActivity_3;
    
    [Space]
    [SerializeField] private Button increaseAmountUseBtt;
    [SerializeField] private Button decreaseAmountUseBtt;

    // Variables
    private int _amountUse;          // số lượng item dùng
    private int _currentCoin;        // số coin hiện tại của user
    private int _totalCoinCost;      // tổng số coin cần để upgrade
    private int _totalExpReceived;   // tổng exp nhận được
    private int _selectItemCoin;     // loại item đang chọn (1, 2, 3 <=> Exp: Small, Medium, Big )
    private int _smallExpValue;      // số lượng của từng item đang có
    private int _mediumExpValue;    
    private int _bigExpValue;

    private int _currentLevel;       // lv hiện tại của nv
    private int _currentExp;         // kinh nghiệm hiện tại của nv
    private int _nextExp;            // kinh nghiệm tối đa để nâng cấp lên lv tiếp theo

    
    private bool _isEventRegistered;
    private UserData _userData;
    private SO_CharacterUpgradeData _upgradeData;
    private SO_PlayerConfiguration _playerConfig;

    
    private void Awake() => GUI_Manager.Add(this);
    private void OnEnable() => Init();
    private void OnDestroy()
    {
        GUI_Manager.Remove(this);
        if(!_isEventRegistered) return;
        _userData.OnCoinChangedEvent -= OnCoinChanged;
    }


    private void Init()
    {
        _totalCoinCost = 0;
        _selectItemCoin = 0;
        _totalCoinCost = 0;
        _totalExpReceived = 0;
        increaseAmountUseBtt.interactable = false;
        decreaseAmountUseBtt.interactable = false;
        panelActivity_1.SetActive(false);
        panelActivity_2.SetActive(false);
        panelActivity_3.SetActive(false);
        SetAmountUseText();
        SetTotalCoinCostText();
    }
    
    public void GetRef(UserData userData, SO_CharacterUpgradeData characterUpgradeData, SO_GameItemData gameItemData, PlayerController player)
    {
        _userData = userData;
        _upgradeData = characterUpgradeData;
        _playerConfig = player.PlayerConfig;
        
        UpdateData();

        if (_isEventRegistered) return;
        _isEventRegistered = true;
        _userData.OnCoinChangedEvent += OnCoinChanged;
    }
    private void OnCoinChanged(int _value)
    {
        _currentCoin = _value;
        currencyText.text = $"{_currentCoin}/{_totalCoinCost}";
    }
    
    
    public void UpdateData()
    {
        GetStatsCharacter();
        UpdateStatsText();
        SetItemValue();
        SetTotalCoinCostText();
        SetTotalEXPReceived();
    }
    
    private void GetStatsCharacter()
    {
        _currentLevel = _playerConfig.Level;
        _currentExp = _playerConfig.CurrentEXP;
        _nextExp = _upgradeData.GetNextEXP(_currentLevel);
    }
    private void UpdateStatsText()
    {
        if(!_playerConfig || !_upgradeData) return;
        
        charLevelText.text = $"Lv. {_currentLevel}";
        charEXPText.text = $"{_currentExp} / {_nextExp}";
        
        mainProgressSliderBar.maxValue = _nextExp;
        mainProgressSliderBar.minValue = 0;
        mainProgressSliderBar.value = _currentExp;
        backProgressSliderBar.maxValue = _nextExp;
        backProgressSliderBar.minValue = 0;
        backProgressSliderBar.value = _currentExp;
    }
    private void SetItemValue()
    {
        if (_userData.HasItemValue(ItemNameCode.EXPSmall, out var _value1))
        {
            _smallExpValue = _value1;
            itemEXP_1ValueText.text = $"{_value1}";
        }
        if (_userData.HasItemValue(ItemNameCode.EXPMedium, out var _value2))
        {
            _mediumExpValue = _value2;
            itemEXP_2ValueText.text = $"{_value2}";
        }
        if (_userData.HasItemValue(ItemNameCode.EXPBig, out var _value3))
        {
            _bigExpValue = _value3;
            itemEXP_3ValueText.text = $"{_value3}";
        }
    }
    

    
    /// <summary>
    /// Đổi item exp
    /// </summary>
    /// <param name="_value"> Giá trị tương ứng 1, 2, 3 <=> Small, Medium, Big </param>
    public void ChangeSelectItem(int _value) 
    {
        _amountUse = 0;
        panelActivity_1.SetActive(false);
        panelActivity_2.SetActive(false);
        panelActivity_3.SetActive(false);
        
        if (_selectItemCoin == _value)
        {
            _selectItemCoin = 0;
            _totalCoinCost = 0;
            _totalExpReceived = 0;
            decreaseAmountUseBtt.interactable = false;
            increaseAmountUseBtt.interactable = false;
        }
        else
        {
            _selectItemCoin = _value;
            switch (_value)
            {
                case 1: panelActivity_1.SetActive(true); break;
                case 2: panelActivity_2.SetActive(true); break;
                case 3: panelActivity_3.SetActive(true); break;
            }
        }
        IncreaseAmountUse(0);
    }

    
    /// <summary>
    /// Tăng số lượng sử dụng của item
    /// </summary>
    /// <param name="_value"> Giá trị tăng/giảm <=> 1/-1 </param>
    public void IncreaseAmountUse(int _value)
    {
        _amountUse += _value;
        
        decreaseAmountUseBtt.interactable = _amountUse > 0;
        switch (_selectItemCoin)
        {
            case 1:
                increaseAmountUseBtt.interactable = _amountUse < _smallExpValue;
                _totalCoinCost = _amountUse * smallExpBuff.UpgradeCost;
                _totalExpReceived = Mathf.FloorToInt(_amountUse * smallExpBuff.Value);
                break;
            
            case 2:
                increaseAmountUseBtt.interactable = _amountUse < _mediumExpValue;
                _totalCoinCost = _amountUse * mediumExpBuff.UpgradeCost;
                _totalExpReceived = Mathf.FloorToInt(_amountUse * mediumExpBuff.Value);
                break;
            
            case 3:
                increaseAmountUseBtt.interactable = _amountUse < _bigExpValue;
                _totalCoinCost = _amountUse * bigExpBuff.UpgradeCost;
                _totalExpReceived = Mathf.FloorToInt(_amountUse * bigExpBuff.Value);
                break;
        }

        DemoUpgradeValue();
        SetAmountUseText();
        SetTotalCoinCostText();
        GetStatsCharacter();
        SetTotalEXPReceived();
    }


    private void DemoUpgradeValue()
    {
        
    }
    private void SetAmountUseText() => amountUseText.text = $"{_amountUse}"; 
    private void SetTotalCoinCostText() => currencyText.text = $"{_currentCoin}/{_totalCoinCost}";
    private void SetTotalEXPReceived()
    {
        var _totalExpReceivedToStr = _totalExpReceived == 0 ? "" : $"+ {_totalExpReceived}";
        charEXPText.text = $"<size=29.5><color=green> {_totalExpReceivedToStr} </color></size>     {_currentExp}/{_nextExp}";
    }

}
