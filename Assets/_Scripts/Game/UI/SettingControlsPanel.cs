using UnityEngine;

public class SettingControlsPanel : MonoBehaviour
{
    [SerializeField] private SettingSliderBar cameraSensitivity;


    private void Start()
    {
        cameraSensitivity.InitValue(50, 500, 120);
    }
}
