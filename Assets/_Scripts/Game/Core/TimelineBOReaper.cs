using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineBOReaper : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public M_SetEmission setEmission;

    [Tooltip("Thời gian kích lại BOSS")]
    [SerializeField] private float bossActivationTime;
    
    private readonly string PP_ActivationTime = "ActivationTime";
    
    private bool _canTrigger;      // có được active BO lên k ?
    private bool _isTriggerPlayer; // có đang TriggerPlayer ?
    
    
    private PlayerController _player;
    private Coroutine _enableTimelineCoroutine;
    private float _bossActivationTimeTemp;
    
    private void Start()
    {
        _player = GameManager.Instance.Player;
        _bossActivationTimeTemp = PlayerPrefs.GetFloat(PP_ActivationTime, 0);
    }

    public void OnEnterPlayer(GameObject _gameObject)
    {
        _isTriggerPlayer = true;
        
        if (_enableTimelineCoroutine != null) 
            StopCoroutine(EnableTimelineCoroutine());
        _enableTimelineCoroutine = StartCoroutine(EnableTimelineCoroutine());
    }
    public void OnExitPlayer(GameObject _gameObject)
    {
        _isTriggerPlayer = false;
    }
    
        
    private void ApplyEmission(float _currentVal, float SetVal)
    {
        setEmission.ChangeCurrentIntensity(_currentVal);
        setEmission.ChangeIntensitySet(SetVal); 
        setEmission.Apply();
    }
    private IEnumerator EnableTimelineCoroutine()
    { 
        _canTrigger = _bossActivationTimeTemp <= 0;
        
        if(!_canTrigger) yield break;
        
        yield return new WaitForSeconds(.7f);
        ApplyEmission(0, 15);
        
        yield return new WaitForSeconds(2f);
        if (_isTriggerPlayer)
        {
            DeActiveControlPlayer();
            playableDirector.Play();
            _bossActivationTimeTemp = bossActivationTime;
            _canTrigger = false;
            PlayerPrefs.SetFloat(PP_ActivationTime, bossActivationTime);
        }
        else
        {
            yield return new WaitForSeconds(.7f);
            playableDirector.Stop();
            ApplyEmission(15, 0);
        }
    }
    private void FixedUpdate()
    {
        if (_bossActivationTimeTemp > 0)
        {
            _bossActivationTimeTemp -= Time.fixedDeltaTime;
            return;
        }
        
        if(_isTriggerPlayer)  return;
        
        _canTrigger = true;
        playableDirector.Stop();
        ApplyEmission(0, 0);
    }

    public void ActiveControlPlayer() // gọi trên EventAnimationTimeline
    {
        if (!_player) 
            return;
        _player.CanControl = true;
    }
    public void DeActiveControlPlayer()
    {
        if (!_player) 
            return;
        _player.CanControl = false;
    }


    private void OnApplicationQuit() => PlayerPrefs.SetFloat(PP_ActivationTime, _bossActivationTimeTemp);
}
