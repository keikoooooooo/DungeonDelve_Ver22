using UnityEngine;

public class GUI_SettingAudio : MonoBehaviour
{
    [SerializeField] private SliderBar volumeSlider;
    [SerializeField] private SliderBar musicVolumeSlider;
    [SerializeField] private SliderBar sfxVolumeSlider;
    
    // Key PlayerPrefs
    private readonly string PP_VolumeIndex = "VolumeIndex";
    private readonly string PP_MusicVolumeIndex = "MusicVolumeIndex";
    private readonly string PP_SFXVolumeIndex = "SFXVolumeIndex";

    private void Start()
    {
        volumeSlider.Slider.onValueChanged.AddListener(OnVolumeChange);   
        musicVolumeSlider.Slider.onValueChanged.AddListener(OnMusicVolumeChange);   
        sfxVolumeSlider.Slider.onValueChanged.AddListener(OnSFXVolumeChange);

        volumeSlider.InitValue(0, 10, PlayerPrefs.GetInt(PP_VolumeIndex, 5));
        musicVolumeSlider.InitValue(0, 10, PlayerPrefs.GetInt(PP_MusicVolumeIndex, 5));
        sfxVolumeSlider.InitValue(0, 10, PlayerPrefs.GetInt(PP_SFXVolumeIndex, 5));
    }
    private void OnDestroy()
    {
        volumeSlider.Slider.onValueChanged.RemoveListener(OnVolumeChange);   
        musicVolumeSlider.Slider.onValueChanged.RemoveListener(OnMusicVolumeChange);   
        sfxVolumeSlider.Slider.onValueChanged.RemoveListener(OnSFXVolumeChange);   
    }


    private void OnVolumeChange(float _value)
    {
        PlayerPrefs.SetInt(PP_VolumeIndex, (int)_value);
        AudioListener.volume = _value * 0.1f;
    }
    private void OnMusicVolumeChange(float _value)
    {
        PlayerPrefs.SetInt(PP_MusicVolumeIndex, (int)_value);
        // TODO
        
    }
    private void OnSFXVolumeChange(float _value)
    {
        PlayerPrefs.SetInt(PP_SFXVolumeIndex, (int)_value);
        // TODO
        
    }

    
}
