using System;
using UnityEngine;

public class PlayerDataHandle : MonoBehaviour
{
    public UserData UserData { get; private set; }


    [field: SerializeField] public UpgradeData UpgradeData { get; private set; }
    [field: SerializeField] public PlayerConfiguration PlayerConfig { get; private set; }
    
    
    [SerializeField] private PlayerRenderTexture playerRenderTexturePrefab;
    public PlayerRenderTexture PlayerRenderTexture { get; private set; }
    


    private void Start()
    {
        PlayerRenderTexture = Instantiate(playerRenderTexturePrefab, null);
    }
}
