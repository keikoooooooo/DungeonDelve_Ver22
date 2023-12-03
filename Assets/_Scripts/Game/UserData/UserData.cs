using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class UserData
{
    public string username { get; private set; }
    public int coin { get; private set; }
    
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
            { ItemNameCode.EXPSmall, 100},
            { ItemNameCode.EXPMedium, 200},
            { ItemNameCode.EXPBig, 500},
        };
    }
    
    
    /// <summary>
    /// Check coin hiện tại có đủ mua vật phẩm không? 
    /// </summary>
    /// <param name="_coinNeeded"> Số Coin cần mua </param>
    /// <returns></returns>
    public bool IsCoinSufficientForPurchase(int _coinNeeded) => coin >= _coinNeeded;

    
    /// <summary>
    /// Check trong inventory của User có vật phẩm theo nameCode không, nếu có trả về số lượng của vật phẩm
    /// </summary>
    /// <param name="_itemCode"> NameCode cần tìm </param>
    /// <param name="_value"> Giá trị trả về </param>
    /// <returns></returns>
    public bool HasItemValue(ItemNameCode _itemCode, out int _value)
    {
        _value = 0;
        if (inventories.TryGetValue(_itemCode, out var value))
        {
            _value = value;
        }
        return _value != 0;
    }
    
    
    /// <summary>
    /// Tăng/Giảm Coin, nếu giá trị truyền vào là âm(-) sẽ DecreaseCoin
    /// </summary>
    /// <param name="_amount"> Số lượng tăng/giảm Coin</param>
    public void IncreaseCoin(int _amount)
    {
        coin = Mathf.Clamp(coin + _amount, 0, coin + _amount);
        SendEventCoinChaged();
    }
    

    /// <summary>
    /// Tăng/Giảm value của Item, nếu giá trị truyền vào là âm(-) sẽ Decrease value của Item
    /// </summary>
    /// <param name="_amount"> Số lượng tăng/giảm Coin</param>
    public void IncreaseItem(ItemNameCode _itemCode, int _amount)
    {
        if (inventories.ContainsKey(_itemCode))
        {
            inventories[_itemCode] += _amount;
        }
    }
    
    
    /// <summary>
    /// Gọi Event để gửi giá trị Coin đi
    /// </summary>
    public void SendEventCoinChaged() => OnCoinChangedEvent?.Invoke(coin);
    
}
