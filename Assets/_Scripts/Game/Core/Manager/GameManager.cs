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
    private PlayerController _playerPrefab;
    
    private void OnEnable()
    {
        CharacterUpgradeData.RenewValue();
        
        if(!PlayFabHandleUserData.Instance)
        {
            UserData = new UserData("Test Editor", 50000000);
            _playerPrefab = CharacterData.CharactersData[1].prefab;
            _playerConfig = Instantiate(_playerPrefab.PlayerConfig);
        }
        else
        {
            UserData = PlayFabHandleUserData.Instance.UserData;
            _playerConfig = PlayFabHandleUserData.Instance.PlayerConfig;
            _playerPrefab = GetPlayerPrefab(_playerConfig.NameCode);
        }
        _playerConfig.ChapterIcon = _playerPrefab.PlayerConfig.ChapterIcon;
        InstancePlayer();
    }

    private void InstancePlayer()
    {
        Player = Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
        Player.PlayerData.SetData(_playerConfig);
         
        // Set Status Value
        var _value = _playerConfig.GetHP();
        Player.Health.InitValue(_value, _value);
        _value = _playerConfig.GetST();
        Player.Stamina.InitValue(_value, _value);
    }
    
    private PlayerController GetPlayerPrefab(CharacterNameCode _characterNameCode)
    => CharacterData.CharactersData.Where(characterCustom => characterCustom.prefab.PlayerConfig.NameCode == _characterNameCode)
        .Select(characterCustom => characterCustom.prefab).FirstOrDefault();
    
}
