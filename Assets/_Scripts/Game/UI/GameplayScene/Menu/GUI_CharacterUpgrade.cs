using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_CharacterUpgrade : MonoBehaviour, IGUI
{
    [Header("Stats Character")]
    [SerializeField] private TextMeshProUGUI charLevelText;
    [SerializeField] private TextMeshProUGUI charEXPText;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI itemQuantityText;
    [Space]
    [SerializeField] private Slider mainProgressSliderBar;
    [SerializeField] private Slider backProgressSliderBar;
    [Space, Header("Item Buff")]
    [SerializeField] private SO_ExperienceBuff expSmallBuff;
    [SerializeField] private SO_ExperienceBuff expMediumBuff;
    [SerializeField] private SO_ExperienceBuff expBigBuff;
    [Space, Header("Item View")]
    [SerializeField] private TextMeshProUGUI expSmallValueText;
    [SerializeField] private TextMeshProUGUI expMediumValueText;
    [SerializeField] private TextMeshProUGUI expBigValueText;
    [SerializeField] private Transform itemSlots;
    [SerializeField] private Image gradientItem;
    
    [Space, Header("Handler Button")]
    [SerializeField] private Button increaseAmountUseBtt;
    [SerializeField] private Button decreaseAmountUseBtt;
    [SerializeField] private Button cancelBtt;
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

    private int _currentLevel;       // lv hiện tại
    private int _currentExp;         // kinh nghiệm
    private int _nextExp;            // kinh nghiệm tối đa để nâng cấp lên lv tiếp theo
    
    // giá trị cộng thêm khi upgrade
    private int _increaseLevel;      
    private int _increaseEXP;       
    private int _increaseHP => 200 * _increaseLevel;
    private int _increasATK => 15 * _increaseLevel;        
    private int _increasDEF => 3 * _increaseLevel;         
    
    
    private bool _isEventRegistered;
    private UserData _userData;
    private PlayerController _player;
    private SO_CharacterUpgradeData _upgradeData;
    private SO_PlayerConfiguration _playerConfig;

    
    private void Awake()
    {
        GUI_Manager.Add(this);
    }
    private void OnEnable()
    {
        cancelBtt.onClick.AddListener(InitValue);
        cancelBtt.onClick.AddListener(UpdateData);
        upgradeBtt.onClick.AddListener(OnClickUpgradeButton);
        InitValue();
    }
    private void OnDisable()
    {
        cancelBtt.onClick.RemoveListener(InitValue);
        cancelBtt.onClick.RemoveListener(UpdateData);
        upgradeBtt.onClick.RemoveListener(OnClickUpgradeButton);
    }
    private void OnDestroy()
    {
        GUI_Manager.Remove(this);
        
        if(!_isEventRegistered) return;
        _userData.OnCoinChangedEvent -= OnCoinChanged;
    }
    

    public void InitValue()
    {
        _increaseLevel = 0;
        _increaseEXP = 0;
        _amountUse = 0;
        _selectItem = 0;
        _totalCoinCost = 0;
        _totalExpReceived = 0;
        increaseAmountUseBtt.interactable = false;
        decreaseAmountUseBtt.interactable = false;
        gradientItem.gameObject.SetActive(false);
    }
    
    public void GetRef(GameManager _gameManager)
    {
        _userData = _gameManager.UserData;
        _player = _gameManager.Player;
        _upgradeData = _gameManager.CharacterUpgradeData;
        _playerConfig = _gameManager.Player.PlayerConfig;
        
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
        SetItemQuantity();
        
        SetCoinText();
        SetEXPText();
        SetLevelText();
        SetAmountUseText();
        SetUpgradeStateButton();
    }
    private void GetStats()
    {
        if(!_playerConfig) return;
        _currentLevel = _playerConfig.GetLevel();
        _currentExp = _playerConfig.GetCurrentEXP();
        _nextExp = _upgradeData.GetNextEXP(_currentLevel);
        
        mainProgressSliderBar.maxValue = _nextExp;
        mainProgressSliderBar.minValue = 0;
        mainProgressSliderBar.value = _currentExp;
        
        backProgressSliderBar.maxValue = _nextExp;
        backProgressSliderBar.minValue = 0;
        backProgressSliderBar.value = _currentExp;
    }
    private void SetItemQuantity()
    {
        _smallExpValue = _userData.HasItemValue(ItemNameCode.EXPSmall);
        _mediumExpValue = _userData.HasItemValue(ItemNameCode.EXPMedium);
        _bigExpValue = _userData.HasItemValue(ItemNameCode.EXPBig);
        
        SetSmallExpValueText();
        SetMeidumExpValueText();
        SetBigExpValueText();
    }
    

    
    /// <summary>
    /// Đổi item exp
    /// </summary>
    /// <param name="_value"> Giá trị tương ứng 1, 2, 3 <=> Small, Medium, Big </param>
    public void OnSelectItemButton(int _value)
    {
        InitValue();
        
        _selectItem = _value;
        
        gradientItem.transform.SetParent(itemSlots.GetChild(_value - 1));
        gradientItem.transform.localPosition = Vector3.zero;
        gradientItem.gameObject.SetActive(true);
        
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
                increaseAmountUseBtt.interactable = _amountUse < _smallExpValue && _currentLevel <= SO_CharacterUpgradeData.levelMax;
                _totalCoinCost = _amountUse * expSmallBuff.UpgradeCost;
                _increaseEXP = (int)expSmallBuff.Value;
                break;
            
            case 2:
                increaseAmountUseBtt.interactable = _amountUse < _mediumExpValue && _currentLevel <= SO_CharacterUpgradeData.levelMax;
                _totalCoinCost = _amountUse * expMediumBuff.UpgradeCost;
                _increaseEXP = (int)expMediumBuff.Value;
                break;
            
            case 3:
                increaseAmountUseBtt.interactable = _amountUse < _bigExpValue && _currentLevel <= SO_CharacterUpgradeData.levelMax;
                _totalCoinCost = _amountUse * expBigBuff.UpgradeCost;
                _increaseEXP = (int)expBigBuff.Value;
                break;
        }
        
        _totalExpReceived = Mathf.FloorToInt(_amountUse * _increaseEXP);
        
        DemoProgress();
        SetAmountUseText();
        SetLevelText();
        SetCoinText();
        SetEXPText();
        SetUpgradeStateButton();
    }
    
    public void OnClickUpgradeButton()
    {
        SetStats();
        
        InitValue();
        UpdateData();
        
        GUI_Manager.UpdateGUIData();
    }
    
    
    private void SetStats()
    {
        var currentLevel = _playerConfig.GetLevel() + _increaseLevel;
        var currentHP = _playerConfig.GetHP() + _increaseHP;
        var currentATK = _playerConfig.GetATK() + _increasATK;
        var currentDEF = _playerConfig.GetDEF() + _increasDEF;
        
        UpgradeNoticeManager.Instance.SetLevelText($"Lv. {currentLevel}");
        CreateTextNotice("Max HP",$"{_playerConfig.GetHP()}", $"{currentHP}");
        CreateTextNotice("ATK",$"{_playerConfig.GetATK()}", $"{currentATK}");
        CreateTextNotice("DEF",$"{_playerConfig.GetDEF()}", $"{currentDEF}");
        UpgradeNoticeManager.Instance.EnableNotice();
        
        _userData.IncreaseCoin(-_totalCoinCost);
        _playerConfig.SetLevel(currentLevel);
        _playerConfig.SetCurrentEXP((int)backProgressSliderBar.value);
        _playerConfig.SetHP(currentHP);
        _playerConfig.SetATK(currentATK);
        _playerConfig.SetDEF(currentDEF);
        _player.InitStatus();
        
        switch (_selectItem)
        {
            case 1: _userData.IncreaseItemValue(ItemNameCode.EXPSmall, -_amountUse);  break;
            case 2: _userData.IncreaseItemValue(ItemNameCode.EXPMedium, -_amountUse); break;
            case 3: _userData.IncreaseItemValue(ItemNameCode.EXPBig, -_amountUse);    break;
        }
    }
    private void DemoProgress()
    {
        _increaseLevel = 0;
        if (_amountUse == 0)
        {
            backProgressSliderBar.maxValue = mainProgressSliderBar.maxValue;
            backProgressSliderBar.value = mainProgressSliderBar.value;
            return;
        }

        var hasExp = _upgradeData.GetTotalEXP(_currentLevel);
        // Từ tổng điểm kn vừa tìm + thêm kn hiện tại đang có + thêm kn của lượng item cấp
        var totalIncreaseExp = hasExp + _currentExp + _totalExpReceived;
        
        for (var i = _currentLevel - 1; i < _upgradeData.Data.Count; i++)
        {
            // Check tổng exp sẽ có và exp của từng mốc lv và chạy fill của slider để biết tiến trình
            if (totalIncreaseExp >= _upgradeData.Data[i].TotalExp) 
            {
                _increaseLevel++;
                backProgressSliderBar.maxValue = _upgradeData.Data[i + 1].EXP;
                backProgressSliderBar.value = _currentExp + (_totalExpReceived - _upgradeData.Data[i].TotalExp);
                continue;
            }

            // Nếu chạy xuống tới đây, tìm giá trị exp dư ra và kết thúc vòng for
            var _remainingExp = _upgradeData.Data[i].TotalExp - totalIncreaseExp;
            backProgressSliderBar.value = _upgradeData.GetNextEXP(_increaseLevel + _currentLevel) - _remainingExp;
            break;
        }
    }
    

    private static void CreateTextNotice(string _title, string _oldValue, string _newValue) => UpgradeNoticeManager.Instance.CreateTextNotice(_title, _oldValue, _newValue);
    private void SetUpgradeStateButton() => upgradeBtt.interactable = _amountUse != 0 && _coin >= _totalCoinCost;
    // Set UGUI Text
    private void SetSmallExpValueText() => expSmallValueText.text = _smallExpValue == 0 ? $"<color=red>{_smallExpValue}</color>" : $"<color=white>{_smallExpValue}</color>";
    private void SetMeidumExpValueText() => expMediumValueText.text = _mediumExpValue == 0 ? $"<color=red>{_mediumExpValue}</color>" : $"<color=white>{_mediumExpValue}</color>";
    private void SetBigExpValueText() => expBigValueText.text  = _bigExpValue == 0 ? $"<color=red>{_bigExpValue}</color>" : $"<color=white>{_bigExpValue}</color>";
    private void SetAmountUseText() => itemQuantityText.text = $"{_amountUse}"; 
    private void SetCoinText() => currencyText.text = $"{_coin}/{_totalCoinCost}";
    private void SetLevelText()
    {
        var _demoLvToStr = _increaseLevel == 0 ? "" : $"+ {_increaseLevel}";
        charLevelText.text = $"Lv. {_currentLevel}  <size=35><color=#FFD900> {_demoLvToStr}</color></size>";
    }
    private void SetEXPText()
    {
        var _totalExpReceivedToStr = _totalExpReceived == 0 ? "" : $"+ {_totalExpReceived}";
        charEXPText.text = $"<size=29.5><color=#FFD900> {_totalExpReceivedToStr} </color></size>  {_currentExp}/{_nextExp}";
    }

}
