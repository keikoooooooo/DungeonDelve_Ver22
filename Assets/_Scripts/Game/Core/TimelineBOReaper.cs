using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineBOReaper : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public M_SetEmission setEmission;

    private bool isEnterPlayer; // player đã vào khu vực chưa 
    private bool isTriggerPlayer; // có đang TriggerPlayer ?
    
    private PlayerController _playerController;
    private Coroutine _enableTimelineCoroutine;

    

    public void OnEnterPlayer(GameObject _gameObject)
    {
        if (_playerController == null && _gameObject.TryGetComponent<PlayerController>(out var _player))
        {
            _playerController = _player;
        }
        
        isTriggerPlayer = true;
        if (_enableTimelineCoroutine != null) StopCoroutine(EnableTimelineCoroutine());
        _enableTimelineCoroutine = StartCoroutine(EnableTimelineCoroutine());
    }
    public void OnExitPlayer(GameObject _gameObject)
    {
        isTriggerPlayer = false;
        playableDirector.Stop();
    }
    
    
    private IEnumerator EnableTimelineCoroutine()
    { 
        if(isEnterPlayer)
            yield break;
        
        yield return new WaitForSeconds(.7f);
        setEmission.currentIntensity = 0;
        setEmission.intensitySetTo = 15f;
        setEmission.Apply();
        
        yield return new WaitForSeconds(2f);
        
        if (isTriggerPlayer)
        {
            DeActiveControlPlayer();
            isEnterPlayer = true;
            playableDirector.Play();
        }
        else
        {
            yield return new WaitForSeconds(.7f);
            playableDirector.Stop();
            setEmission.currentIntensity = 15f;
            setEmission.intensitySetTo = 0f;
            setEmission.Apply();
        }
    }
    
    public void ActiveControlPlayer() // gọi trên EventAnimationTimeline
    {
        if (!_playerController) 
            return;
        _playerController.enabled = true;
    }
    public void DeActiveControlPlayer()
    {
        if (!_playerController) 
            return;
        _playerController.enabled = false;
    }


}
