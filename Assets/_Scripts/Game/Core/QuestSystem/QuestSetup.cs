using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CollectionTask
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


[Serializable]
[CreateAssetMenu(menuName = "Create Quest", fileName = "Quest_")]
public class QuestSetup : ScriptableObject
{
    [HideInInspector, SerializeField] 
    private string id;
    
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private CollectionTask task;
    [SerializeField] private List<ItemReward> rewards;
    
    [HideInInspector, SerializeField]
    private bool isCompletedQuest;

    [HideInInspector, SerializeField]
    private bool isLocked;
    
    //
    public string GetIDQuest() => id;
    public string GetTitle() => title;
    public string GetDescription() => description;
    public CollectionTask GetTask() => task;
    public List<ItemReward> GetReward() => rewards;
    public bool GetCompletedQuest() => isCompletedQuest;
    public bool IsLocked() => isLocked;
    //
    public void SetIDQuest(string _value) => id = _value;
    public void SetTitle(string _value) => title = _value;
    public void SetDescription(string _value) => description = _value;
    public void SetTask(CollectionTask _value) => task = _value;
    public void SetReward(List<ItemReward> _value) => rewards = _value;
    public void SetCompletedQuest(bool _value) => isCompletedQuest = _value;
    public bool SetQuestState(bool _value) =>  isLocked = _value;
}
