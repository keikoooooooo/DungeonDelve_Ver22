using UnityEngine;

public class GUI_SettingAudio : MonoBehaviour
{
    [SerializeField] private SliderBar volumeSlider;
    [SerializeField] private SliderBar musicVolumeSlider;
    [SerializeField] private SliderBar sfxVolumeSlider;


    private void Start()
    {
        volumeSlider.Slider.onValueChanged.AddListener(OnVolumeChange);   
        musicVolumeSlider.Slider.onValueChanged.AddListener(OnMusicVolumeChange);   
        sfxVolumeSlider.Slider.onValueChanged.AddListener(OnSFXVolumeChange);

        volumeSlider.InitValue(0, 10, 5);
        musicVolumeSlider.InitValue(0, 10, 5);
        sfxVolumeSlider.InitValue(0, 10, 5);
    }
    private void OnDestroy()
    {
        volumeSlider.Slider.onValueChanged.RemoveListener(OnVolumeChange);   
        musicVolumeSlider.Slider.onValueChanged.RemoveListener(OnMusicVolumeChange);   
        sfxVolumeSlider.Slider.onValueChanged.RemoveListener(OnSFXVolumeChange);   
    }


    private void OnVolumeChange(float _value)
    {
        
    }
    private void OnMusicVolumeChange(float _value)
    {
        
    }
    private void OnSFXVolumeChange(float _value)
    {
        
    }

    
}