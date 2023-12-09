using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GUI_Bag : MonoBehaviour, IGUI
{
    [SerializeField] private GUI_Item itemPrefab;
    [SerializeField] private Transform itemContent;
    [Space]
    [SerializeField] private Slot slotPrefab;
    [SerializeField] private Transform slotContent;
    [Space]
    [SerializeField] private DropdownBar sortDropdown;
    
    // Variables
    private UserData _userData;
    private SO_GameItemData _gameItemData;
    
    private ObjectPooler<GUI_Item> _poolItem;
    private readonly Dictionary<ItemCustom, int> _itemData = new();
    
    private List<Slot> _slots = new();
    
    private void Awake() => GUI_Manager.Add(this);
    private void OnDestroy() => GUI_Manager.Remove(this);

    
    public void GetRef(GameManager _gameManager)
    {
        _userData = _gameManager.UserData;
        _gameItemData = _gameManager.GameItemData;

        Init();
        UpdateData();
    }
    private void Init()
    {
        _poolItem = new ObjectPooler<GUI_Item>(itemPrefab, itemContent, _gameItemData.GameItemDatas.Count);
        for (var i = 0; i < 4; i++)
        {
            var slot = Instantiate(slotPrefab, slotContent);
            slot.SetKeyText(i);
            _slots.Add(slot);
        }
    }

    
    public void UpdateData()
    {
        _poolItem.List.Where(item => item.gameObject.activeSelf).ToList().ForEach(x => x.Release());
        
        foreach (var (key, value) in _userData.GetItemInInventory())
        {
            if (!_gameItemData.GetItemCustom(key, out var itemCustom)) continue;
            var _item = _poolItem.Get();
            _item.SetItem(itemCustom, value);
            _item.SetValueText($"{value}");
        }
        
        OnSelectSortOption(0);
    }
    public void OnSelectSortOption(int _value)
    {
        sortDropdown.Dropdown.value = _value;
        sortDropdown.Dropdown.RefreshShownValue();
        
        var _guiItems = _poolItem.List.Where(item => item.gameObject.activeSelf).ToList();
        switch (_value)
        {
            case 0: case 1: 
                _guiItems = SortedByName(_guiItems, _value == 1);
                break;
            
            case 2: case 3: 
                _guiItems = SortedByRarity(_guiItems, _value == 3);
                break;
            
            case 4: case 5:
                _guiItems = SortedByQuantity(_guiItems, _value == 5);
                break;
        }

        _itemData.Clear();
        foreach (var guiItem in _guiItems)
        {
            var itemCustom = new ItemCustom
            {
                code = guiItem.GetItemCustom.code,
                ratity = guiItem.GetItemCustom.ratity,
                sprite = guiItem.GetItemCustom.sprite
            };
            _itemData.Add(itemCustom, guiItem.GetItemValue);
        }
        
        UpdateDataItem(_itemData);
    }
    private static List<GUI_Item> SortedByName(List<GUI_Item> _guiItems, bool _reverse)
    {
        _guiItems.Sort((item1, item2) => item1.GetItemCustom.code.CompareTo(item2.GetItemCustom.code));
        if (_reverse) 
            _guiItems.Reverse();
        
        return _guiItems;
    }
    private static List<GUI_Item> SortedByRarity(List<GUI_Item> _guiItems, bool _reverse)
    {
        _guiItems.Sort((item1, item2) => item1.GetItemCustom.ratity.CompareTo(item2.GetItemCustom.ratity));
        if (_reverse)
            _guiItems.Reverse();
        
        return _guiItems;
    }
    private static List<GUI_Item> SortedByQuantity(List<GUI_Item> _guiItems, bool _reverse)
    {        
        _guiItems.Sort((item1, item2) => item1.GetItemValue.CompareTo(item2.GetItemValue));
        if (_reverse)
            _guiItems.Reverse();
        
        return _guiItems;
    }
    
    private void UpdateDataItem(IDictionary<ItemCustom, int> _data)
    {
        foreach (var guiItem in _poolItem.List.Where(item => item.gameObject.activeSelf))
        {
            if(!_data.Any()) return;
            var keyValuePair = _data.First();
            guiItem.SetItem(keyValuePair.Key, keyValuePair.Value);
            guiItem.SetValueText($"{keyValuePair.Value}");
            _data.Remove(keyValuePair.Key);
        }
        
    }
    
    
}
