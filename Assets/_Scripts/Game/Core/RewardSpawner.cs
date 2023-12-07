using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class RewardSpawner : MonoBehaviour
{
    [BoxGroup("Base Reward"), SerializeField, MinMaxSlider(1, 999), Tooltip("Giá trị tối thiểu và tối đa của phần thưởng")]
    private Vector2 coin;
    [BoxGroup("Base Reward"), SerializeField, MinMaxSlider(1, 999), Tooltip("Giá trị tối thiểu và tối đa của phần thưởng")]
    private Vector2 exp;
    
    [SerializeField]
    private List<ItemReward> itemRewards;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))SendRewardData();
    }

    public int GetCoin() => (int)Random.Range(coin.x, coin.y);
    public int GetExp() => (int)Random.Range(exp.x, exp.y);
    
    public void SendRewardData() => EnemyRewardManager.Instance.CreateReward(this);

}
