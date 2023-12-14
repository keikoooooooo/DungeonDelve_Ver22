using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDrop : MonoBehaviour, IPooled<ItemDrop>
{
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float _jumpForce = 10f;
    
    private ItemReward _itemReward;
    private Coroutine _timeActiveCoroutine;
    
    private void OnEnable()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        
        var jumpDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
        rb.velocity = jumpDirection * _jumpForce;
        rb.useGravity = true;
        
        if(_timeActiveCoroutine != null) StopCoroutine(TimerCoroutine());
        _timeActiveCoroutine = StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(80, 100));
        if(gameObject.activeSelf) Release();
    }
    public void SetItemDrop(Sprite _sprite, ItemReward _itemRewardData)
    {
        _itemReward = _itemRewardData;
        spriteRender.sprite = _sprite;
    }
    
    
    public void OnTriggerEnterPlayer() => RewardManager.Instance.AddNoticeReward(this, _itemReward);
    public void OnTriggerExitPlayer() => RewardManager.Instance.RemoveNoticeReward(this);
    
    
    public void Release() => ReleaseCallback?.Invoke(this);
    public Action<ItemDrop> ReleaseCallback { get; set; }
}
