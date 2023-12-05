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
    
    [Tooltip("Các slot đang trang bị item")] 
    public Dictionary<int, ItemNameCode> slotEquippeds;
    
    [Tooltip("Dữ liệu toàn bộ Item của User")]
    public Dictionary<ItemNameCode, int> inventories;
    
    public event Action<int> OnCoinChangedEvent;
    
    
    public UserData() { }
    public UserData(string _username, int _coin)
    {
        username = _username;
        coin = _coin;
        slotEquippeds = new Dictionary<int, ItemNameCode>()
        {
            { 1, default },
            { 2, default },
            { 3, default },
            { 4, default }
        };
        inventories = new Dictionary<ItemNameCode, int>()
        {
            { ItemNameCode.POHealth , 5},
            { ItemNameCode.POStamina, 5},
            { ItemNameCode.EXPSmall, 10},
            { ItemNameCode.EXPMedium, 20},
            { ItemNameCode.EXPBig, 30},
            { ItemNameCode.JASliver1, 200},
        };
    }
    
    
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
    public int HasItemValue(ItemNameCode _itemCode) => inventories.TryGetValue(_itemCode, out var value) ? value : 0;
    
    
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
        if (!inventories.ContainsKey(_itemCode))
        {
            inventories.Add(_itemCode, _amount);
            return;
        }
        
        inventories[_itemCode] += _amount;
        if (inventories[_itemCode] > 0) return;
        inventories.Remove(_itemCode);
    }
    
    
    /// <summary>
    /// Gọi Event để gửi giá trị Coin đi
    /// </summary>
    public void SendEventCoinChaged() => OnCoinChangedEvent?.Invoke(_coin);
    
}
