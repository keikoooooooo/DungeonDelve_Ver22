using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineBOReaper : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public EmissionMetarial emissionMetarial;

    private bool isEnterPlayer;
    private PlayerController _playerController;
    private Coroutine _enableTimelineCoroutine;


    public void OnEnterPlayerSensor(GameObject _gameObject)
    {
        if (_gameObject.TryGetComponent<PlayerController>(out var _player))
        {
            _playerController = _player;
        }
        
        if(_enableTimelineCoroutine != null) 
            StopCoroutine(EnableTimelineCoroutine());
        _enableTimelineCoroutine = StartCoroutine(EnableTimelineCoroutine());
    }
    public void OnExitPlayerSensor(GameObject _gameObject)
    {
        if(_enableTimelineCoroutine != null) 
            StopCoroutine(EnableTimelineCoroutine());
    }
    
    
    private IEnumerator EnableTimelineCoroutine()
    { 
        if(isEnterPlayer)
            yield break;
        
        yield return new WaitForSeconds(.7f);
        emissionMetarial.EnableEmission();
        
        yield return new WaitForSeconds(2f);
        playableDirector.Play();
        DeActiveControlPlayer();
        isEnterPlayer = true;
    }
    
    public void ActiveControlPlayer()
    {
        if (!_playerController) 
            return;
        _playerController.CanMove = true;
        _playerController.CanRotation = true;
    }
    public void DeActiveControlPlayer()
    {
        if (!_playerController) 
            return;
        _playerController.CanMove = false;
        _playerController.CanRotation = false;
    }


}
