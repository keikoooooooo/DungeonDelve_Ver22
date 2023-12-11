using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<EnemyController> enemysPrefab;
    [SerializeField] private Transform waypoints;
    [SerializeField] private int maxCountSpawn;

    private PlayerController _player;
    private readonly YieldInstruction _yieldInstruction = new WaitForSeconds(2f);
    private Coroutine _spawnCheckCoroutine;
    private int _currentEnemy;
    private int _randEnemySpawn;
    private int _randWaypointSpawn;

    private ObjectPooler<EnemyController> _poolEnemies;
    
    private void Start()
    {
        _poolEnemies = new ObjectPooler<EnemyController>(enemysPrefab[0], transform, 5);
        
        var gameManage = GameManager.Instance;
        if(gameManage) _player = GameManager.Instance.Player;
        
        if(_spawnCheckCoroutine != null) StopCoroutine(_spawnCheckCoroutine);
        _spawnCheckCoroutine = StartCoroutine(SpawnCoroutine());
    }
    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            if(_currentEnemy < maxCountSpawn)
            {
                _randEnemySpawn = Random.Range(0, enemysPrefab.Count);
                _randWaypointSpawn = Random.Range(0, waypoints.childCount);
                var _enemy = _poolEnemies.Get(waypoints.GetChild(_randWaypointSpawn).position);
                _enemy.OnDieEvent.AddListener(HandleEnemyDie);
                _currentEnemy++;
            }
            yield return _yieldInstruction;
        }
    }


    private void HandleEnemyDie(EnemyController _enemy)
    {
        _currentEnemy--;
        _enemy.OnDieEvent.RemoveListener(HandleEnemyDie);
    }
    
    

}
