using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;


    private float fps;
    private int currentFPS;
    
    private void Update()
    {
        fps = 1.0f / Time.deltaTime;
        currentFPS = Mathf.RoundToInt(fps);

        fpsText.text = $"{currentFPS}";
    }
}
