using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_WeaponUpgrade : MonoBehaviour, IGUI
{
    [SerializeField] private TextMeshProUGUI weaLevelText;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI currencyText;
    [Space]
    [SerializeField] private Item itemPrefab;
    [SerializeField] private Transform slotItems;

    [Space]
    [SerializeField] private Button upgradeBtt;

    private ObjectPooler<Item> _poolItem;
    private List<Item> _items = new();
    private UserData _userData;
    private SO_PlayerConfiguration _playerConfig;
    private SO_RequiresWeaponUpgradeConfiguration _weaponUpgradeConfig;
    private SO_GameItemData _gameItemData;
    private bool isEventRegistered;
    private int _coin;
    private int _weaponLevel;
    private bool _canUpgrade;
    
    
    private void Awake() => GUI_Manager.Add(this);
    private void OnDisable()
    {
        if(!isEventRegistered) return;
        _userData.OnCoinChangedEvent -= OnCoinChanged;
    }
    private void OnDestroy() => GUI_Manager.Remove(this);

    
    public void GetRef(GameManager _gameManager)
    {
        _userData = _gameManager.UserData;
        _playerConfig = _gameManager.Player.PlayerConfig;
        _gameItemData = _gameManager.GameItemData;
        _weaponUpgradeConfig = _gameManager.Player.PlayerData.WeaponUpgradeConfig;
        
        
        if (!isEventRegistered)
        {
            isEventRegistered = true;
            _userData.OnCoinChangedEvent += OnCoinChanged;
        }
        
        Init();
        UpdateData();
    }

    private void OnCoinChanged(int _value)
    {
        _coin = _value;
        SetCoinText();
    }
    private void Init()
    {
        _poolItem = new ObjectPooler<Item>(itemPrefab, slotItems, _gameItemData.GameItemDatas.Count);
        foreach (var item in _poolItem.Pool)
        {
            _items.Add(item);
        }
    }
    
    
    public void UpdateData()
    {
        GetStats();
        SetItemRequires();
        
        SetWeaponLevelText();
        SetUpgradeStateButton();
    }

    private void GetStats()
    {
        if (!_playerConfig) return;

        _weaponLevel = _playerConfig.GetWeaponLevel();
        progressSlider.maxValue = 1;
        progressSlider.minValue = 0;
        progressSlider.value = 0;
    }

    private void SetItemRequires()
    {
        if(!_weaponUpgradeConfig) return;
        
        foreach (var item in _items.Where(x => x.gameObject.activeSelf))
        {
            item.Release();
        }
        var _requiresConfig = _weaponUpgradeConfig.RequiresDatas[_weaponLevel - 1];
        foreach (var _requiresItem in _requiresConfig.requiresItem)
        {
            if (!_gameItemData.GetItemCustom(_requiresItem.code, out var _itemCustom)) continue;
            
            var item = _poolItem.Get();
            item.SetItem(_itemCustom, _requiresItem.value);
            item.SetValueText(_userData.HasItemValue(_itemCustom.code, out var _value)
                ? $"{_value} / {_requiresItem.value}"
                : $"{0} / {_requiresItem.value}");

            if (_value < _requiresItem.value) _canUpgrade = false;
        }
        
    }



    private void SetUpgradeStateButton() => upgradeBtt.interactable = _canUpgrade;
    private void SetWeaponLevelText() => weaLevelText.text = $"Lv. {_weaponLevel}";
    private void SetCoinText() => currencyText.text = $"{_coin}/{_weaponUpgradeConfig.RequiresDatas[_weaponLevel - 1].coinCost}";
    
}
