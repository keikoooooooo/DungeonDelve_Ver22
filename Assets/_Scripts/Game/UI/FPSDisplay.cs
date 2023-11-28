using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;


    private float fps;
    private int currentFPS;
    private float refreshRate;
    private Coroutine _fpsCoroutine;


    private void OnEnable()
    {
        if (_fpsCoroutine != null)
        {
            StopCoroutine(_fpsCoroutine);
        }
        _fpsCoroutine = StartCoroutine(FPSCoroutine());
    }

    private IEnumerator FPSCoroutine()
    {
        while (true)
        {
            refreshRate = (float)Screen.currentResolution.refreshRateRatio.value;
            
            fps = 1.0f / Time.unscaledDeltaTime;
            currentFPS = Mathf.RoundToInt(fps / refreshRate);
            fpsText.text = $"{currentFPS}";
            yield return null;
        }
    }
}
