using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDrop : MonoBehaviour, IPooled<ItemDrop>
{
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] private Rigidbody rb;
    
    private RewardSetup.ItemReward _itemReward;
    private readonly float _jumpForce = 2.5f;
    
    private void OnEnable()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        
        var jumpDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
        rb.velocity = jumpDirection * _jumpForce;
        rb.useGravity = true;
    }
    
    public void SetItemDrop(Sprite _sprite, RewardSetup.ItemReward _itemRewardData)
    {
        _itemReward = _itemRewardData;
        spriteRender.sprite = _sprite;
    }
    
    
    public void OnTriggerEnterPlayer() => RewardManager.Instance.AddNoticeReward(this, _itemReward);
    public void OnTriggerExitPlayer() => RewardManager.Instance.RemoveNoticeReward(this);
    
    
    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<ItemDrop> ReleaseCallback { get; set; }
}
