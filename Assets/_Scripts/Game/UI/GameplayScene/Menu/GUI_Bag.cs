using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    private readonly List<GUI_Item> _guiItems = new ();
    private readonly Dictionary<ItemCustom, int> _guiItemsTemp = new ();
    
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
        ReleaseAllGUIItem();
        _guiItems.Clear();
        
        foreach (var (key, value) in _userData.GetItemInInventory())
        {
            if (!_gameItemData.GetItemCustom(key, out var itemCustom)) continue;
            var _item = _poolItem.Get();
            _item.SetItem(itemCustom, value);
            _item.SetValueText($"{value}");
            _guiItems.Add(_item);
        }
    }
    
    private void ReleaseAllGUIItem() => _poolItem.List.Where(item => item.gameObject.activeSelf).ToList().ForEach(x => x.Release());

    public void OnSelectSortOption(int _value)
    {
        switch (_value)
        {
            case 0:
            case 1: 
                SortedByName(_value == 1); break;
            
            case 2: 
            case 3: 
                SortedByRarity(_value == 3); break;
            
            case 4:
            case 5:
                SortedByQuantity(_value == 5); break;
        }
    }
    private void SortedByName(bool _reverse)
    {
        _guiItems.Sort((item, guiItem) => item.GetNameCode.CompareTo(guiItem.GetNameCode));
        if (_reverse) 
            _guiItems.Reverse();
        
        SaveItemTemp();
        CreateItem();
    }
    private void SortedByRarity(bool _reverse)
    { 
        _guiItems.Sort((item, guiItem) => item.GetRarity.CompareTo(guiItem.GetRarity));
        if (_reverse) 
            _guiItems.Reverse();
        
        SaveItemTemp();
        CreateItem();
    }
    private void SortedByQuantity(bool _reverse)
    {        
        _guiItems.Sort((item, guiItem) => item.GetItemValue.CompareTo(guiItem.GetItemValue));
        if (_reverse)
            _guiItems.Reverse();
        
        SaveItemTemp();
        CreateItem();
    }

    private void SaveItemTemp()
    {
        _guiItemsTemp.Clear();
        foreach (var guiItem in _guiItems)
        {
            _guiItemsTemp.Add(guiItem.GetItemCustom, guiItem.GetItemValue);
        }
    }
    private void CreateItem()
    {
        _guiItems.Clear();
        ReleaseAllGUIItem();
        foreach (var keyValuePair in _guiItemsTemp)
        {
            var _item = _poolItem.Get();
            _item.SetItem(keyValuePair.Key, keyValuePair.Value);
            _item.SetValueText($"{keyValuePair.Value}");
            _guiItems.Add(_item);
        }
    }
    
    
}
