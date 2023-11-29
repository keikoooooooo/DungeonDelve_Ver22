using System;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterTypeEnums
{
    Arlan = 0,
    Lynx = 1
}

[Serializable]
public class CharacterCustom
{
    public PlayerController Character;
    public CharacterTypeEnums Type;
}

/// <summary>
/// Dữ liệu tất cả nhân vật trong game.
/// </summary>
[CreateAssetMenu(fileName = "Character Data", menuName = "Game Configuration/Character Data")]
public class CharacterData : ScriptableObject
{
    public List<CharacterCustom> CharactersData = new();
}
