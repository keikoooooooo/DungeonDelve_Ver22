using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class GUI_Bag : MonoBehaviour, IGUI
{
    [SerializeField] private Slot[] slots;
    [SerializeField] private DropdownBar sortDropdown;
    public static event Action<Slot[]> OnItemChangedSlotEvent;
    
    [Space]
    [SerializeField] private UI_Item itemPrefab;
    [SerializeField] private Transform itemContent;
    
    [BoxGroup("APPLY BUFF"), SerializeField] private SO_HealthBuff healthBuff;
    [BoxGroup("APPLY BUFF"), SerializeField] private SO_StaminaBuff staminaBuff;
    [BoxGroup("APPLY BUFF"), SerializeField] private SO_DamageBuff damageBuff;
    [BoxGroup("APPLY BUFF"), SerializeField] private SO_DefenseBuff defenseBuff;
    [BoxGroup("VFX BUFF"), SerializeField] private ParticleSystem effectBuff;
    [BoxGroup("VFX BUFF"), SerializeField] private ParticleSystem effectBuffLoop;
    
    // Variables
    private UserData _userData;
    private SO_GameItemData _gameItemData;
    private static ObjectPooler<UI_Item> _poolItem;
    private readonly Dictionary<ItemCustom, int> _itemData = new();
    private readonly string PP_SortOption = "SortOptionIndex";
    private string _accountID;
    private PlayerController _player;
    private ParticleSystem _effectBU, _effectBULoop;
    private Coroutine _useBuffCoroutine;
    
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
        GUI_Inputs.InputAction.UI.Item1.performed += UseItem1;
        GUI_Inputs.InputAction.UI.Item2.performed += UseItem2;
        GUI_Inputs.InputAction.UI.Item3.performed += UseItem3;
        GUI_Inputs.InputAction.UI.Item4.performed += UseItem4;
    }
    private void UnRegisterEvent()
    {
        GUI_Manager.Remove(this);
        GUI_Inputs.InputAction.UI.Item1.performed -= UseItem1;
        GUI_Inputs.InputAction.UI.Item2.performed -= UseItem2;
        GUI_Inputs.InputAction.UI.Item3.performed -= UseItem3;
        GUI_Inputs.InputAction.UI.Item4.performed -= UseItem4;
        
        foreach (var slot in slots)
        {
            slot.OnSelectSlotEvent.RemoveListener(OnDropItem);
        }
    }

    public void UseItem1(InputAction.CallbackContext _callback) => HandleUseItem(slots[0]);
    public void UseItem2(InputAction.CallbackContext _callback) => HandleUseItem(slots[1]);
    public void UseItem3(InputAction.CallbackContext _callback) => HandleUseItem(slots[2]);
    public void UseItem4(InputAction.CallbackContext _callback) => HandleUseItem(slots[3]);
    private void HandleUseItem(Slot _slot)
    {
        
        var _item = _slot.GetItem;
        if(_item == null)
        {
            Debug.Log("Non user item");
            return;
        }

        var _itemNameCode = _item.GetItemCustom.code;
        _userData.IncreaseItemValue(_itemNameCode, -1);
        if (_userData.HasItemValue(_itemNameCode) <= 0)
        {
            PlayerPrefs.SetString(_slot.KeyPlayerPrefs, string.Empty);
            _slot.SetSlot(null);
        }
        
        GUI_Manager.UpdateGUIData();
        UseBuffEffect(_itemNameCode);
    }
    private void UseBuffEffect(ItemNameCode _codeItemBuff)
    {
        AudioManager.PlayOneShot(FMOD_Events.Instance.buffEffect, transform.position);
        switch (_codeItemBuff)
        {
            case ItemNameCode.POHealth: 
                healthBuff.Apply(_player);
                _effectBU.gameObject.SetActive(true);
                _effectBU.Play();
                break;
            
            case ItemNameCode.POStamina: 
                staminaBuff.Apply(_player);
                _effectBU.gameObject.SetActive(true);
                _effectBU.Play();
                break;
            
            case ItemNameCode.PODamage: 
                damageBuff.Apply(_player);
                _effectBULoop.gameObject.SetActive(true);
                _effectBULoop.Play();
                
                if (_useBuffCoroutine != null)
                    StopCoroutine(_useBuffCoroutine);
                _useBuffCoroutine = StartCoroutine(UseBuffCoroutine(damageBuff.buffTimeOut));
                break;
            
            case ItemNameCode.PODefense: 
                defenseBuff.Apply(_player);
                _effectBULoop.gameObject.SetActive(true);
                _effectBULoop.Play();
                
                if (_useBuffCoroutine != null)
                    StopCoroutine(_useBuffCoroutine);
                _useBuffCoroutine = StartCoroutine(UseBuffCoroutine(defenseBuff.buffTimeOut));
                break;
        }
    }
    private IEnumerator UseBuffCoroutine(float _buffDeactivateTime)
    {
        yield return new WaitForSeconds(_buffDeactivateTime);
        _effectBULoop.Stop();
        _effectBULoop.gameObject.SetActive(false);
    }
  
    
    public void GetRef(GameManager _gameManager)
    {
        _player = _gameManager.Player;
        _userData = _gameManager.UserData;
        _gameItemData = _gameManager.GameItemData;
        _poolItem = new ObjectPooler<UI_Item>(itemPrefab, itemContent, _gameItemData.GameItemDatas.Count);
        _accountID = PlayFabController.Instance ? PlayFabController.Instance.userID : "";

        InitNewSlot();
        InitVFXBuff();
        UpdateData();
    }
    private void InitNewSlot()
    {
        for (var i = 0; i < slots.Length; i++)
        {
            slots[i].SetKeyText($"{i + 1}");
            slots[i].SetKeyPlayerPrefs($"{_accountID}:Slot_{i + 1}");
            slots[i].OnSelectSlotEvent.AddListener(OnDropItem);
        }
    }
    private void InitVFXBuff()
    {
        _effectBU = Instantiate(effectBuff, _player.transform);
        _effectBU.gameObject.SetActive(false);
        _effectBU.transform.localPosition = new Vector3(0, 1, 0);
        _effectBU.Stop();
        
        _effectBULoop = Instantiate(effectBuffLoop, _player.transform);
        _effectBULoop.gameObject.SetActive(false);
        _effectBULoop.transform.localPosition = new Vector3(0, 1, 0);
        _effectBULoop.Stop();
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
    private static List<UI_Item> SortedByName(List<UI_Item> _guiItems, bool _reverse)
    {
        _guiItems.Sort((item1, item2) => item1.GetItemCustom.code.CompareTo(item2.GetItemCustom.code));
        if (_reverse) 
            _guiItems.Reverse();
        
        return _guiItems;
    }
    private static List<UI_Item> SortedByRarity(List<UI_Item> _guiItems, bool _reverse)
    {
        _guiItems.Sort((item1, item2) => item1.GetItemCustom.ratity.CompareTo(item2.GetItemCustom.ratity));
        if (_reverse)
            _guiItems.Reverse();
        
        return _guiItems;
    }
    private static List<UI_Item> SortedByQuantity(List<UI_Item> _guiItems, bool _reverse)
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
    public static UI_Item GetGUIItem(ItemNameCode _itemNameCode)
    {
        UI_Item uiItem = null;
        foreach (var guiItem in _poolItem.List.Where(item => item.gameObject.activeSelf))
        {
            if (guiItem.GetItemCustom.code == _itemNameCode)
            {
                uiItem = guiItem;
            }
        }
        return uiItem;
    } 
    
    
    public void OnDropItem(Slot _slot, UI_Item _item)
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
