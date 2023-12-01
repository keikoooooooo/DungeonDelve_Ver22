public interface IGUI
{
    /// <summary>
    /// Tham chiếu tất cả Data trong game được gửi từ MenuController
    /// </summary>
    /// <param name="userData"> Dữ liệu của user </param>
    /// <param name="characterUpgradeData"> Dữ liêu nâng cấp nhân vật trong game </param>
    /// <param name="gameItemData"> Dữ liệu tất cả Item trong game </param>
    /// <param name="player"> Dữ liệu của Player </param>
    public void GetRef(UserData userData, SO_CharacterUpgradeData characterUpgradeData, SO_GameItemData gameItemData, PlayerController player);
    
    
    /// <summary>
    /// Cập nhật data trên UI lại
    /// </summary>
    public void UpdateData();
}