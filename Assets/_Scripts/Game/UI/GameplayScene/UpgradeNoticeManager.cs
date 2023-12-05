using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeNoticeManager : Singleton<UpgradeNoticeManager>
{
    [SerializeField] private Animator animator;
    [SerializeField] private Button confirmBtt;
    [SerializeField] private TextMeshProUGUI levelText;
    
    [SerializeField] private TextBar textBarPrefab;
    [SerializeField] private Transform textContent;

    private ObjectPooler<TextBar> _poolTextBar;

    
    private void Start()
    {
        _poolTextBar = new ObjectPooler<TextBar>(textBarPrefab, textContent, 15);
        confirmBtt.onClick.AddListener(DisableNotice);
    }
    private void OnDestroy()
    {
        confirmBtt.onClick.RemoveListener(DisableNotice);
    }



    public void SetLevelText(string _value) => levelText.text = _value;
    public void EnableNotice(List<string> _values)
    {
        foreach (var value in _values)
        {
            var textBar = _poolTextBar.Get();
            textBar.SetValueText(value);
        }
        animator.Play("OnEnableUpgradeSuccess");
    }
    private void DisableNotice()
    {
        animator.Play("OnDisableUpgradeSuccess");
        foreach (var textBar in _poolTextBar.List)
        {
            textBar.Release();
        }
    }


}
