using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    [SerializeField] private GameObject panelLoading;
    public Animator animator;

    private readonly int IDSceneLoading_IN = Animator.StringToHash("SceneLoading_IN");
    private readonly int IDSceneLoading_OUT = Animator.StringToHash("SceneLoading_OUT");

    private float _progressLoad;
    private Coroutine _loadCoroutine;
    
    
    public void LoadScene(string _sceneName)
    {
        if(_loadCoroutine != null) StopCoroutine(_loadCoroutine);
        _loadCoroutine = StartCoroutine(LoadCoroutine(_sceneName));
    }
    private IEnumerator LoadCoroutine(string _sceneName)
    {
        animator.SetTrigger(IDSceneLoading_IN);
        var scene = SceneManager.LoadSceneAsync(_sceneName);
        scene.allowSceneActivation = false;
        _progressLoad = 0;
        
        while (true)
        {
            if(_progressLoad > .9f) break;
            Debug.Log("LOAD SCENE");
            _progressLoad = Mathf.Clamp01(scene.progress / 0.9f);
            yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));
            yield return null;
        }

        yield return new WaitForSeconds(Random.Range(2,3.5f));
        scene.allowSceneActivation = true;
        animator.SetTrigger(IDSceneLoading_OUT);
    }
    
    
}
