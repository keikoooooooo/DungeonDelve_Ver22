using FMODUnity;
using UnityEngine;

public class FMOD_Events : Singleton<FMOD_Events>
{
    [field:  Header("PLAYER SFX")]
    [field: SerializeField] public EventReference walkFootsteps { get; private set; }
    [field: SerializeField] public EventReference runFootsteps { get; private set; }
    [field: SerializeField] public EventReference runfastFootsteps { get; private set; }
    
    
    
    
    
}
