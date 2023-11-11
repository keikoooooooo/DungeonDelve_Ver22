using UnityEngine;

public class DMGPopUpGenerator : Singleton<DMGPopUpGenerator>
{
    public Camera mainCamera;
    public DMGPopUp dmgPopUpPrefab;
    
    private ObjectPooler<DMGPopUp> _poolDMGPopUp;
    
    private void Start()
    {
        _poolDMGPopUp = new ObjectPooler<DMGPopUp>(dmgPopUpPrefab, null, 20);

        foreach (var VARIABLE in _poolDMGPopUp.Pool)
        {
            VARIABLE.mainCamera = mainCamera;
        }
    }
    
    public void CreateDMGPopUp(int _damage, Vector3 _position, bool isCRIT)
    {
        var dmgPopUp = _poolDMGPopUp.Get(_position);
        dmgPopUp.ShowDMGPopUp(_damage, isCRIT);
    }
    
}
