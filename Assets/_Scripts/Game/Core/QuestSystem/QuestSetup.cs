using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CollectionTask
{
    [SerializeField, Tooltip("Loại nhiệm vụ")]
    private ItemNameCode code;

    [SerializeField, Tooltip("Nếu loại nhiệm vụ là Progress sẽ sử dụng giá trị này")] 
    private int value;

    public ItemNameCode GetNameCode() => code;
    public ItemNameCode SetNameCode(ItemNameCode _value) => code = _value;
    public int GetValue() => value;
    public int SetValue(int _value) => value = _value;
}


[CreateAssetMenu(menuName = "Create Quest", fileName = "Quest_")]
public class QuestSetup : ScriptableObject
{
    [SerializeField] private int indexQuest;
    [SerializeField] private string titleQuest;
    [SerializeField] private string descriptionQuest;
    [SerializeField] private CollectionTask collectionTask;
    [SerializeField] private List<ItemReward> rewardSetup;
    
    
    public int GetIndex() => indexQuest;
    public string GetTitle() => titleQuest;
    public string GetDescription() => descriptionQuest;
    public CollectionTask GetTask() => collectionTask;
    public List<ItemReward> GetReward() => rewardSetup;
    

    public void SetIndex(int _value) => indexQuest = _value;
    public void SetTitle(string _value) => titleQuest = _value;
    public void SetDescription(string _value) => descriptionQuest = _value;
    public void SetTask(CollectionTask _value) => collectionTask = _value;
    public void SetReward(List<ItemReward> _value) => rewardSetup = _value;
}
