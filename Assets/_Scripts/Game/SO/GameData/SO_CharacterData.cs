using System;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterNameCode
{
    Arlan = 0,
    Lynx = 1
}

[Serializable]
public class CharacterCustom
{
    public PlayerController prefab;
    public CharacterNameCode nameCode;
}

/// <summary>
/// Dữ liệu tất cả nhân vật trong game.
/// </summary>
[CreateAssetMenu(fileName = "Character Data", menuName = "Game Configuration/Character Data")]
public class SO_CharacterData : ScriptableObject
{
    public List<CharacterCustom> CharactersData = new();
}
