using System;
using FMODUnity;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerVoice : MonoBehaviour
{

    [BoxGroup("STORY"), SerializeField] private EventReference GoodMorningEvent;
    [BoxGroup("STORY"), SerializeField] private EventReference GoodAfternoonEvent;
    [BoxGroup("STORY"), SerializeField] private EventReference GoodEveningEvent;
    [BoxGroup("STORY"), SerializeField] private EventReference GoodNightEvent;
    [BoxGroup("STORY"), SerializeField] private EventReference[] ChatEvent;
    [BoxGroup("STORY"), SerializeField] private EventReference[] OpenChestEvent;
    [BoxGroup("STORY"), SerializeField] private EventReference[] RelaxEvent;

    //
    [BoxGroup("COMBAT"), SerializeField] private EventReference[] JumpEvent;
    [BoxGroup("COMBAT"), SerializeField] private EventReference[] DashEvent;
    [BoxGroup("COMBAT"), SerializeField] private EventReference[] LightAttackEvent;
    [BoxGroup("COMBAT"), SerializeField] private EventReference[] MidAttackEvent;
    [BoxGroup("COMBAT"), SerializeField] private EventReference[] HeavyAttackEvent;
    [BoxGroup("COMBAT"), SerializeField] private EventReference[] ElementalSkillEvent;
    [BoxGroup("COMBAT"), SerializeField] private EventReference[] ElementalBurstEvent;
    [BoxGroup("COMBAT"), SerializeField] private EventReference[] LightHitEvent;
    [BoxGroup("COMBAT"), SerializeField] private EventReference[] HeavyHitEvent;
    [BoxGroup("COMBAT"), SerializeField] private EventReference[] DieEvent;

    private void OnEnable()
    {
        var _currentHour = DateTime.Now.Hour;
        switch (_currentHour)
        {
            case >= 0 and <= 12:
                AudioManager.PlayOneShot(GoodMorningEvent, transform.position);
                break;
            case > 12 and <= 18:
                AudioManager.PlayOneShot(GoodAfternoonEvent, transform.position);
                break;
            default:
                AudioManager.PlayOneShot(GoodEveningEvent, transform.position);
                break;
        }
    }

    private static int Clamp(int _index, EventReference[] _references) => Mathf.Clamp(_index, 0, _references.Length - 1);
    private void Play(EventReference _eventReference) => AudioManager.PlayOneShot(_eventReference, transform.position);

    
    #region STORY
    public void PlayChat(int _audioIndex) => AudioManager.PlayOneShot(ChatEvent[_audioIndex], transform.position);
    public void PlayChat() => AudioManager.PlayOneShot(ChatEvent[Random.Range(0, ChatEvent.Length)], transform.position);
    
    public void PlayOpenChest(int _audioIndex) => AudioManager.PlayOneShot(OpenChestEvent[_audioIndex], transform.position);
    public void PlayOpenChest() => AudioManager.PlayOneShot(OpenChestEvent[Random.Range(0, OpenChestEvent.Length)], transform.position);
    
    public void PlayRelax(int _audioIndex) => AudioManager.PlayOneShot(RelaxEvent[_audioIndex], transform.position);
    public void PlayRelax() => AudioManager.PlayOneShot(RelaxEvent[Random.Range(0, RelaxEvent.Length)], transform.position);
    #endregion
    
    #region COMBAT
    public void PlayLightAttack() => Play(LightAttackEvent[Random.Range(0, LightAttackEvent.Length)]);
    public void PlayMidAttack() => Play(MidAttackEvent[Random.Range(0, MidAttackEvent.Length)]);
    
    public void PlayHeavyAttack() => Play(HeavyAttackEvent[Random.Range(0, HeavyAttackEvent.Length)]);
    public void PlayElementalSkill() => Play(ElementalSkillEvent[Random.Range(0, ElementalSkillEvent.Length)]);
    public void PlayElementalBurstEvent() => Play(ElementalBurstEvent[Random.Range(0, ElementalBurstEvent.Length)]);
    //
    public void PlayJumping() => Play(JumpEvent[Random.Range(0, JumpEvent.Length)]);
    public void PlayDash() => Play(DashEvent[Random.Range(0, DashEvent.Length)]);
    public void PlayLightHit() => Play(LightHitEvent[Random.Range(0, LightHitEvent.Length)]);
    public void PlayHeavyHit() => Play(HeavyHitEvent[Random.Range(0, HeavyHitEvent.Length)]);
    public void PlayDie() => Play(DieEvent[Random.Range(0, DieEvent.Length)]);
    #endregion
    
}
