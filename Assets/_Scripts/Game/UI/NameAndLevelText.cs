using TMPro;
using UnityEngine;

public class NameAndLevelText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;

    public void ChangeNameText(string _value)
    {
        if(!nameText) return;
        nameText.text = $"{_value}";
    }

    public void ChangeLevelText(int _value)
    {
        if(!levelText) return;
        levelText.text = $"Lv. {_value}";
    }
    
}
