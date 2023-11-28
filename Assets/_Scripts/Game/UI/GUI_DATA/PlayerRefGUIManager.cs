using System.Collections.Generic;


/// <summary>
///  Gọi func SendRef để gửi tham chiếu của Player đến tất cả interface IGUIData có trong danh sách GUIDatas để cập nhật data của _player ra UI của từng thành phần
/// </summary>
public static class PlayerRefGUIManager
{
    private static readonly List<IPlayerRef> GUIDatas = new();
    
    
    /// <summary>
    /// Nhận tham chiếu của Player từ MenuController và cập nhật các thông tin cần thiết ra UI (nếu có)
    /// </summary>
    /// <param name="_player"></param>
    public static void SendRef(PlayerController _player) => GUIDatas.ForEach(item => item.GetRef(_player));
    
    
    public static void Add(IPlayerRef playerRef)
    {
        if(GUIDatas.Contains(playerRef)) return;
        GUIDatas.Add(playerRef);
    }
    public static void Remove(IPlayerRef playerRef)
    {
        if(!GUIDatas.Contains(playerRef)) return;
        GUIDatas.Remove(playerRef);
    }
    
}
