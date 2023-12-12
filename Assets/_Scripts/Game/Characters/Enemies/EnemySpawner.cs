using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyController> enemysPrefab;
    [SerializeField] private Transform waypoints;
    
    [SerializeField, Tooltip("Số lượng tối đa khu vực này Spawn được")] 
    private int maxCountSpawn;
    
    [SerializeField, Tooltip("Bán kính tối đa mỗi Waypoint để random vị trí spawn")] 
    private int maxDistance;
    
    [SerializeField, Space] private bool drawGizmos;
    
    
    private readonly YieldInstruction _yieldInstruction = new WaitForSeconds(10f);
    private Coroutine _spawnCheckCoroutine;
    private ObjectPooler<EnemyController> _poolEnemies;
    private int _currentEnemy;
    
    private void Start()
    {
        _poolEnemies = new ObjectPooler<EnemyController>(enemysPrefab[0], transform, 1);
        if(_spawnCheckCoroutine != null) StopCoroutine(_spawnCheckCoroutine);
        _spawnCheckCoroutine = StartCoroutine(SpawnCoroutine());
    }
    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            if(_currentEnemy < maxCountSpawn)
            {
                _currentEnemy++;
                var _pointIdx = Random.Range(0, waypoints.childCount);
                var _enemy = _poolEnemies.Get(GetRandomPoint(waypoints.GetChild(_pointIdx).position));
                _enemy.OnDieEvent.AddListener(HandleEnemyDie);
            }
            yield return _yieldInstruction;
        }
    }
    private void HandleEnemyDie(EnemyController _enemy)
    {
        _currentEnemy--;
        _enemy.OnDieEvent.RemoveListener(HandleEnemyDie);
    }
    private Vector3 GetRandomPoint(Vector3 waypointPosition)
    {
        var randomAngle = Random.insideUnitCircle * maxDistance;
        return waypointPosition + new Vector3(randomAngle.x, 0, randomAngle.y);;
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
