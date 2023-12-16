using System;
using System.Collections.Generic;
using UnityEngine;
using QuestInGame;

namespace QuestInGame
{
    [Serializable]
    public class Task
    {
        [HideInInspector]
        [SerializeField] private string id;
        [SerializeField] private bool isCompleted;
        [SerializeField] private bool isLocked;
        [SerializeField] private bool isReceived;
        public Task() { }
            
        /// <summary>
        /// Tạo 1 Data để lưu trữ thông tin về Task đang nhận
        /// </summary>
        /// <param name="_id"> ID Quest </param>
        /// <param name="_isCompletedQuest"> Task đã hoàn thành chưa ? </param>
        public Task(string _id, bool _isCompleted, bool _isLocked, bool _isReceived)
        {
            id = _id;
            isCompleted = _isCompleted;
            isLocked = _isLocked;
            isReceived = _isReceived;
        }
            
        public string GetID => id;
        public bool IsCompleted => isCompleted;
        public bool IsLocked => isLocked;
        public bool IsReceived => isReceived;
            
        public void SetID(string _value) => id = _value;
        public void SetCompleted(bool _value) => isCompleted = _value;
        public bool SetState(bool _value) =>  isLocked = _value;
        public bool SetReceived(bool _value) =>  isReceived = _value;
    } 

    [Serializable]
    public class TaskRequirement
    {
        [SerializeField, Tooltip("Vật phẩm yêu cầu")]
        private ItemNameCode code;

        [SerializeField, Tooltip("Số lượng vật phẩm cần")] 
        private int value;

        public ItemNameCode GetNameCode() => code;
        public ItemNameCode SetNameCode(ItemNameCode _value) => code = _value;
            
        public int GetValue() => value;
        public int SetValue(int _value) => value = _value;
    }
}


[Serializable]
[CreateAssetMenu(menuName = "Create Quest", fileName = "Quest_")]
public class QuestSetup : ScriptableObject
{
    [SerializeField] private Task task;
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private TaskRequirement requirement;
    [SerializeField] private List<ItemReward> rewards;
    
    
    #region Getter
    /// <summary> Thông tin Task </summary>
    public Task GetTask() => task;
    
    /// <summary> Tiêu đề Task </summary>
    public string GetTitle() => title;
    
    /// <summary> Mô tả Task </summary>
    public string GetDescription() => description;
    
    /// <summary> Yêu cầu của Task </summary>
    public TaskRequirement GetRequirement() => requirement;
    
    /// <summary> Phần thưởng của Task </summary>
    public List<ItemReward> GetReward() => rewards;
    #endregion
    
    #region Setter
    public void SetTask(Task _value) => task = _value;
    public void SetTitle(string _value) => title = _value;
    public void SetDescription(string _value) => description = _value;
    public void SetReward(List<ItemReward> _value) => rewards = _value;
    #endregion

}
