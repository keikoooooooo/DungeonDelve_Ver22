using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineBOReaper : MonoBehaviour
{
    [SerializeField] private MonoBehaviourID behaviourID;
    [Space] 
    [SerializeField] private InteractiveUI interactiveUI;
    [SerializeField] private EnemyController reaperBOSS;
    [SerializeField] private Chest chest;
    [SerializeField] private PlayableDirector playableDirector;

    [Space]
    [SerializeField] private M_SetEmission setEmission;
    [Tooltip("Thời gian kích lại BOSS (s)")] [SerializeField] private float bossActivationTime;
    
    [BoxGroup("VOLUME CHANGE"), SerializeField] private AmbienceVolumeChangeTrigger ambienceVolumeChange;
    [BoxGroup("VOLUME CHANGE"), SerializeField] private BackgroundAudio reaperBattleAudio;
    
    private bool _canTrigger;      // có được active BO lên k ?
    private bool _isTriggerPlayer; // có đang TriggerPlayer ?
    // Ref
    private PlayerController _player;
    private PlayerHUD _playerHUD;
    private CameraFOV _cameraFOV;
    private DateTime _lastDay;
    private TimelineAsset _timeline;
    // 
    private Coroutine _enableTimelineCoroutine;
    private Coroutine _bossDieCoroutine;
    private Coroutine _timeCoroutine;

    
    private void Start()
    {
        interactiveUI.OnPanelOpenEvent += OnEnterPlayer;
        _player = GameManager.Instance.Player;
        _playerHUD =  GameManager.Instance.PlayerHUD;
        _player.OnRevivalTimeEvent += HandlePlayerDie;
        _cameraFOV = _player.GetComponentInChildren<CameraFOV>();
        reaperBOSS.OnDieEvent.AddListener(HandleBossDie);

        _timeline = (TimelineAsset)playableDirector.playableAsset;
        MuteGroupTrack(0, false);
        MuteGroupTrack(1, true);
    }
    private void OnDestroy()
    {
        interactiveUI.OnPanelOpenEvent -= OnEnterPlayer;
        _player.OnRevivalTimeEvent -= HandlePlayerDie;
        reaperBOSS.OnDieEvent.RemoveListener(HandleBossDie);
    }
    
    
#if UNITY_EDITOR
    [ContextMenu("Reset Timeline Key")]
    private void OnResetQuestKey()
    {
        if (!PlayerPrefs.HasKey(behaviourID.GetID)) return;
        PlayerPrefs.DeleteKey(behaviourID.GetID);
        Debug.Log("Delete PlayerPrefs Key Success !. \nKey: " + behaviourID.GetID);
    }
#endif

    
    private void HandlePlayerDie(float _timeRevival)
    {
        if (_timeRevival != 0) return;
        HandleCommon();
        reaperBOSS.gameObject.SetActive(false);
    }
    private void HandleBossDie(EnemyController _enemy)
    {
        if (_bossDieCoroutine != null) StopCoroutine(_bossDieCoroutine);
        _bossDieCoroutine = StartCoroutine(HandleBODieCoroutine());
    }
    private IEnumerator HandleBODieCoroutine()
    {
        PlayerPrefs.SetString(behaviourID.GetID, DateTime.Now.ToString("O"));
        HandleCommon();
        
        yield return new WaitForSeconds(1f);
        NoticeManager.Instance.OpenSuccessfulChallengeNoticePanelT4();
        
        yield return new WaitForSeconds(2f);
        BlackBoard.Instance.Enable(1.5f);
        GUI_Inputs.DisableInput();
        _player.input.PlayerInput.Disable();
        
        yield return new WaitForSeconds(.9f);
        _player.cinemachineFreeLook.m_YAxis.Value = .5f;
        var _currentAng = _player.model.eulerAngles.y;
        var angle = _currentAng >= 180 ? Mathf.Abs(180 - _currentAng) : -Mathf.Abs(180 - _currentAng);
        _player.cinemachineFreeLook.m_XAxis.Value = angle;
        _player.cinemachineFreeLook.m_Lens.FieldOfView = 35f;
        
        
        yield return new WaitForSeconds(.15f);
        _player.cinemachineFreeLook.enabled = false;
        
        yield return new WaitForSeconds(.8f);
        NoticeManager.Instance.OpenBossConqueredNoticeT5();
        
        yield return new WaitForSeconds(3.5f);
        MuteGroupTrack(0, true);
        MuteGroupTrack(1, false);
        playableDirector.Play();
        
        yield return new WaitForSeconds(2f);
        chest.CreateChest();
    }
    
    
    private void HandleCommon()
    {
        reaperBattleAudio.Stop();
        ambienceVolumeChange.SetVolume(.3f);
        ApplyEmission(15, 0);
    }

    private IEnumerator TimeCoroutine()
    {
        while (true)
        {
            var _currentTime = DateTime.Now;
            var _nextMidnight = _currentTime.Date.AddDays(1);
            var _time = _nextMidnight - _currentTime;
            interactiveUI.SetNoticeText($"Can start after {_time.Hours} hour, {_time.Minutes} minute.");
            yield return new WaitForSecondsRealtime(10f);
        }
    }
    public void OnTriggerEnterPlayer()
    {
        _canTrigger = false;
        _lastDay = DateTime.Parse(PlayerPrefs.GetString(behaviourID.GetID, DateTime.MinValue.ToString()));
        if (_lastDay >= DateTime.Today)
        {
            if (_timeCoroutine != null) StopCoroutine(_timeCoroutine);
            _timeCoroutine = StartCoroutine(TimeCoroutine());
           return;
        }
        _canTrigger = true;
        interactiveUI.SetNoticeText("[F] Start.");
    }
    public void OnTriggerExitPlayer()
    {
        _isTriggerPlayer = false;
        if (_timeCoroutine != null) StopCoroutine(_timeCoroutine);
    }
    public void OnEnterPlayer()
    {
        _isTriggerPlayer = true;
        if (_enableTimelineCoroutine != null) 
            StopCoroutine(EnableTimelineCoroutine());
        _enableTimelineCoroutine = StartCoroutine(EnableTimelineCoroutine());
    }
    public void ExitBossActivationArea(bool _isExit) // Khi Player ra khỏi khu vực BossBattle 
    {
        if (!_isExit) return;
        
        // Check BO có đang được active?
        if (!reaperBOSS.gameObject.activeSelf) return;
        
        _canTrigger = false;
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
        if(!_canTrigger) yield break;
        
        ApplyEmission(0, 15);
        MuteGroupTrack(0, false);
        MuteGroupTrack(1, true);
        
        yield return new WaitForSeconds(2f);
        if (_isTriggerPlayer)
        {
            playableDirector.Play();
            _canTrigger = false;
            ambienceVolumeChange.SetVolume(.05f);
            reaperBattleAudio.Play();
            interactiveUI.OnExitPlayer();
        }
        else
        {
            yield return new WaitForSeconds(.7f);
            playableDirector.Stop();
            ApplyEmission(15, 0);
        }
    }

    public void SetCamFOV()
    {
        _cameraFOV.SetCurrentFOV(_player.cinemachineFreeLook.m_Lens.FieldOfView);
        _player.cinemachineFreeLook.enabled = true;
    }
    public void ActiveControl()
    {
        GUI_Inputs.EnableInput();

        if (!_player) return;
        _player.input.PlayerInput.Enable();
        _playerHUD.OpenHUD();
    }
    public void DeactiveControl()
    {
        GUI_Inputs.DisableInput();
        
        if (!_player) return;
        _player.input.PlayerInput.Disable();
        _playerHUD.CloseHUD();
    }
    
    private void MuteGroupTrack(int _trackIndex, bool _isMute)
    {
        var _groupTrack = _timeline.GetRootTrack(_trackIndex);
        _groupTrack.muted = _isMute;
    
        var t = playableDirector.time;
        playableDirector.RebuildGraph();
        playableDirector.time = t;
    }
}
