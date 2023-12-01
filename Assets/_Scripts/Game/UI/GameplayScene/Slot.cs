using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private Image iconItem;
    private Item item;

    
    public void SetSlot(Item _item)
    {
        item = _item;
        
        iconItem.sprite = item.GetSprite;
    }
    
    /// <summary>
    /// Set khóa Key tương ứng phím tắt Input
    /// </summary>
    /// <param name="_value"> Giá trị Key tương ứng </param>
    public void SetKeyText(int _value) => keyText.text = $"{_value}";
    
}
