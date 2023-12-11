using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class RewardSetup : MonoBehaviour
{
    
    [Serializable]
    public class ItemReward
    {
        [SerializeField, Tooltip("Loại phần thưởng, được phân loại bởi Item Namecode")]
        private ItemNameCode code;
        
        [SerializeField, MinMaxSlider(1, 999), Tooltip("Giá trị tối thiểu và tối đa của phần thưởng")]
        private Vector2 value;
        
        
        /// <summary>
        /// Trả về code của phần thưởng
        /// </summary>
        /// <returns></returns>
        public ItemNameCode GetNameCode() => code;
        
        /// <summary>
        /// Giá trị phần thưởng
        /// </summary>
        /// <returns></returns>
        public int GetValue() => (int)Random.Range(value.x, value.y);
    }
    
    
    [Tooltip("Thiết lập 1 danh sách dữ liệu phần thưởng và gọi SendRewardData để class RewardManager nhận dữ liệu và xử lí.")]
    [SerializeField] private List<ItemReward> rewardsData;
    public List<ItemReward> GetRewardData() => rewardsData;
    
    
    /// <summary>
    /// Gửi dữ liệu phần thưởng tới class quản lí mỗi khi enemy die
    /// </summary>
    public void SendRewardData() => RewardManager.Instance.CreateReward(this);
    
}
