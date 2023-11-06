using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfiguration", menuName = "Configuration/Player")]
public class PlayerConfiguration : ScriptableObject
{
    [Tooltip("Tốc độ đi bộ"), Space] 
    public float walkSpeed = 2.5f;
    
    [Tooltip("Tốc độ chạy"), Space]  
    public float runSpeed = 4f;
    
    [Tooltip("Tốc độ chạy nhanh"), Space]  
    public float runFastSpeed = 8f;
    
    [Tooltip("Năng lượng cho mỗi lần lướt"), Space] 
    public int dashEnergy = 25;
    
    [Tooltip("Độ cao khi nhảy"), Space] 
    public float jumpHeight = 1.2f;

    [Tooltip("Thời gian nhảy cho lần tiếp theo"), Space] 
    public float jumpTimeOut = .35f;
    
    [Tooltip("Thời gian hồi kỹ năng (giây)"), Space] 
    public float skillCooldown;
    
    [Tooltip("Thời gian hồi kỹ năng đặc biệt (giây)"), Space] 
    public float specialCooldown;

}
