using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineBOReaper : MonoBehaviour
{
    [SerializeField] private MonoBehaviourID behaviourID;
    [Space]
    [SerializeField] private EnemyController reaperBOSS;
    [SerializeField] private Chest chest;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private M_SetEmission setEmission;
    [Tooltip("Thời gian kích lại BOSS (s)")] [SerializeField] private float bossActivationTime;
    
    [BoxGroup("VOLUME CHANGE"), SerializeField] private AmbienceVolumeChangeTrigger ambienceVolumeChange;
    [BoxGroup("VOLUME CHANGE"), SerializeField] private BackgroundAudio reaperBattleAudio;
    
    private bool _canTrigger;      // có được active BO lên k ?
    private bool _isTriggerPlayer; // có đang TriggerPlayer ?
    private PlayerController _player;
    private PlayerHUD _playerHUD;
    private Coroutine _enableTimelineCoroutine;
    private Coroutine _bossDieCoroutine;
    private Coroutine _playerDieCoroutine;
    private DateTime _lastTime;

    
    private void Start()
    {
        _player = GameManager.Instance.Player;
        _playerHUD =  GameManager.Instance.PlayerHUD;
        _player.OnDieEvent += HandlePlayerDie;
        reaperBOSS.OnDieEvent.AddListener(HandleBossDie);   
    }
    private void OnDestroy()
    {
        _player.OnDieEvent -= HandlePlayerDie;
        reaperBOSS.OnDieEvent.RemoveListener(HandleBossDie);
    }

    
    private void HandleBossDie(EnemyController _enemy)
    {
        if (_bossDieCoroutine != null) StopCoroutine(_bossDieCoroutine);
        _bossDieCoroutine = StartCoroutine(HandleBODieCoroutine());
    }
    private IEnumerator HandleBODieCoroutine()
    {
        ApplyEmission(15, 0);
        yield return new WaitForSeconds(3f);
        chest.CreateChest();
    }
    
    private void HandlePlayerDie()
    {
        if (_playerDieCoroutine != null) StopCoroutine(_playerDieCoroutine); 
        _playerDieCoroutine = StartCoroutine(HandlePlayerDieCoroutine());
    }
    private IEnumerator HandlePlayerDieCoroutine()
    {
        yield return new WaitForSeconds(2f);
        reaperBattleAudio.Stop();
        ambienceVolumeChange.SetVolume(.3f);
        reaperBOSS.gameObject.SetActive(false);
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
    public void ExitBossActivationArea(bool _isExit) // Khi Player ra khỏi khu vực BossBattle 
    {
        if (!_isExit) return;
        
        // Check BO có đang được active?
        if (!reaperBOSS.gameObject.activeSelf) return;
        
        _canTrigger = false;
        PlayerPrefs.SetString(behaviourID.GetID, DateTime.Now.ToString("O"));
        ambienceVolumeChange.SetVolume(.3f);
        reaperBattleAudio.Stop();
        ApplyEmission(15, 0);
        reaperBOSS.gameObject.SetActive(false);
    }
    
    private void ApplyEmission(float _currentVal, float SetVal)
    {
        setEmission.ChangeCurrentIntensity(_currentVal);
        setEmission.ChangeIntensitySet(SetVal); 
        setEmission.Apply();
    }
    private IEnumerator EnableTimelineCoroutine()
    {
        _lastTime = DateTime.Parse(PlayerPrefs.GetString(behaviourID.GetID, DateTime.MinValue.ToString()));
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
            PlayerPrefs.SetString(behaviourID.GetID, DateTime.Now.ToString("O"));
            ambienceVolumeChange.SetVolume(.05f);
            reaperBattleAudio.Play();
        }
        else
        {
            yield return new WaitForSeconds(.7f);
            playableDirector.Stop();
            ApplyEmission(15, 0);
        }
    }

    public void ActiveControlPlayer() // gọi trên EventAnimationTimeline
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
