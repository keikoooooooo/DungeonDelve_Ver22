using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class BackgroundAudio : MonoBehaviour
{
    [SerializeField] private new EventReference audio;
    [SerializeField] private string volumeParameterName;
    [SerializeField, Range(0, 1)] private float volumeStart;
    
    private EventInstance _instance;
    private void Start()
    {
        _instance = AudioManager.CreateInstance(audio);
        _instance.setParameterByName(volumeParameterName, volumeStart);
        Play();
    }
    
    public void Play() => _instance.start();
    public void Stop() => _instance.stop(STOP_MODE.ALLOWFADEOUT);
    
}
