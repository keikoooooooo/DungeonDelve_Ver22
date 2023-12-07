using System;
using UnityEngine;


[Serializable]
public class ItemReward
{
    public ItemNameCode code;
    public int minValue;
    public int maxValue;
}

public class EnemyRewardManager : Singleton<EnemyRewardManager>
{

    [SerializeField] private Coin coinPrefab;

    private PlayerController player;
    private ObjectPooler<Coin> _poolCoin;


    private void Start()
    {
        _poolCoin = new ObjectPooler<Coin>(coinPrefab, null, 20);
        
        if (!GameManager.Instance || !GameManager.Instance.Player) return;
        player = GameManager.Instance.Player;
        _poolCoin.List.ForEach(coin => coin.SetPlayer(player));
    }

    public void CreateReward(RewardSpawner _reward)
    {
        var position = _reward.transform.position;
        position.y += .5f;
        
        for (var i = 0; i < 10; i++)
        {
            _poolCoin.Get(position);
        }   
        
        Debug.Log("Coin = " +_reward.GetCoin());
        Debug.Log("EXP =  " +_reward.GetExp());
    }
    

}
