using System;
using UnityEngine;

public class PlayerDataHandle : MonoBehaviour
{
    
    /// <summary>
    /// Dữ liệu của người dùng như: Username, Gem, ...
    /// </summary>
    public UserData UserData { get; private set; }

    /// <summary>
    /// Dữ liệu tất cả Item trong game
    /// </summary>
    [field: SerializeField] public GameItemData GameItemData { get; private set; }
    
    /// <summary>
    /// Dữ liệu về các thông tin và yêu cầu khi nâng cấp Level của nhân vật
    /// </summary>
    [field: SerializeField] public UpgradeData UpgradeData { get; private set; }
    
    /// <summary>
    /// Toàn bộ cấu hình của nhân vật: HP, ST, ATK, .....
    /// </summary>
    [field: SerializeField] public PlayerConfiguration PlayerConfig { get; private set; }
    
    /// <summary>
    /// RenderTexture(RawImage) -> Render các model của nhân vật lên UI
    /// </summary>
    public PlayerRenderTexture PlayerRenderTexture { get; private set; }
    [SerializeField] private PlayerRenderTexture playerRenderTexturePrefab;
    


    private void Start()
    {
        PlayerRenderTexture = Instantiate(playerRenderTexturePrefab, null);
    }
}
