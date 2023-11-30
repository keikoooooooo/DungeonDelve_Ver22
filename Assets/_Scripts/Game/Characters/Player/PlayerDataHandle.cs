using System;
using UnityEngine;

public class PlayerDataHandle : MonoBehaviour
{
    /// <summary>
    /// Toàn bộ cấu hình của nhân vật: HP, ST, ATK, .....
    /// </summary>
    [field: SerializeField] public PlayerConfiguration PlayerConfig { get; private set; }
    
    /// <summary>
    /// RenderTexture(RawImage) -> Render các model của nhân vật lên UI
    /// </summary>
    public PlayerRenderTexture PlayerRenderTexture { get; private set; }
    [SerializeField] private PlayerRenderTexture playerRenderTexturePrefab;

    
    private void OnEnable()
    {
        if(!PlayFabHandleUserData.Instance) return;
        SetCharacterData();
    }
    private void Start()
    {
        PlayerRenderTexture = Instantiate(playerRenderTexturePrefab, null);
    }
    private void OnDisable()
    {
        if(!PlayFabHandleUserData.Instance) return;
        SetCharacterData();
    }

    
    private void SetCharacterData()
    {
        PlayerConfig = PlayFabHandleUserData.Instance.PlayerConfig;
        Debug.Log("On get data in playfab");
        Debug.Log("Skill CD = "+PlayerConfig.ElementalSkillCD);
        Debug.Log("Burst CD = "+PlayerConfig.ElementalBurstlCD);
    }
    
    
    
}
