using System.Collections.Generic;
using System.Linq;
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


    

    
    public void GetRef(GameManager _gameManager)
    {
        _userData = _gameManager.UserData;
        _gameItemData = _gameManager.GameItemData;

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
        foreach (var item in _items.Where(item => item.gameObject.activeSelf))
        {
            item.Release();
        }
        foreach (var (key, value) in _userData.inventories)
        {
            if (!_gameItemData.GetItemCustom(key, out var itemCustom)) continue;
            var _item = _poolItem.Get();
            _item.SetItem(itemCustom, value);
            _item.SetValueText(value);
        }
        
    }
    
}
