using TMPro;
using UnityEngine;

public class NameAndLevelText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    
    public void ChangeNameText(string _value) => nameText.text = $"{_value}";
    public void ChangeLevelText(int _value) => levelText.text = $"Lv. {_value}";
    
}
