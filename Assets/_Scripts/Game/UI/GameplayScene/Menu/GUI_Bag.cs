using UnityEngine;

public class GUI_Bag : MonoBehaviour, IPlayerRef
{
    [SerializeField] private Item itemPrefab;
    [SerializeField] private Transform itemContent;

    private UserData _userData;
    private PlayerController _player;
    private GameItemData _gameItemData;
    private ObjectPooler<Item> _poolItem;
    
    
    
    private void Awake() => PlayerRefGUIManager.Add(this);
    private void Start()
    {
        _userData = GameManager.Instance.UserData;
        _gameItemData = GameManager.Instance.GameItemData;
        _poolItem = new ObjectPooler<Item>(itemPrefab, itemContent, 50);
    }
    private void OnDestroy() => PlayerRefGUIManager.Remove(this);
    
    
    
    public void GetRef(PlayerController player)
    {
        _player = player;
        UpdateInventoryUser();
    }


    private void UpdateInventoryUser()
    {
        foreach (var inventory in _userData.inventories)
        {
            var _item = _poolItem.Get();
            _item.SetItem(_gameItemData.GetItemCustom(inventory.itemCode), inventory.value);
        }
    }
    
}
