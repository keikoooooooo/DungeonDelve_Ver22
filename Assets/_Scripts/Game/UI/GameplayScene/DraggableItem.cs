using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GUI_Item item;
    [Header("UI")] 
    [SerializeField] private Image iconDrag;

    private readonly Color _enableColor = new(1, 1, 1, 1);
    private readonly Color _disableColor = new(1, 1, 1, 0);
    
    public GUI_Item GetItem() => item;


    private void OnEnable() => DraggableData.Add(gameObject, this);
    private void OnDisable() => DraggableData.Remove(gameObject);
    

    public void OnBeginDrag(PointerEventData eventData)
    {
        iconDrag.sprite = item.GetItemCustom.sprite;
        iconDrag.color = _enableColor;
        iconDrag.raycastTarget = false;
        
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        iconDrag.raycastTarget = true;
        iconDrag.color = _disableColor;
        
        transform.SetParent(item.transform);
        transform.localPosition = Vector3.zero;
    }
    
}
