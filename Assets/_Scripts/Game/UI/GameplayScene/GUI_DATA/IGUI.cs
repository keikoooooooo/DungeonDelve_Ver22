﻿public interface IGUI
{
    /// <summary>
    /// Tham chiếu tới Script đang giữ Data trong game được gửi từ MenuController
    /// </summary>
    /// <param name="_gameManager"> Instance đang giữ Data </param>
    public void GetRef(GameManager _gameManager);
    
    
    /// <summary>
    /// Cập nhật data trên UI lại
    /// </summary>
    public void UpdateData();
}