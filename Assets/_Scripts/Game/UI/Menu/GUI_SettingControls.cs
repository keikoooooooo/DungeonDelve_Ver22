using UnityEngine;

public class GUI_SettingControls : MonoBehaviour
{
    [SerializeField] private SliderBar cameraSensitivity;


    private void Start()
    {
        cameraSensitivity.InitValue(50, 500, 120);
    }
}
