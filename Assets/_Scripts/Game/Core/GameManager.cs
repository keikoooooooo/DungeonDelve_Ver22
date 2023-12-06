using System.Linq;
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
    
    public PlayerController Player { get; private set; }
    private SO_PlayerConfiguration _playerConfig;
    
    
    private void OnEnable()
    {
        CharacterUpgradeData.RenewValue();

        PlayerController _playerPrefab;
        if(!PlayFabHandleUserData.Instance)
        {
            UserData = new UserData("Test Editor", 50000000);
            _playerPrefab = CharacterData.CharactersData[0].prefab;
            _playerConfig = Instantiate(_playerPrefab.PlayerConfig);
        }
        else
        {
            UserData = PlayFabHandleUserData.Instance.UserData;
            _playerConfig = PlayFabHandleUserData.Instance.PlayerConfig;
            _playerPrefab = GetPlayerPrefab(_playerConfig.NameCode);
        }
        
        Player = Instantiate(_playerPrefab, new Vector3(-43.2999992f,11.6000004f,118.199997f), Quaternion.identity);
        //Player = Instantiate(_playerPrefab, new Vector3(14.3000002f,-30.8999996f,194.300003f), Quaternion.identity);
        _playerConfig.ChapterIcon = _playerPrefab.PlayerConfig.ChapterIcon;
        Player.PlayerData.SetData(_playerConfig);
        Player.InitStatus();
    }

    private PlayerController GetPlayerPrefab(CharacterNameCode _characterNameCode)
    => CharacterData.CharactersData.Where(characterCustom => characterCustom.prefab.PlayerConfig.NameCode == _characterNameCode)
        .Select(characterCustom => characterCustom.prefab).FirstOrDefault();
    
}
