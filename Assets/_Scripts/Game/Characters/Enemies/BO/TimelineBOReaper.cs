using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineBOReaper : MonoBehaviour
{
    [SerializeField] private MonoBehaviourID behaviourID;
    [SerializeField] private EnemyController reaperBOSS;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private M_SetEmission setEmission;
    
    [Tooltip("Thời gian kích lại BOSS (s)")]
    [SerializeField] private float bossActivationTime;
    
    private bool _canTrigger;      // có được active BO lên k ?
    private bool _isTriggerPlayer; // có đang TriggerPlayer ?
    
    private PlayerController _player;
    private PlayerHUD _playerHUD;
    private Coroutine _enableTimelineCoroutine;
    private DateTime _lastTime;

    
    private void OnEnable()
    {
        reaperBOSS.OnDieEvent.AddListener(HandleBossDie);   
    }
    private void Start()
    {
        _player = GameManager.Instance.Player;
        _playerHUD = _player.GetComponentInChildren<PlayerHUD>();
    }
    private void OnDisable()
    {
        reaperBOSS.OnDieEvent.RemoveListener(HandleBossDie);   
    }

    
    private void HandleBossDie(EnemyController _enemy)
    {
        ApplyEmission(15, 0);
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
        _lastTime = DateTime.Parse(PlayerPrefs.GetString(behaviourID.ID, DateTime.Now.ToString()));
        _canTrigger = DateTime.Now.Subtract(_lastTime).TotalSeconds > bossActivationTime;
        if(!_canTrigger) yield break;
        
        yield return new WaitForSeconds(.7f);
        ApplyEmission(0, 15);
        
        yield return new WaitForSeconds(2f);
        if (_isTriggerPlayer)
        {
            DeActiveControlPlayer();
            playableDirector.Play();
            _canTrigger = false;
            PlayerPrefs.SetString(behaviourID.ID, DateTime.Now.ToString("O"));
        }
        else
        {
            yield return new WaitForSeconds(.7f);
            playableDirector.Stop();
            ApplyEmission(15, 0);
        }
    }

    public void ActiveControlPlayer() // gọi trên EventAnimationTimelineS
    {
        GUI_Inputs.EnableInput();
        
        if (!_player) return;
        _player.input.PlayerInput.Enable();
        _playerHUD.OpenHUD();
    }
    public void DeActiveControlPlayer()
    {
        GUI_Inputs.DisableInput();
        
        if (!_player) return;
        _player.input.PlayerInput.Disable();
        _playerHUD.CloseHUD();
    }
    
}
