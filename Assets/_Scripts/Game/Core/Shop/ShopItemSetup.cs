using System;
using UnityEngine;

[Serializable]
public class ShopItemSetup
{
    [SerializeField, Tooltip("Loại Item")] private ItemNameCode itemNameCode;
    [SerializeField, Tooltip("Giá bán")] private int price;
    [SerializeField, Tooltip("Số lượng vật phậm nhận được")] private int quantityReceive;
    [SerializeField, Tooltip("Số lượng được mua tối đa trong 1 ngày")] private int purchaseMax;
    [field: SerializeField, HideInInspector] public int currentPurchase { get; private set; }
    
    public bool CanBuyItem => currentPurchase < purchaseMax;
    
    public ItemNameCode GetItemCode() => itemNameCode;
    public int GetPrice() => price;
    public int GetQuantityReceive() => quantityReceive;
    public int GetPurchaseMax() => purchaseMax;

    public void BuyItem() => currentPurchase = Mathf.Clamp(currentPurchase + 1, 0, purchaseMax);
    public void SetCurrentPurchase(int _value) => currentPurchase = _value;
    public int GetCurrentPurchase() => currentPurchase;
}
