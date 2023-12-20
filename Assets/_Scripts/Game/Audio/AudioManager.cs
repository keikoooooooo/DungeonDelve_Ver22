using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public static void PlayerOneShot(EventReference _eventReference, Vector3 _worldPos)
    {
        RuntimeManager.PlayOneShot(_eventReference, _worldPos);
    }

    public EventInstance CreateInstance(EventReference _eventReference)
    {
        return RuntimeManager.CreateInstance(_eventReference);
    }
    
}


