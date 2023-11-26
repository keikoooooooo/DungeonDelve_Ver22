using System.Collections;
using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;


    private float fps;
    private int currentFPS;

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
            fps = 1.0f / Time.unscaledDeltaTime;
            currentFPS = Mathf.RoundToInt(fps);
            fpsText.text = $"{currentFPS}";
            yield return null;
        }
    }
}
