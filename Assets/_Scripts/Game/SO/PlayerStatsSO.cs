using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    [Tooltip("Tốc độ đi bộ")] 
    public float walkSpeed = 2.5f;
    
    [Tooltip("Tốc độ chạy")] 
    public float runSpeed = 4f;
    
    [Tooltip("Tốc độ chạy nhanh")] 
    public float runFastSpeed = 8f;
    
    [Tooltip("Năng lượng cho mỗi lần lướt")]
    public int dashEnergy = 25;
    
    [Tooltip("Độ cao khi nhảy")]
    public float jumpHeight = 1.2f;

    [Tooltip("Thời gian nhảy cho lần tiếp theo")]
    public float jumpTimeOut = .35f;
    
    [Tooltip("Thời gian hồi kỹ năng (giây)")]
    public float skillCooldown;
    
    [Tooltip("Thời gian hồi kỹ năng đặc biệt (giây)")]
    public float specialCooldown;

}
