using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_CharacterUpgrade : MonoBehaviour, IGUI
{
    [Header("Infor")]
    [SerializeField] private TextMeshProUGUI charLevelText;
    [SerializeField] private TextMeshProUGUI charEXPText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI itemQuantityText;
    [Space]
    [SerializeField] private Slider mainProgressSliderBar;
    [SerializeField] private Slider backProgressSliderBar;
    [Space]
    [SerializeField] private SO_ExperienceBuff expSmallBuff;
    [SerializeField] private SO_ExperienceBuff expMediumBuff;
    [SerializeField] private SO_ExperienceBuff expBigBuff;
    [Space]
    [SerializeField] private TextMeshProUGUI expSmallValueText;
    [SerializeField] private TextMeshProUGUI expMediumValueText;
    [SerializeField] private TextMeshProUGUI expBigValueText;
    [SerializeField] private GameObject panelActivity_1;
    [SerializeField] private GameObject panelActivity_2;
    [SerializeField] private GameObject panelActivity_3;
    
    [Space]
    [SerializeField] private Button increaseAmountUseBtt;
    [SerializeField] private Button decreaseAmountUseBtt;
    [SerializeField] private Button upgradeBtt;

    // Variables
    private int _coin;               // coin đang sở hữu
    private int _amountUse;          // số lượng item dùng
    private int _totalCoinCost;      // tổng số coin cần để upgrade
    private int _totalExpReceived;   // tổng exp nhận được
    private int _selectItem;         // loại item đang chọn (1, 2, 3 <=> Exp: Small, Medium, Big )
    
    private int _smallExpValue;      // số lượng từng item đang có
    private int _mediumExpValue;    
    private int _bigExpValue;

    private int _demoLevel;          // hiển thị lv mới(demo) sau khi upgrade
    private int _currentLevel;       // lv hiện tại của nv
    private int _currentExp;         // kinh nghiệm hiện tại của nv
    private int _nextExp;            // kinh nghiệm tối đa để nâng cấp lên lv tiếp theo
    
    private bool _isEventRegistered;
    private UserData _userData;
    private SO_CharacterUpgradeData _upgradeData;
    private SO_PlayerConfiguration _playerConfig;
    
    
    private void Awake()
    {
        GUI_Manager.Add(this);
    }
    private void OnEnable() => InitValue();
    private void OnDestroy()
    {
        GUI_Manager.Remove(this);
        if(!_isEventRegistered) return;
        _userData.OnCoinChangedEvent -= OnCoinChanged;
    }
    
    public void InitValue()
    {
        _demoLevel = 0;
        _amountUse = 0;
        _selectItem = 0;
        _totalCoinCost = 0;
        _totalExpReceived = 0;
        panelActivity_1.SetActive(false);
        panelActivity_2.SetActive(false);
        panelActivity_3.SetActive(false);
        increaseAmountUseBtt.interactable = false;
        decreaseAmountUseBtt.interactable = false;
        
        SetLevelText();
        SetAmountUseText();
        SetCoinText();
        SetEXPText();
        UpdateProgress();
        SetUpgradeStateButton();
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
        _coin = _value;
        SetCoinText();
    }
    
    public void UpdateData()
    {
        GetStats();
        UpdateProgress();
        SetItemQuantity();
        
        SetCoinText();
        SetEXPText();
        SetLevelText();
        SetUpgradeStateButton();
    }
    
    private void GetStats()
    {
        if(!_playerConfig) return;
        _currentLevel = _playerConfig.Level;
        _currentExp = _playerConfig.CurrentEXP;
        _nextExp = _upgradeData.GetNextEXP(_currentLevel);
    }
    private void UpdateProgress()
    {
        mainProgressSliderBar.maxValue = _nextExp;
        mainProgressSliderBar.minValue = 0;
        mainProgressSliderBar.value = _currentExp;
        
        backProgressSliderBar.maxValue = _nextExp;
        backProgressSliderBar.minValue = 0;
        backProgressSliderBar.value = _currentExp;
    }
    private void SetItemQuantity()
    {
        if (_userData.HasItemValue(ItemNameCode.EXPSmall, out var _value1))
        {
            _smallExpValue = _value1;
            SetSmallExpValueText();
        }
        if (_userData.HasItemValue(ItemNameCode.EXPMedium, out var _value2))
        {
            _mediumExpValue = _value2;
            SetMeidumExpValueText();
        }
        if (_userData.HasItemValue(ItemNameCode.EXPBig, out var _value3))
        {
            _bigExpValue = _value3;
            SetBigExpValueText();
        }
    }
    

    
    /// <summary>
    /// Đổi item exp
    /// </summary>
    /// <param name="_value"> Giá trị tương ứng 1, 2, 3 <=> Small, Medium, Big </param>
    public void OnSelectItemButton(int _value)
    {
        InitValue();
        
        _selectItem = _value;
        panelActivity_1.SetActive(_value == 1);
        panelActivity_2.SetActive(_value == 2);
        panelActivity_3.SetActive(_value == 3);
        
        OnIncreaseAmountItemButton(0);
    }
    
    /// <summary>
    /// Tăng số lượng sử dụng của item
    /// </summary>
    /// <param name="_value"> Giá trị tăng/giảm <=> 1/-1 </param>
    public void OnIncreaseAmountItemButton(int _value)
    {
        _amountUse += _value;
        
        decreaseAmountUseBtt.interactable = _amountUse > 0;
        switch (_selectItem)
        {
            case 1:
                increaseAmountUseBtt.interactable = _amountUse < _smallExpValue;
                _totalCoinCost = _amountUse * expSmallBuff.UpgradeCost;
                _totalExpReceived = Mathf.FloorToInt(_amountUse * expSmallBuff.Value);
                break;
            
            case 2:
                increaseAmountUseBtt.interactable = _amountUse < _mediumExpValue;
                _totalCoinCost = _amountUse * expMediumBuff.UpgradeCost;
                _totalExpReceived = Mathf.FloorToInt(_amountUse * expMediumBuff.Value);
                break;
            
            case 3:
                increaseAmountUseBtt.interactable = _amountUse < _bigExpValue;
                _totalCoinCost = _amountUse * expBigBuff.UpgradeCost;
                _totalExpReceived = Mathf.FloorToInt(_amountUse * expBigBuff.Value);
                break;
        }
        
        GetStats();
        DemoProgress();
        SetAmountUseText();
        SetLevelText();
        SetCoinText();
        SetEXPText();
        SetUpgradeStateButton();
    }
    
    public void OnClickUpgradeButton()
    {
        _userData.IncreaseCoin(-_totalCoinCost); 
        _playerConfig.Level += _demoLevel;

        switch (_selectItem)
        {
            case 1: _userData.IncreaseItem(ItemNameCode.EXPSmall, -_amountUse); break;
            case 2: _userData.IncreaseItem(ItemNameCode.EXPMedium, -_amountUse); break;
            case 3: _userData.IncreaseItem(ItemNameCode.EXPBig, -_amountUse); break;
        }
        
        _amountUse = 0;
        SetAmountUseText();
        UpdateData();
    }
    

    private void DemoProgress()
    {
        var hasExp = _upgradeData.GetTotalEXP(_currentLevel);
        var totalIncreaseExp = hasExp + _currentExp + _totalExpReceived;
        
        for (var i = _currentLevel - 1; i < _upgradeData.DataList.Count; i++)
        {
            if (i >= 1)
            {
                backProgressSliderBar.minValue = 0;
                backProgressSliderBar.maxValue = _upgradeData.DataList[i].TotalExp;
                backProgressSliderBar.value = totalIncreaseExp;
            }
            
            if (totalIncreaseExp >= _upgradeData.DataList[i].TotalExp) continue;
            _demoLevel = _amountUse != 0 ? _upgradeData.DataList[i].Level - 1 : 0;
            break;
        }
        
        Debug.Log("Level new upgrade +" + _demoLevel);

        if (_amountUse == 0)
        {
            UpdateProgress();
        }
    }



    private void SetUpgradeStateButton() => upgradeBtt.interactable = _amountUse != 0 && _coin >= _totalCoinCost;
    // Set UGUI Text
    private void SetSmallExpValueText() => expSmallValueText.text = $"{_smallExpValue}";
    private void SetMeidumExpValueText() => expMediumValueText.text = $"{_mediumExpValue}";
    private void SetBigExpValueText() => expBigValueText.text = $"{_bigExpValue}";
    private void SetAmountUseText() => itemQuantityText.text = $"{_amountUse}"; 
    private void SetCoinText() => currencyText.text = $"{_coin}/{_totalCoinCost}";
    private void SetLevelText()
    {
        var _demoLvToStr = _demoLevel == 0 ? "" : $"+ {_demoLevel}";
        charLevelText.text = $"Lv. {_currentLevel}   <color=green> {_demoLvToStr} </color>";
    }
    private void SetEXPText()
    {
        var _totalExpReceivedToStr = _totalExpReceived == 0 ? "" : $"+ {_totalExpReceived}";
        charEXPText.text = $"<size=29.5><color=green> {_totalExpReceivedToStr} </color></size>     {_currentExp}/{_nextExp}";
    }

}
