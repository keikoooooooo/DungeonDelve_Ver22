using UnityEngine;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [Tooltip("Tên Scene cần Load")]
    public string SceneName;

    private bool _nullBtt;
    private void OnEnable()
    {
        _nullBtt = _button != null;
        
        if(!_nullBtt) return;
        _button.onClick.AddListener(LoadScene);
    }
    private void OnDisable()
    {
        if(!_nullBtt) return;
        _button.onClick.RemoveListener(LoadScene);
    }

    public void LoadScene()
    {
        if(!LoadSceneManager.Instance) return;
        
        LoadSceneManager.Instance.LoadScene(SceneName);
    }
    
}
