using UnityEngine;
using Random = UnityEngine.Random;

public class GoblinSlingshot_Effects : MonoBehaviour
{
    public EnemyController enemyController;
    
    [Tooltip("Điểm xuất hiện effect")]
    public Transform effectPoint;

    [Header("Prefab projectile")] 
    public EffectBase projectilePrefab;
    
    private ObjectPooler<EffectBase> _poolProjectile;
    private Transform slotsVFX;
    
    private void Start()
    {
        slotsVFX = GameObject.FindWithTag("SlotsVFX").transform;
        _poolProjectile = new ObjectPooler<EffectBase>(projectilePrefab, slotsVFX, 5);
    }

    public void EffectAttack()
    {
        var playerPos = enemyController.PlayerPosition;
        playerPos.y += Random.Range(1f, 1.5f);
        var rotation = Quaternion.LookRotation(playerPos - effectPoint.position);
        var projectile = _poolProjectile.Get(effectPoint.position, rotation);
        projectile.FIRE();
    }


    
    
    
}
