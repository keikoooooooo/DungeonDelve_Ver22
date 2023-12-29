using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private MonoBehaviourID behaviourID;
    [field: SerializeField] public InteractiveUI interactiveUI { get; private set; }
    public static List<ShopItemSetup> ShopData { get; } = new();
    private static readonly string _folderSave = "s_save";


    private void Start()
    {
        var _data = Resources.LoadAll<ShopItemSetup>("Shop Custom");
        ShopData.Clear();
        foreach (var shopItemSetup in _data)
        {
            ShopData.Add(Instantiate(shopItemSetup));
        }
        
        var _lastDay = DateTime.Parse(PlayerPrefs.GetString(behaviourID.GetID, DateTime.MinValue.ToString()));
        if (_lastDay < DateTime.Today)
            LoadNewShopItem();
        else
            LoadOldShopItem();

        SortShopItemData();
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(behaviourID.GetID, DateTime.Now.ToString());
        foreach (var _itemSetup in ShopData)
        {
            var purchaseData = new ShopItemSetup.ItemPurchaseData(_itemSetup.GetPurchaseCurrent());
            FileHandle.Save(purchaseData, _folderSave, _itemSetup.GetID());
        }
    }
    private static void LoadNewShopItem()
    {
        foreach (var shopItemSetup in ShopData)
        {
            shopItemSetup.SetPurchaseCurrent(0);
            FileHandle.Delete(_folderSave, shopItemSetup.GetID());
        }
    }
    private static void LoadOldShopItem()
    {
        foreach (var _itemPurchase in ShopData)
        {
            var _checkFile = FileHandle.Load(_folderSave, _itemPurchase.GetID(), out ShopItemSetup.ItemPurchaseData _currentPurchaseData);
            if (!_checkFile) continue;
            _itemPurchase.SetPurchaseCurrent(_currentPurchaseData.currentValue);
        }
    }
    public static void SortShopItemData()
    {
        ShopData.Sort((x1, x2) =>
        {
            var compareCanBuyItem = x2.CanBuyItem.CompareTo(x1.CanBuyItem);
            return compareCanBuyItem == 0 ? x1.GetRarity().CompareTo(x2.GetRarity()) : compareCanBuyItem;
        });
    }
    
#if UNITY_EDITOR
    [ContextMenu("Reset Quest Key")]
    private void OnResetQuestKey()
    {
        if (!PlayerPrefs.HasKey(behaviourID.GetID)) return;
        PlayerPrefs.DeleteKey(behaviourID.GetID);
        Debug.Log("Delete PlayerPrefs Key Success !. \nKey: " + behaviourID.GetID);
    }
#endif
    
}
