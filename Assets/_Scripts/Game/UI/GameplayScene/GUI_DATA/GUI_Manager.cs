using System.Collections.Generic;


/// <summary>
///  Gọi func SendRef để gửi tham chiếu của Player đến tất cả interface IGUIData có trong danh sách GUIDatas để cập nhật data của _player ra UI của từng thành phần
/// </summary>
public static class GUI_Manager
{
    private static readonly List<IGUI> GUIDatas = new();
    
    /// <summary>
    /// Gửi tất cả RefData tới các GUI_ có đăng kí nhận ref, chỉ gọi 1 lần duy nhất mỗi lần scene này được khởi tạo
    /// </summary>
    /// <param name="userData"> Dữ liệu của user </param>
    /// <param name="characterUpgradeData"> Dữ liêu nâng cấp nhân vật trong game </param>
    /// <param name="gameItemData"> Dữ liệu tất cả Item trong game </param>
    /// <param name="player"> Dữ liệu của Player </param>
    public static void SendRef(UserData _userData, SO_CharacterUpgradeData _characterUpgradeData, SO_GameItemData _gameItemData, PlayerController _player)
        => GUIDatas.ForEach(item => item.GetRef(_userData, _characterUpgradeData, _gameItemData, _player));

    
    /// <summary>
    /// Cập nhật lại tất cả dữ liệu trên toàn bộ UI 
    /// </summary>
    public static void UpdateGUIData() => GUIDatas.ForEach(gui => gui.UpdateData());
    
    public static void Add(IGUI iGui)
    {
        if(GUIDatas.Contains(iGui)) return;
        GUIDatas.Add(iGui);
    }
    public static void Remove(IGUI iGui)
    {
        if(!GUIDatas.Contains(iGui)) return;
        GUIDatas.Remove(iGui);
    }
    
    
}
