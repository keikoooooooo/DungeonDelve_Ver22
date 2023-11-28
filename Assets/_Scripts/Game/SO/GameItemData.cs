using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class ItemCustom
{
    public ItemType Type;
    public Sprite Sprite;

    public ItemCustom() { }
}

/// <summary>
/// Dự liệu tất cả các Item trong game: Type và Sprite
/// </summary>
[CreateAssetMenu(fileName = "Item Default Datas", menuName = "Game Configuration/Game Item Data")]
public class GameItemData : ScriptableObject
{
    public List<ItemCustom> GameItemDatas = new ();
}
