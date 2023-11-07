using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfiguration", menuName = "Configuration/Player")]
public class PlayerConfiguration : ScriptableObject
{
    [Header("Stats")]
    [Tooltip("Máu tối đa")] 
    public int maxHealth;
    
    [Tooltip("Sức bền tối đa")]
    public int maxStamina = 100;
    
    [Tooltip("Năng lượng cho mỗi lần lướt")] 
    public int dashEnergy = 25;
    
    [Tooltip("Tốc độ đi bộ")] 
    public float walkSpeed = 2.5f;
    
    [Tooltip("Tốc độ chạy")]  
    public float runSpeed = 4f;
    
    [Tooltip("Tốc độ chạy nhanh")]  
    public float runFastSpeed = 8f;
    
    [Tooltip("Độ cao khi nhảy")] 
    public float jumpHeight = 1.2f;

    [Tooltip("Thời gian nhảy cho lần tiếp theo")] 
    public float jumpTimeOut = .35f;
    
    [Tooltip("Thời gian hồi kỹ năng (giây)")] 
    public float skillCooldown;
    
    [Tooltip("Thời gian hồi kỹ năng đặc biệt (giây)")] 
    public float specialCooldown;

}
