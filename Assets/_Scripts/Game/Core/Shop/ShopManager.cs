using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    [SerializeField] private MonoBehaviourID behaviourID;
    [field: SerializeField] public InteractiveUI interactiveUI { get; private set; }
    [field: SerializeField] public List<ShopItemSetup> shopData { get; private set; }
    private static readonly string _folderSave = "s_save";


    private void Start()
    {
        var _lastDay = DateTime.Parse(PlayerPrefs.GetString(behaviourID.GetID, DateTime.Now.ToString()));
        if (_lastDay <= DateTime.Today)
            LoadNewShopItem();
        else
            LoadOldShopItem();
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString(behaviourID.GetID, DateTime.Now.ToString());
        shopData.ForEach(x => FileHandle.Save(x, _folderSave, x.GetItemCode().ToString()));
    }
    private void LoadNewShopItem()
    {
        foreach (var shopItemSetup in shopData)
        {
            FileHandle.Delete(_folderSave, shopItemSetup.GetItemCode().ToString());
        }
    }
    private void LoadOldShopItem()
    {
        foreach (var shopItemSetup in shopData)
        {
            var _checkFile = FileHandle.Load(_folderSave, shopItemSetup.GetItemCode().ToString(), out ShopItemSetup _shopItem);
            if (!_checkFile) continue;
            shopItemSetup.SetCurrentPurchase(_shopItem.GetCurrentPurchase());
        }
    }
    
}
