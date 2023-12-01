using UnityEngine;

public class PlayerDataHandle : MonoBehaviour
{
    /// <summary>
    /// Toàn bộ cấu hình của nhân vật: HP, ST, ATK, .....
    /// </summary>
    [field: SerializeField] public SO_PlayerConfiguration PlayerConfig { get; private set; }
    
    /// <summary>
    /// RenderTexture(RawImage) -> Render các model của nhân vật lên UI
    /// </summary>
    public PlayerRenderTexture PlayerRenderTexture { get; private set; }
    [SerializeField] private PlayerRenderTexture playerRenderTexturePrefab;
    
    private void Start()
    {
        PlayerRenderTexture = Instantiate(playerRenderTexturePrefab, null);
    }
    
    
    /// <summary>
    /// Cập nhật DataConfig vào nhân vật
    /// </summary>
    public void SetData(SO_PlayerConfiguration _playerConfig)
    {
        PlayerConfig = _playerConfig;
    }
    
}
