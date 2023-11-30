using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// Dữ liệu người dùng
    /// </summary>
    [field: SerializeField]  public UserData UserData { get; private set; }
    
    /// <summary>
    /// Dữ liệu tất cả Item trong game
    /// </summary>
    [field: SerializeField] public CharacterData CharacterData { get; private set; }

    /// <summary>
    /// Dữ liệu về các thông tin và yêu cầu khi nâng cấp Level của nhân vật
    /// </summary>
    [field: SerializeField] public CharacterUpgradeData CharacterUpgradeData { get; private set; }
    
    /// <summary>
    /// Dữ liệu tất cả Item trong game
    /// </summary>
    [field: SerializeField] public GameItemData GameItemData { get; private set; }
    
    public PlayerController player { get; private set; }
    private PlayerConfiguration _playerConfiguration;
    
    private void OnEnable()
    {
        if(!PlayFabHandleUserData.Instance)
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            return;
        }
        
        UserData = PlayFabHandleUserData.Instance.UserData;
        _playerConfiguration = PlayFabHandleUserData.Instance.PlayerConfig;
        
        foreach (var characterCustom in CharacterData.CharactersData.Where(characterCustom => characterCustom.nameCode == _playerConfiguration.NameCode))
        {
            player = Instantiate(characterCustom.prefab, Vector3.zero, quaternion.identity);
            player.PlayerData.SetCharacterData(_playerConfiguration);
            break;
        }
    }

}
