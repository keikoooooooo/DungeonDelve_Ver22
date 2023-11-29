using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadSceneButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [Tooltip("Tên Scene cần Load")]
    public string SceneName;
    
    private void OnEnable()
    {
        _button.onClick.AddListener(LoadScene);
    }
    private void OnDisable()
    {
        _button.onClick.RemoveListener(LoadScene);
    }

    private void LoadScene()
    {
        LoadSceneManager.Instance.LoadScene(SceneName);
    }
}
