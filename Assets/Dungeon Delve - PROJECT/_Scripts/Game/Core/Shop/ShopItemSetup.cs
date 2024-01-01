using System;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Create Item Purchase", fileName = "Purchase_")]
public class ShopItemSetup : ScriptableObject
{
    [Serializable]
    public class ItemPurchaseData
    {
        [field: SerializeField] public int currentValue { get; private set; }
        public ItemPurchaseData() { }
        public ItemPurchaseData(int _currentValue)
        {
            currentValue = _currentValue;
        }
    }
    
    [SerializeField, ReadOnly] private string id;
    [SerializeField, Tooltip("Loại Item")] private ItemNameCode itemNameCode;
    [SerializeField, ReadOnly, Tooltip("Số lượng mua hiện tại trong ngày")] private int purchaseCurrent;
    [SerializeField, Tooltip("Số lượng được mua tối đa trong 1 ngày")] private int purchaseMax;
    [SerializeField, Tooltip("Giá bán")] private int price;
    [SerializeField, Tooltip("Số lượng vật phẩm nhận được")] private int quantityReceive;
    [SerializeField, ReadOnly] private ItemRarity itemRarity;
    public bool CanBuyItem => purchaseCurrent < purchaseMax;
    
    
    // GETTER
    public string GetID() => id;
    public ItemNameCode GetItemCode() => itemNameCode;
    public int GetPurchaseMax() => purchaseMax;
    public int GetPurchaseCurrent() => purchaseCurrent;
    public int GetPrice() => price;
    public int GetQuantityReceive() => quantityReceive;
    public ItemRarity GetRarity() => itemRarity;
    
    // SETTER
    public void SetID(string _value) => id = _value; 
    public void SetItemCode(ItemNameCode _itemNameCode) => itemNameCode = _itemNameCode;
    public void SetPurchaseMax(int _value) => purchaseMax = _value;
    public void SetPurchaseCurrent(int _value) => purchaseCurrent = _value;
    public void SetPrice(int _value) => price = _value;
    public void SetQuantityReceive(int _value) => quantityReceive = _value;
    public void SetRarity(ItemRarity _itemRarity) => itemRarity = _itemRarity;
    public void Purchase(int _value) => purchaseCurrent = Mathf.Clamp(purchaseCurrent + _value, 0, purchaseMax);
    
}
