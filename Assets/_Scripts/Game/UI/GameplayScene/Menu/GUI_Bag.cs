using System.Collections.Generic;
using UnityEngine;

public class GUI_Bag : MonoBehaviour, IGUI
{
    [SerializeField] private Item itemPrefab;
    [SerializeField] private Transform itemContent;
    [Space]
    [SerializeField] private Slot slotPrefab;
    [SerializeField] private Transform slotContent;
    
    // Variables
    private UserData _userData;
    private SO_GameItemData _gameItemData;
    private ObjectPooler<Item> _poolItem;
    private List<Slot> _slots = new();
    private readonly List<Item> _items = new();

    private void Awake() => GUI_Manager.Add(this);
    private void OnDestroy() => GUI_Manager.Remove(this);


    

    
    public void GetRef(UserData userData, SO_CharacterUpgradeData characterUpgradeData, SO_GameItemData gameItemData, PlayerController player)
    {
        _userData = userData;
        _gameItemData = gameItemData;

        Init();
        UpdateData();
    }
    private void Init()
    {
        _poolItem = new ObjectPooler<Item>(itemPrefab, itemContent, _gameItemData.GameItemDatas.Count);
        foreach (var item in _poolItem.Pool)
        {
            _items.Add(item);
        }
        for (var i = 0; i < 4; i++)
        {
            var slot = Instantiate(slotPrefab, slotContent);
            slot.SetKeyText(i);
            _slots.Add(slot);
        }
    }

    public void UpdateData()
    {
        _items.ForEach(x => x.Release());
        foreach (var inventory in _userData.inventories)
        {
            if (!_gameItemData.GetItemCustom(inventory.Key, out var itemCustom)) continue;
            var _item = _poolItem.Get();
            _item.SetItem(itemCustom, inventory.Value);
        }
    }
    
}
