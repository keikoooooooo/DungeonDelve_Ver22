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
    
    private void Start()
    {
        PlayerRenderTexture = Instantiate(playerRenderTexturePrefab, null);
    }
 

    
    public void SetCharacterData(PlayerConfiguration _playerConfig)
    {
        PlayerConfig = _playerConfig;
    }
    
    
    
}
