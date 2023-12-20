using FMODUnity;
using NaughtyAttributes;
using UnityEngine;

public class FMOD_Events : Singleton<FMOD_Events>
{

    [field: BoxGroup("SFX/Player/MALE"), SerializeField] public EventReference male_JUMP { get; private set; }
    [field: BoxGroup("SFX/Player/MALE"), SerializeField] public EventReference male_DASH { get; private set; }

    [field: BoxGroup("SFX/Player/FEMALE"), SerializeField] public EventReference female_JUMP { get; private set; }
    [field: BoxGroup("SFX/Player/FEMALE"), SerializeField] public EventReference female_DASH { get; private set; }
    
}
