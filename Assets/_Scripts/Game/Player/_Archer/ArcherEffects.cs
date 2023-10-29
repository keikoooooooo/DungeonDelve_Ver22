using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArcherEffects : MonoBehaviour
{
    [Tooltip("Script điều khiển chính"), SerializeField]
    private ArcherController _archerController;
    
    [Space(10), Tooltip("Góc độ bắn mũi tên lên"), SerializeField, Range(-40, 0)] 
    private float angleXAttack;  
    
    [Tooltip("Vị trí sẽ xuất hiện projectile")] 
    public Transform attackPoint;
    
    [Header("Prefab projectile")] 
    [SerializeField] private EffectBase arrowComboPrefab;
    [SerializeField] private EffectBase holdingPrefab;
    
    [Space, Header("Visual Effect")]
    [SerializeField] private ParticleSystem effectHolding;
    [SerializeField] private ParticleSystem effectSpecial;
    
    private Transform slotsVFX;
    private ObjectPooler<EffectBase> _poolArrowCombo;
    private ObjectPooler<EffectBase> _poolArrowHold;

    private Coroutine _mouseHoldTimeCoroutine;
    private float _holdingTime;
    
    
    private float angleYAttack => _archerController.model.eulerAngles.y;
    private bool isEnemy => _archerController.playerSensor.target != null;
    
    
    // Coroutine
    private Coroutine _specialCoroutine;
    
    
    private void Start()
    {
        Initialized();
    }
    private void Initialized()
    {
        slotsVFX = GameObject.FindWithTag("SlotsVFX").transform; 
        _poolArrowCombo = new ObjectPooler<EffectBase>(arrowComboPrefab, slotsVFX, 10);
        _poolArrowHold = new ObjectPooler<EffectBase>(holdingPrefab, slotsVFX, 10);

        effectSpecial.transform.SetParent(slotsVFX);
    }
    
    
    private void EffectArrowCombo(AnimationEvent eEvent)
    {
        var _quaternion = isEnemy ? RandomDirection() : Quaternion.Euler(angleXAttack , angleYAttack, 0f);
        var arrow = _poolArrowCombo.Get(attackPoint.position, _quaternion);
        arrow.FIRE();
    }
    private void EffectArrowHold(AnimationEvent eEvent)
    {
        TurnOffFxHold();
        var arrow = _poolArrowHold.Get(attackPoint.position, attackPoint.rotation);
        arrow.FIRE();
    }
    public void TurnOnFxHold()
    {
        effectHolding.gameObject.SetActive(true);
        effectHolding.Play();
    }
    public void TurnOffFxHold()
    {
        effectHolding.Stop();
        effectHolding.gameObject.SetActive(false);
    }

    private void Effect_Skill(AnimationEvent eEvent)
    {
        var position = attackPoint.position;
        var rotation = Quaternion.Euler(isEnemy ? -6f : angleXAttack, attackPoint.eulerAngles.y + eEvent.intParameter, attackPoint.eulerAngles.z);
        var arrow = _poolArrowCombo.Get(position, rotation);
        arrow.FIRE();
    }
    private void EffectSpecial(AnimationEvent eEvent)
    {
        if (_specialCoroutine != null) 
            StopCoroutine(_specialCoroutine);
        _specialCoroutine = StartCoroutine(ActiveFXSpecial());
    }
    private IEnumerator ActiveFXSpecial()
    {
        // In
        effectSpecial.Play();
        yield return new WaitForSeconds(.3f);
        effectSpecial.transform.position = attackPoint.position;
        effectSpecial.transform.rotation = Quaternion.Euler(-50f, angleYAttack, 0);
        effectSpecial.gameObject.SetActive(true);
        yield return new WaitForSeconds(.85f);
        
        // Out
        var maxRadius = 3f;
        for (var i = 0; i < 20; i++)
        {
            // lấy 1 vị tri ngẫu nhiên trong bán kính maxRadius
            var randomPoint = Random.insideUnitCircle * maxRadius;
            
            // Từ vị trí xuất hiện và vị trí mục tiêu tìm ngẫu nhiên 1 vị trí mới trong bk vừa tìm đc
            var currentPos = transform.position + new Vector3(randomPoint.x, 7f, randomPoint.y);
            var targetPos = _archerController.targetMarkerQ.transform.position + new Vector3(randomPoint.x, Random.Range(-.5f, .5f), randomPoint.y);
   
            var arrow = _poolArrowHold.Get(currentPos, Quaternion.LookRotation(targetPos - currentPos));
            arrow.FIRE();
            yield return new WaitForSeconds(0.1f);
        }
        
        effectSpecial.gameObject.SetActive(false);
        effectSpecial.Stop();
    }
    
    
    private Quaternion RandomDirection()
    {
        var posTarget = _archerController.playerSensor.target.transform.position;
        posTarget.y += 1.2f;
        var randRotX = Random.Range(-2f, 2f);
        var randRotY = Random.Range(-2f, 2f);
        return Quaternion.LookRotation(posTarget - attackPoint.transform.position) * Quaternion.Euler(randRotX, randRotY, 0);
    }

}