using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
/// <summary>
/// Trong bán kính (SphereCollider) nếu có va chạm và Object có LayerMask = Layer chỉ định, trả về vị trí của Object gần với (this) nhất.
/// </summary>
public class PlayerSensor : MonoBehaviour
{
    [Tooltip("Vị trí theo dõi chính, cần truyền models vào để xoay hướng models về hướng của enemy")]
    public Transform location;
    
    [Tooltip("LayerMask cần kiểm tra va chạm")]
    public LayerMask tagetMask;

    public GameObject target { get; private set; }
    
    private readonly List<GameObject> inZone = new List<GameObject>(); // tạo 1 list lưu lại tất cả enemy va chạm
    private Coroutine _zoneCheckCoroutine;
    private Coroutine _rotateLocationCoroutine;
    

    /// <summary>
    /// Nếu có mục tiêu trong tầm ? sẽ xoay vị trí (this) về hướng đó
    /// </summary>
    public bool RotateToTarget()
    {
        if (location == null || target == null) 
            return false;
        
        if(_rotateLocationCoroutine != null) 
            StopCoroutine(_rotateLocationCoroutine);
        _rotateLocationCoroutine = StartCoroutine(RotateLocationCoroutine());
        
        return true;
    }
    private IEnumerator RotateLocationCoroutine()
    {
        var direction = Quaternion.LookRotation(target.transform.position - location.position);
        
        var dicLocal = Math.Floor(location.eulerAngles.y);
        var dicTarget = Math.Floor(direction.eulerAngles.y);
        
        var rotation = Quaternion.Euler(0, direction.eulerAngles.y, 0);
        while (Math.Abs(dicLocal - dicTarget) > .2f)
        {
            location.rotation = Quaternion.RotateTowards(location.rotation, rotation, 1000 * Time.deltaTime);
            dicLocal = Math.Floor(location.eulerAngles.y);
            yield return null;
        }
    }

    private GameObject FindClosestEnemy() // Trả về object đứng gần với transform nhất
    {
        var minDistance = float.MaxValue;
        GameObject newObj = null;
        
        foreach (var _gameObject in inZone)
        {
            var distance = Vector3.Distance(location.position, _gameObject.transform.position);
            if (!(distance < minDistance)) continue;
            minDistance = distance;
            newObj = _gameObject;
        }
        return newObj;
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (!tagetMask.Contains(other.gameObject)) return;
        
        inZone.Add(other.gameObject);
            
        if (_zoneCheckCoroutine != null)
            StopCoroutine(_zoneCheckCoroutine);
        _zoneCheckCoroutine = StartCoroutine(ZoneCheckCoroutine());
    }
    private void OnTriggerExit(Collider other)
    {
        if (!tagetMask.Contains(other.gameObject)) return;
        
        inZone.Remove(other.gameObject);
        
        if (inZone.Count == 0 && _zoneCheckCoroutine != null)
        {
            target = null;
            StopCoroutine(_zoneCheckCoroutine);
        }
    }
    
    private IEnumerator ZoneCheckCoroutine()
    {
        while (true)
        {
            target = inZone.Count == 0 ? null : FindClosestEnemy();
            yield return null;
        }
    }

}
