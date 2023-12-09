using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyText;
    [SerializeField] private Image iconItem;
    private GUI_Item _guiItem;

    
    public void SetSlot(GUI_Item guiItem)
    {
        _guiItem = guiItem;
        
        //iconItem.sprite = _guiItem.GetSprite;
    }
    
    /// <summary>
    /// Set khóa Key tương ứng phím tắt Input
    /// </summary>
    /// <param name="_value"> Giá trị Key tương ứng </param>
    public void SetKeyText(int _value) => keyText.text = $"{_value}";
    
}
