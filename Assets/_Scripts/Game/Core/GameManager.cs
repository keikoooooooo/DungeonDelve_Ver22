using System.Linq;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;


/// <summary>
/// Class này sẽ chạy đầu tiên để spawn ra Player và set dữ liệu từ User vào nhân vật
/// </summary>
public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Dữ liệu người dùng
    /// </summary>
    public UserData UserData { get; private set; }
    
    /// <summary>
    /// Dữ liệu tất cả Item trong game
    /// </summary>
    [field: SerializeField] public SO_CharacterData CharacterData { get; private set; }

    /// <summary>
    /// Dữ liệu về các thông tin và yêu cầu khi nâng cấp Level của nhân vật
    /// </summary>
    [field: SerializeField] public SO_CharacterUpgradeData CharacterUpgradeData { get; private set; }
    
    /// <summary>
    /// Dữ liệu tất cả Item trong game
    /// </summary>
    [field: SerializeField] public SO_GameItemData GameItemData { get; private set; }
    
    public PlayerController player { get; private set; }
    private SO_PlayerConfiguration _playerConfig;
    
    
    private void OnEnable()
    {
        if(!PlayFabHandleUserData.Instance)
        {
            UserData = new UserData("Test Editor", 500);
            player = Instantiate(CharacterData.CharactersData[0].prefab, Vector3.zero, quaternion.identity);
            _playerConfig = Instantiate(player.PlayerConfig);
            _playerConfig.ChapterIcon = CharacterData.CharactersData[0].prefab.PlayerConfig.ChapterIcon;
            player.PlayerData.SetData(_playerConfig);
            return;
        }
        
        UserData = PlayFabHandleUserData.Instance.UserData;
        _playerConfig = PlayFabHandleUserData.Instance.PlayerConfig;
        foreach (var characterCustom in CharacterData.CharactersData.Where(characterCustom => characterCustom.nameCode == _playerConfig.NameCode))
        {
            player = Instantiate(characterCustom.prefab, Vector3.zero, quaternion.identity);
            _playerConfig.ChapterIcon = characterCustom.prefab.PlayerConfig.ChapterIcon;
            player.PlayerData.SetData(_playerConfig);
            break;
        }
    }
    
    
    
}
