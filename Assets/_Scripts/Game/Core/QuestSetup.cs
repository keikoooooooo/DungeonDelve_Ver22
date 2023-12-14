using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create Quest", fileName = "Quest_")]
public class QuestSetup : ScriptableObject
{
    [SerializeField] private int indexQuest;
    [SerializeField] private string titleQuest;
    [SerializeField] private string descriptionQuest;
    [SerializeField] private List<ItemReward> rewardSetup;
    
    public int GetIndex() => indexQuest;
    public string GetTitle() => titleQuest;
    public string GetDescription() => descriptionQuest;
    public List<ItemReward> GetReward() => rewardSetup;

    public void SetIndex(int _value) => indexQuest = _value;
    public void SetTitle(string _value) => titleQuest = _value;
    public void SetDescription(string _value) => descriptionQuest = _value;
    public void SetReward(List<ItemReward> _value) => rewardSetup = _value;
}
