using System.Collections.Generic;
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
        GetStats();
        
        decreaseAmountUseBtt.interactable = _amountUse > 0;
        switch (_selectItem)
        {
            case 1:
                increaseAmountUseBtt.interactable = _amountUse < _smallExpValue;
                _totalCoinCost = _amountUse * expSmallBuff.UpgradeCost;
                _totalExpReceived = Mathf.FloorToInt(_amountUse * expSmallBuff.Value);
                DemoProgress((int)expSmallBuff.Value * _value);
                break;
            
            case 2:
                increaseAmountUseBtt.interactable = _amountUse < _mediumExpValue;
                _totalCoinCost = _amountUse * expMediumBuff.UpgradeCost;
                _totalExpReceived = Mathf.FloorToInt(_amountUse * expMediumBuff.Value);
                DemoProgress((int)expMediumBuff.Value * _value);
                break;
            
            case 3:
                increaseAmountUseBtt.interactable = _amountUse < _bigExpValue;
                _totalCoinCost = _amountUse * expBigBuff.UpgradeCost;
                _totalExpReceived = Mathf.FloorToInt(_amountUse * expBigBuff.Value);
                DemoProgress((int)expBigBuff.Value * _value);
                break;
        }
        
        SetAmountUseText();
        SetLevelText();
        SetCoinText();
        SetEXPText();
    }

    
    private void DemoProgress(int _expIncrease)
    {
        // var missingValue = backProgressSliderBar.maxValue - backProgressSliderBar.value;
        // var remainingValue = _totalExpReceived - missingValue;
        //
        // var _totalExpReceivedTemp = _totalExpReceived - _currentExp;
        //
        // for (var i = _currentLevel; i < _upgradeData.defaultDatas.Count; i++)
        // {
        //     if (backProgressSliderBar.value + _expIncrease >= _upgradeData.defaultDatas[i].EXP)
        //     {
        //         _demoLevel++;
        //         backProgressSliderBar.maxValue = _upgradeData.GetNextEXP(_demoLevel);
        //         backProgressSliderBar.minValue = 0;
        //         backProgressSliderBar.value = remainingValue;
        //     }
        //     
        //     _totalExpReceivedTemp -= _expIncrease;
        //     if (_totalExpReceivedTemp <= 0)
        //     {
        //         break;
        //     }
        // }
        
        var missingValue = backProgressSliderBar.maxValue - backProgressSliderBar.value;
        var remainingValue = _totalExpReceived - missingValue;

        var findLv = _currentLevel;
        
        // Exp đang có, không tính exp cho lv tiếp theo của level hiện tại
        var hasExp = _upgradeData.GetTotalEXP(_currentLevel);
        // tính tổng exp sẽ cộng vào, tính từ lv tiếp theo từ level hiện tại cộng vào
        var totalIncreaseExp = _upgradeData.UpgradeData[_currentLevel - 1].EXP + _expIncrease;
        
        var _totalExpReceivedTemp = _upgradeData.UpgradeData[_currentLevel - 1].TotalExp + _expIncrease;
        for (var i = _currentLevel; i < _upgradeData.UpgradeData.Count; i++)
        {
            if (_totalExpReceivedTemp > _upgradeData.UpgradeData[i].TotalExp)
            {
                continue;
            }

            findLv = _upgradeData.UpgradeData[i].Level;
            break;
        }

        if (findLv != _currentLevel)
        {
            
        }
    }

    
    
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
