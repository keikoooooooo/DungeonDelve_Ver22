using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestIndicatorManager : MonoBehaviour
{
    [SerializeField] private IconIndicator indicatorPrefab;
    [SerializeField, Tooltip("Giới hạn cạnh Left và Right mà Indicator được hiển thị")] 
    private float borderWidthSize;
    [SerializeField, Tooltip("Giới hạn cạnh Top và Bottom mà Indicator được hiển thị")]
    private float borderHeightSize;
    
    private Camera _mainCam;
    private Vector3 chestScreenPoint;
    private static readonly Dictionary<Chest, IconIndicator> _chests = new();
    private static ObjectPooler<IconIndicator> _poolIndicator;
    
    
    private void Awake()
    {
        _mainCam = Camera.main;
    }
    private void Start()
    {
        _poolIndicator = new ObjectPooler<IconIndicator>(indicatorPrefab, transform, 5);
    }
    private void LateUpdate()
    {
        if(!_chests.Any()) return;

        foreach (var (key, value) in _chests)
        {
            chestScreenPoint = _mainCam.WorldToScreenPoint(key.transform.position + new Vector3(0, 1.75f, 0));
            var isOffScreen = chestScreenPoint.x <= 0 || chestScreenPoint.x >= Screen.width ||
                              chestScreenPoint.y <= 0 || chestScreenPoint.y >= Screen.height ||
                              chestScreenPoint.z <= 0;
            
            if (isOffScreen) // Ra khỏi màn hình
            {
                OffScreenIndicator(chestScreenPoint, value);
            }
            else
            {
                OnScreenIndicator(chestScreenPoint, value);
            }
        }
    }
    

    private void OffScreenIndicator(Vector3 _screenPoint, IconIndicator _indicator) 
    {
        var clampX = Mathf.Clamp(_screenPoint.x, borderWidthSize, Screen.width - borderWidthSize);
        var clampY = Mathf.Clamp(_screenPoint.y, borderHeightSize, Screen.height - borderHeightSize);
        
        
        _indicator.iconIndicator.position = new Vector3(clampX, clampY, 0);
        
        _indicator.arrowIndicator.gameObject.SetActive(true);
        #region Test 1
        var direction = _indicator.iconIndicator.position - chestScreenPoint;
        direction.Normalize();
        _indicator.arrowIndicator.localPosition = direction * 55f;
        _indicator.arrowIndicator.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        #endregion
        
        #region Test 2
        // var directionToArrow = (chestScreenPoint - _indicator.iconIndicator.position).normalized;
        // var arrowPos = _indicator.iconIndicator.position + directionToArrow * distanceToParent;
        // _indicator.arrowIndicator.position = arrowPos;
        // _indicator.arrowIndicator.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(directionToArrow.y, directionToArrow.x) * Mathf.Rad2Deg);
        #endregion
    }
    private void OnScreenIndicator(Vector3 _screenPoint, IconIndicator _indicator)
    {
       _indicator.iconIndicator.position = _screenPoint;
       _indicator.arrowIndicator.gameObject.SetActive(false);
    }

    
    public static void AddNoticeChest(Chest _chest)
    {
        if (_chests.ContainsKey(_chest)) 
            return;
        
        var _indi = _poolIndicator.Get();
        _chest.Indicator = _indi;
        _chests.Add(_chest, _indi);
    }
    public static void RemoveNoticeChest(Chest _chest)
    {
        if (!_chests.ContainsKey(_chest)) 
            return;
        
        _chest.Indicator.Release();
        _chests.Remove(_chest);
    }

}
