using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardManager : Singleton<RewardManager>
{
    [SerializeField] private SO_GameItemData gameItemData;
    [Space]
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private ItemDrop _itemDropCommonPrefab;
    [SerializeField] private ItemDrop _itemDropUnCommonPrefab;
    [SerializeField] private ItemDrop _itemDropRarePrefab;
    [SerializeField] private ItemDrop _itemDropEpicPrefab;
    [SerializeField] private ItemDrop _itemDropLegendaryPrefab;
    
    private UserData _userData;
    private PlayerController _player;
    private ObjectPooler<Coin> _poolCoin;
    private ObjectPooler<ItemDrop> _poolItemCommon;
    private ObjectPooler<ItemDrop> _poolItemUnCommon;
    private ObjectPooler<ItemDrop> _poolItemRare;
    private ObjectPooler<ItemDrop> _poolItemEpic;
    private ObjectPooler<ItemDrop> _poolItemLegendary;

    
    // tạo 1 danh sách phẩn thưởng gồm các vật phẩm đang ở gần user và chờ user nhấn input để nhận
    private readonly Dictionary<ItemDrop, RewardSetup.ItemReward> _itemRewardsData = new(); 
    
    // tạo hàng chờ phần thưởng của coin, và coin di chuyển tới vị trí player sẽ tự động nhận thưởng
    private readonly Queue<RewardSetup.ItemReward> _coinRewardData = new();
    
    
    private void Start()
    {
        Init();
        GetRef();
    }
    private void Update()
    {
        HandleInput();
    }
    
    
    private void Init()
    {
        _poolCoin = new ObjectPooler<Coin>(coinPrefab, transform, 30);
        _poolCoin.List.ForEach(coin => coin.SetPlayer(_player));

        _poolItemCommon = new ObjectPooler<ItemDrop>(_itemDropCommonPrefab, transform, 10);
        _poolItemUnCommon = new ObjectPooler<ItemDrop>(_itemDropUnCommonPrefab, transform, 10);
        _poolItemRare = new ObjectPooler<ItemDrop>(_itemDropRarePrefab, transform, 10);
        _poolItemEpic = new ObjectPooler<ItemDrop>(_itemDropEpicPrefab, transform, 10);
        _poolItemLegendary = new ObjectPooler<ItemDrop>(_itemDropLegendaryPrefab, transform, 10);
    }
    private void GetRef()
    {
        if (!GameManager.Instance || !GameManager.Instance.Player) return;
        _userData = GameManager.Instance.UserData;
        _player = GameManager.Instance.Player;
    }
    private void HandleInput()
    {
        if (!_itemRewardsData.Any() || !GUIInputs.F) return;
        GUIInputs.F = false;
        
        var _keyValuePair = _itemRewardsData.FirstOrDefault();
        _keyValuePair.Key.Release();
        RemoveNoticeReward(_keyValuePair.Key);
        SetReward(_keyValuePair.Value);
    }
    
    
    /// <summary>
    /// Tạo phần thưởng dựa trên danh sách đã Setup trước đó
    /// </summary>
    /// <param name="_rewardSetup"> Instance chứa danh sách phần thưởng. </param>
    public void CreateReward(RewardSetup _rewardSetup)
    {
        Coin _coin = null;
        var position = _rewardSetup.transform.position + Vector3.up;
        var _rewards = _rewardSetup.GetRewardData();
        
        foreach (var VARIABLE in _rewards)
        {
            if (VARIABLE.GetNameCode() != ItemNameCode.COCoin)
            {
                CreateItemDrop(VARIABLE, position );
                continue;
            }
            
            _coinRewardData.Enqueue(VARIABLE);
            for (var i = 0; i < 8; i++)
            {
                _coin = _poolCoin.Get(position);
                if(_coin.IsPlayer) 
                    continue;
                _coin.SetPlayer(_player);
            }
        
            if (_coin != null) 
                _coin.OnMoveCompleteEvent += CoinMoveCompleted;
        }
    }
    private void CoinMoveCompleted(Coin _coin)
    {
        _coin.OnMoveCompleteEvent -= CoinMoveCompleted;
        SetReward(_coinRewardData.Dequeue());
    }
    private void CreateItemDrop(RewardSetup.ItemReward _itemReward, Vector3 _pos)
    {
        if (!gameItemData.GetItemCustom(_itemReward.GetNameCode(), out var itemCustom))
            return;

        var _itemDrop = itemCustom.ratity switch
        {
            ItemRarity.Common => _poolItemCommon.Get(_pos),
            ItemRarity.Uncommon => _poolItemUnCommon.Get(_pos),
            ItemRarity.Rare => _poolItemRare.Get(_pos),
            ItemRarity.Epic => _poolItemEpic.Get(_pos),
            ItemRarity.Legendary => _poolItemLegendary.Get(_pos),
            _ => null
        };
        
        if (!_itemDrop) return;
        _itemDrop.SetItemDrop(itemCustom.sprite, _itemReward);
    }
    
    
    
    /// <summary>
    /// Thiết lập phần thưởng và lưu vào dữ liệu của người dùng, đồng thời gửi thông báo tới GUI để hiển thị thông tin ra UI
    /// </summary>
    /// <param name="_itemReward"> Thông tin phần thưởng. </param>
    public void SetReward(RewardSetup.ItemReward _itemReward)
    {
        var _val = _itemReward.GetValue();
        var _des = _itemReward.GetDescription() + " <size=12><color=#ABABAB>x</size></color> " + _val;
        var _nameCode = _itemReward.GetNameCode();
        Sprite _sprite = null;
        if (gameItemData.GetItemCustom(_nameCode, out var itemCustom))
        {
            _sprite = itemCustom.sprite;
        }
        RewardNoticeManager.Instance.EnableTitleNoticeT1();
        RewardNoticeManager.CreateNoticeT1(_des, _sprite);
        
        if (_nameCode == ItemNameCode.COCoin)
        {
            _userData.IncreaseCoin(_val);
            return;
        }
        _userData.IncreaseItemValue(_nameCode, _val);
    }
    
    /// <summary>
    /// Thông báo có phần thưởng trên UI, khi người dùng nhấn Input(được cho trước) mới thiết lập phần thưởng cho người dùng
    /// </summary>
    /// <param name="_itemDrop"> Item đang giữ phần thưởng. </param>
    /// <param name="_itemReward"> Phần thưởng được thêm vào. </param>
    public void AddNoticeReward(ItemDrop _itemDrop, RewardSetup.ItemReward _itemReward)
    {
        var _des = _itemReward.GetDescription();
        var _nameCode = _itemReward.GetNameCode();
        Sprite _sprite = null;
        if (gameItemData.GetItemCustom(_nameCode, out var itemCustom))
        {
            _sprite = itemCustom.sprite;
        }

        if (_itemRewardsData.ContainsKey(_itemDrop)) return;
        
        // Tạo thông báo có phần thưởng
        RewardNoticeManager.CreateNoticeT2(_des, _sprite);

        // Thêm dữ liệu phần thưởng vào danh sách chờ
        _itemRewardsData.Add(_itemDrop, _itemReward);
    }
    
    /// <summary>
    /// Xóa thông báo phần thưởng trên UI khi người dùng không còn đứng gần vật phẩm
    /// </summary>
    /// <param name="_itemReward"> Phần thưởng sẽ bị xóa. </param>
    public void RemoveNoticeReward(ItemDrop _itemDrop)
    {
        if(!_itemRewardsData.ContainsKey(_itemDrop)) return;

        _itemRewardsData.Remove(_itemDrop);
        RewardNoticeManager.ReleaseAllNoticeT2();
        
        foreach (var keyValuePair in _itemRewardsData)
        {
            var _des = keyValuePair.Value.GetDescription();
            var _nameCode = keyValuePair.Value.GetNameCode();
            Sprite _sprite = null;
            if (gameItemData.GetItemCustom(_nameCode, out var itemCustom))
            {
                _sprite = itemCustom.sprite;
            }
            RewardNoticeManager.CreateNoticeT2(_des, _sprite);
        }
    }



}
