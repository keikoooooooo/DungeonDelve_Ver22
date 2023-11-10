using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;

public class DashVisualEffect : MonoBehaviour
{

    public PlayerStateMachine player;

    [Space, Tooltip("Chỉnh Visual của camera khi dash")]
    public Volume visualVolume;
    
    [Tooltip("Các hạt partical khi lướt")]
    public ParticleSystem dashPartical;

    [Tooltip("Danh sách các Metarial của model, sẽ sáng lên khi lướt")]
    public List<Material> metarialsList;
    
    
    private Tween _volumeTween;
    private Tween _metarialTween;
    
    private static readonly int Dissolve = Shader.PropertyToID("_EmissionColor");

    private readonly string _Emission = "_Emission";
    
    
    private void Start()
    {
        if(player)
            player.E_Dash += OnDashEvent;
        dashPartical.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        if(player)
            player.E_Dash -= OnDashEvent;
    }


    private void OnDashEvent()
    {
        dashPartical.gameObject.SetActive(true);
        dashPartical.Play();
        
        _volumeTween?.Kill();
        _volumeTween = DOVirtual.Float(0, 1, .15f, SetValueVolume).OnComplete(() =>
        {
            DOVirtual.Float(1, 0, .7f, SetValueVolume);
        });
        
        _metarialTween?.Kill();
        _metarialTween = DOVirtual.Float(-10f, 2.5f, .3f, SetValueMetarial).OnComplete(() =>
        {
            DOVirtual.Float(2.5f, -10f, .25f, SetValueMetarial);
        });

    }

    private void SetValueVolume(float value)
    {
        visualVolume.weight = value;
    }

    private void SetValueMetarial(float value)
    {
        //metarialsList.ForEach(x => x.SetFloat(Dissolve, value));

        foreach (var material in metarialsList)
        {
            var color = material.GetColor(_Emission);
            // color = new Color(color.r * Mathf.Pow(2, value),
            //                   color.r * Mathf.Pow(2, value),
            //                   color.r * Mathf.Pow(2, value),
            //                   color.a);
            material.SetColor(_Emission, color);
        }
    }
    
    
}