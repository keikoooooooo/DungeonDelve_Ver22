using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_WeaponUpgrade : MonoBehaviour, IGUI
{
    [Header("Infor")]
    [SerializeField] private TextMeshProUGUI weaLevelText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI currencyText;

    private UserData _userData;
    private SO_PlayerConfiguration _playerConfig;
    private bool isEventRegistered;
    private int _currentCoin;
    private int _selectItemCoin;

    
    private void Awake() => GUI_Manager.Add(this);
    private void OnDisable()
    {
        if(!isEventRegistered) return;
        _userData.OnCoinChangedEvent -= OnCoinChanged;
    }
    private void OnDestroy() => GUI_Manager.Remove(this);

    
    public void GetRef(UserData userData, SO_CharacterUpgradeData characterUpgradeData, SO_GameItemData gameItemData, PlayerController player)
    {
        _userData = userData;
        _playerConfig = player.PlayerConfig;

        UpdateData();
        
        if (isEventRegistered) return;
        isEventRegistered = true;
        _userData.OnCoinChangedEvent += OnCoinChanged;
    }
    
    public void UpdateData()
    {
        SetStatsText();
    }

    private void SetStatsText()
    {
        if (!_playerConfig) return;
        
        currencyText.text = $"{_userData.coin}/0";
        weaLevelText.text = $"Lv. {_playerConfig.WeaponLevel}";
        progressSlider.maxValue = 1;
        progressSlider.minValue = 0;
        progressSlider.value = 0;
    }
    
    private void OnCoinChanged(int _value)
    {
        _currentCoin = _value;
        currencyText.text = $"{_currentCoin}/{_selectItemCoin}";
    }


}
