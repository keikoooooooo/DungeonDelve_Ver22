using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


[Serializable]
public class UserData
{
    [SerializeField, JsonProperty]
    private string _username;
    [SerializeField, JsonProperty]
    private int _coin;

    public string username
    {
        get => _username;
        private set => _username = value;
    }
    public int coin
    {
        get => _coin;
        private set => _coin = value;
    }
    
    [Tooltip("Dữ liệu item mà người dùng sở hữu"), SerializeField, JsonProperty] 
    private Dictionary<ItemNameCode, int> _itemInventory;
    
    public event Action<int> OnCoinChangedEvent;
    
    
    public UserData() { }
    public UserData(string _username, int _coin)
    {
        username = _username;
        coin = _coin;
        _itemInventory = new Dictionary<ItemNameCode, int>()
        {
            { ItemNameCode.POHealth , 99},
            { ItemNameCode.PODamage, 99},
            { ItemNameCode.POStamina, 99},
            { ItemNameCode.PODefense, 99},
            { ItemNameCode.POIceResist, 99},
            { ItemNameCode.EXPSmall, 6},
            { ItemNameCode.EXPMedium, 7},
            { ItemNameCode.EXPBig, 8},
            { ItemNameCode.JASliver1, 9},
            { ItemNameCode.JASliver2, 10},
            { ItemNameCode.JASliver3, 11},
        };
    }

    
    /// <summary>
    /// Trả về danh sách các Item mà người dùng đang có
    /// </summary>
    /// <returns></returns>
    public Dictionary<ItemNameCode, int> GetItemInInventory() => _itemInventory;
    
    
    /// <summary>
    /// Check coin hiện tại có đủ mua vật phẩm không? 
    /// </summary>
    /// <param name="_coinNeeded"> Số Coin cần mua </param>
    /// <returns></returns>
    public bool IsCoinSufficientForPurchase(int _coinNeeded) => _coin >= _coinNeeded;

    
    /// <summary>
    /// Check trong inventory của User có vật phẩm theo nameCode không, nếu có trả về số lượng của vật phẩm
    /// </summary>
    /// <param name="_itemCode"> NameCode cần tìm </param>
    /// <param name="_value"> Giá trị trả về </param>
    /// <returns></returns>
    public int HasItemValue(ItemNameCode _itemCode) => _itemInventory.TryGetValue(_itemCode, out var value) ? value : 0;
    
    
    /// <summary>
    /// Tăng/Giảm Coin, nếu giá trị truyền vào là âm(-) sẽ DecreaseCoin
    /// </summary>
    /// <param name="_amount"> Số lượng tăng/giảm Coin</param>
    public void IncreaseCoin(int _amount)
    {
        _coin = Mathf.Clamp(_coin + _amount, 0, _coin + _amount);
        SendEventCoinChaged();
    }
    

    /// <summary>
    /// Tăng/Giảm value của Item, nếu giá trị truyền vào là âm(-) sẽ Decrease value của Item
    /// </summary>
    /// <param name="_amount"> Số lượng tăng/giảm Coin</param>
    public void IncreaseItemValue(ItemNameCode _itemCode, int _amount)
    {
        if (!_itemInventory.ContainsKey(_itemCode))
        {
            _itemInventory.Add(_itemCode, _amount);
            return;
        }
        
        _itemInventory[_itemCode] += _amount;
        if (_itemInventory[_itemCode] > 0) return;
        _itemInventory.Remove(_itemCode);
    }
    
    
    /// <summary>
    /// Gọi Event để gửi giá trị Coin đi
    /// </summary>
    public void SendEventCoinChaged() => OnCoinChangedEvent?.Invoke(_coin);
    
}
