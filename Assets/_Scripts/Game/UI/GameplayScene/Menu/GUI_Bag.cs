using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GUI_Bag : MonoBehaviour, IGUI
{
    [SerializeField] private Slot[] slots;
    public static event Action<Slot[]> OnItemChangedSlotEvent;
    
    
    [Space]
    [SerializeField] private GUI_Item itemPrefab;
    [SerializeField] private Transform itemContent;
    [Space]
    [SerializeField] private DropdownBar sortDropdown;
    
    
    // Variables
    private UserData _userData;
    private SO_GameItemData _gameItemData;
    private static ObjectPooler<GUI_Item> _poolItem;
    private readonly Dictionary<ItemCustom, int> _itemData = new();
    private readonly string PP_SortOption = "SortOptionIndex";

    
    private void OnEnable()
    {
        RegisterEvent();
    }
    private void OnDisable()
    {
        UnRegisterEvent();
    }
    
    private void RegisterEvent()
    {
        GUI_Manager.Add(this);
        GUIInputs.InputAction.UI.UseItem1.performed += UseItem1;
        GUIInputs.InputAction.UI.UseItem2.performed += UseItem2;
        GUIInputs.InputAction.UI.UseItem3.performed += UseItem3;
        GUIInputs.InputAction.UI.UseItem4.performed += UseItem4;
    }
    private void UnRegisterEvent()
    {
        foreach (var slot in slots)
        {
            slot.OnSelectSlotEvent.AddListener(OnDropItem);
        }
        
        GUI_Manager.Remove(this);
        GUIInputs.InputAction.UI.UseItem1.performed -= UseItem1;
        GUIInputs.InputAction.UI.UseItem2.performed -= UseItem2;
        GUIInputs.InputAction.UI.UseItem3.performed -= UseItem3;
        GUIInputs.InputAction.UI.UseItem4.performed -= UseItem4;
    }
    
    
    public void UseItem1(InputAction.CallbackContext _callback) => HandleUseItem(slots[0]);
    public void UseItem2(InputAction.CallbackContext _callback) => HandleUseItem(slots[1]);
    public void UseItem3(InputAction.CallbackContext _callback) => HandleUseItem(slots[2]);
    public void UseItem4(InputAction.CallbackContext _callback) => HandleUseItem(slots[3]);
    private void HandleUseItem(Slot _slot)
    {
        var _item = _slot.GetItem;
        if(_item == null) 
            return;

        var _itemNameCode = _item.GetItemCustom.code;
        _userData.IncreaseItemValue(_itemNameCode, -1);

        if (_userData.HasItemValue(_itemNameCode) <= 0)
        {
            PlayerPrefs.SetString(_slot.KeyPlayerPrefs, string.Empty);
            _slot.SetSlot(null);
        }
        
        GUI_Manager.UpdateGUIData();
    }

  
    public void GetRef(GameManager _gameManager)
    {
        _userData = _gameManager.UserData;
        _gameItemData = _gameManager.GameItemData;
        _poolItem = new ObjectPooler<GUI_Item>(itemPrefab, itemContent, _gameItemData.GameItemDatas.Count);
        
        InitNewSlot();
        UpdateData();
    }
    private void InitNewSlot()
    {
        for (var i = 0; i < slots.Length; i++)
        {
            slots[i].SetKeyText($"{i + 1}");
            slots[i].SetKeyPlayerPrefs($"SlotSave_{i + 1}");
            slots[i].OnSelectSlotEvent.AddListener(OnDropItem);
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
        
        sortDropdown.Dropdown.value = PlayerPrefs.GetInt(PP_SortOption, 0);
        sortDropdown.Dropdown.RefreshShownValue();
        OnSelectSortOption(sortDropdown.Dropdown.value);
    }
    public void OnSelectSortOption(int _value)
    {
        PlayerPrefs.SetInt(PP_SortOption, _value);
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
                sprite = guiItem.GetItemCustom.sprite,
                description = guiItem.GetItemCustom.description,
                type = guiItem.GetItemCustom.type,
                nameItem = guiItem.GetItemCustom.nameItem,
            };
            _itemData.Add(itemCustom, guiItem.GetItemValue);
        }
        
        SortItem(_itemData);
        LoadOldSlot();
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
    private void SortItem(IDictionary<ItemCustom, int> _data)
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
    private void LoadOldSlot()
    {
        foreach (var _slot in slots)
        {
            var _data = JsonUtility.FromJson<ItemCustom>(PlayerPrefs.GetString(_slot.KeyPlayerPrefs, null));
            if(_data == null)
            {
                _slot.SetSlot(null);
                continue;
            }
            _slot.SetSlot(GetGUIItem(_data.code));
        }
        OnItemChangedSlotEvent?.Invoke(slots);
    }
    public static GUI_Item GetGUIItem(ItemNameCode _itemNameCode)
    {
        GUI_Item _guiItem = null;
        foreach (var guiItem in _poolItem.List.Where(item => item.gameObject.activeSelf))
        {
            if (guiItem.GetItemCustom.code == _itemNameCode)
            {
                _guiItem = guiItem;
            }
        }
        return _guiItem;
    } 
    
    
    public void OnDropItem(Slot _slot, GUI_Item _item)
    {
        var _slotEmptyIdx = -1;
        var _sameSlotItem = -1;
        
        for (var i = 0; i < slots.Length; i++)
        {
            if (slots[i] == _slot)
            {
                _slotEmptyIdx = i;
            }
            if (slots[i].GetItem == _item)
            {
                _sameSlotItem = i;
            }
        }

        if (_sameSlotItem != -1)
        {
            PlayerPrefs.SetString( slots[_sameSlotItem].KeyPlayerPrefs, null);
            slots[_sameSlotItem].SetSlot(null);
        }
        
        slots[_slotEmptyIdx].SetSlot(_item);
        PlayerPrefs.SetString(slots[_slotEmptyIdx].KeyPlayerPrefs, _item != null ? JsonUtility.ToJson(_item.GetItemCustom) : string.Empty);
        
        OnItemChangedSlotEvent?.Invoke(slots);
    }
    
}
