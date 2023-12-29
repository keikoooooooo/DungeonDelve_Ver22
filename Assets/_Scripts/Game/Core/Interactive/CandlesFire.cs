using System.Collections;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CandlesFire : MonoBehaviour
{
    [SerializeField] private new Light light;
    [SerializeField] private ParticleSystem fire;
    
    [SerializeField, Tooltip("Thời gian Object hoạt động (s)")]
    private float timeActive = 60;

    private Coroutine _disableCoroutine;
    private Tween _lightTween;
    private readonly float _enableDuration = 1.4f;
    private readonly float _disableDuration = .5f;
    private float _currentIntensity;
    
    private void Start()
    {
        _currentIntensity = light.intensity;
        light.intensity = 0;
        light.gameObject.SetActive(false);
        //
        fire.gameObject.SetActive(false);
        fire.Stop();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fire")) return;

        fire.gameObject.SetActive(true);
        fire.Play();
        
        light.intensity = 0;
        light.gameObject.SetActive(true);
        _lightTween?.Kill();
        _lightTween = DOVirtual.Float(0, _currentIntensity, _enableDuration, SetIntensity);
        
        if (_disableCoroutine != null) 
            StopCoroutine(_disableCoroutine);
        _disableCoroutine = StartCoroutine(DisableCoroutine());
    }
    private IEnumerator DisableCoroutine()
    {
        yield return new WaitForSeconds(timeActive);
        
        _lightTween?.Kill();
        _lightTween = DOVirtual.Float(_currentIntensity, 0, _disableDuration, SetIntensity).OnComplete(() =>
        {
            light.gameObject.SetActive(false);
        });
        
        fire.gameObject.SetActive(false);
        fire.Stop();
    }
    private void SetIntensity(float _value) => light.intensity = _value;
}
