using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private MonoBehaviourID behaviourID;
    
    [Space] [SerializeField] 
    private List<EnemyController> enemiesPrefab;
    
    [SerializeField, Tooltip("Danh sách các Waypoint trong khu vực này")]
    private Transform waypoints;
    
    [SerializeField, Tooltip("Số lượng tối đa khu vực này Spawn được")] 
    private int maxCountSpawn;
    
    [SerializeField, Tooltip("Bán kính tối đa mỗi Waypoint để random vị trí spawn")] 
    private int maxDistance;

    [SerializeField, Tooltip("Thời gian Reset lại toàn bộ số lượng Spawn trong khu vực (s)")]
    private float waitSpawn = 900f; // 15 Phút

    [Space][Tooltip("Khi tiêu diệt toàn bộ Enemy trong khu vực, sẽ gọi Event.")] 
    public UnityEvent AreaCleanedEvent;
    
    [SerializeField, Space] private bool drawGizmos;


    private readonly List<ObjectPooler<EnemyController>> _poolEnemies = new();
    private YieldInstruction _yieldInstruction;
    private Coroutine _spawnCheckCoroutine;
    private DateTime _lastTime;
    private string PP_SaveCurrentEnemy;
    private int _currentEnemy;
    
    
    private void Start()
    {
        PP_SaveCurrentEnemy = behaviourID.GetID + "Idx";
        _yieldInstruction = new WaitForSeconds(waitSpawn);
        foreach (var prefab in enemiesPrefab)
        {
            _poolEnemies.Add(new ObjectPooler<EnemyController>(prefab, transform, maxCountSpawn));
        }
        
        if(_spawnCheckCoroutine != null) 
            StopCoroutine(_spawnCheckCoroutine);
        _spawnCheckCoroutine = StartCoroutine(SpawnCoroutine());
    }
    
    private IEnumerator SpawnCoroutine()
    {
        _currentEnemy = PlayerPrefs.GetInt(PP_SaveCurrentEnemy, maxCountSpawn);
        if (_currentEnemy > 0)
        {
            for (var i = 0; i < _currentEnemy; i++)
            {
                Spawn();
                yield return new WaitForSeconds(Random.Range(1f, 2f));
            }
        }
        
        _lastTime = DateTime.Parse(PlayerPrefs.GetString(behaviourID.GetID, DateTime.MinValue.ToString()));
        var _totalSeconds = DateTime.Now.Subtract(_lastTime).TotalSeconds;
        if (_totalSeconds < waitSpawn)
        {
            Debug.Log($"Không thể Spawn, Vui lòng chờ trong: {waitSpawn - _totalSeconds}s");
            yield return new WaitForSeconds((float)(waitSpawn - _totalSeconds));
        }
        
        while (true)
        {
            if(_currentEnemy < maxCountSpawn)
            {
                var _enemyNeed = maxCountSpawn - _currentEnemy;
                for (var i = 0; i < _enemyNeed; i++)
                {
                    Spawn();
                    yield return new WaitForSeconds(Random.Range(1f, 2f));
                }
                _currentEnemy += _enemyNeed;
            }
            
            PlayerPrefs.SetString(behaviourID.GetID, DateTime.Now.ToString());
            yield return _yieldInstruction;
        }
    }
    private void Spawn()
    {
        var _waypointRand = Random.Range(0, waypoints.childCount);
        var _posRand = GetRandomPoint(waypoints.GetChild(_waypointRand).position);
        
        var _enemyPrefabIdx = Random.Range(0, _poolEnemies.Count);
        var _poolEnemy = _poolEnemies[_enemyPrefabIdx];

        var _enemy = _poolEnemy.Get(_posRand);
        _enemy.OnDieEvent.AddListener(HandleEnemyDie);
    }
    private void HandleEnemyDie(EnemyController _enemy)
    {
        _currentEnemy--;
        _enemy.OnDieEvent.RemoveListener(HandleEnemyDie);
        CheckReward();
    }
    private Vector3 GetRandomPoint(Vector3 waypointPosition)
    {
        var randomAngle = Random.insideUnitCircle * maxDistance;
        return waypointPosition + new Vector3(randomAngle.x, 0, randomAngle.y);;
    }
    private void CheckReward()
    {
        if (_currentEnemy > 0)
            return; 
        AreaCleanedEvent?.Invoke();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(PP_SaveCurrentEnemy, _currentEnemy);
    }
    private void OnDrawGizmos()
    {
        if(!waypoints || waypoints.childCount == 0 || !drawGizmos) return;
        
        for (var i = 0; i < waypoints.transform.childCount; i++)
        {
            var _currentPoint = waypoints.transform.GetChild(i).position + new Vector3(0, .5f, 0);
            var _nextChildIndex = (i + 1) % waypoints.transform.childCount;
            var _nextPoint = waypoints.transform.GetChild(_nextChildIndex).position + new Vector3(0, .5f, 0);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(_currentPoint, .35f);
            Gizmos.color = new Color(1, .5f, .5f, 1f);
            Gizmos.DrawWireSphere(_currentPoint, maxDistance);
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_currentPoint, _nextPoint);
        }
    }
}
