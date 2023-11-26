using TMPro;
using UnityEngine;

public class WeaponStatsPanel : MonoBehaviour
{
    [Header("Infor")]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponLevelText;

    [Header("Base")]
    public TextMeshProUGUI critRateText;
    public TextMeshProUGUI critDMGText;
    
    
    [Header("Attack")]
    public TextMeshProUGUI hit1_DMGText;
    public TextMeshProUGUI hit2_DMGText;
    public TextMeshProUGUI hit3_DMGText;
    public TextMeshProUGUI hit4_DMGText;
    public TextMeshProUGUI hit5_DMGText;
    
    public TextMeshProUGUI chargedAttack_DMGText;
    public TextMeshProUGUI chargedAttack_STCostText;
    
    public TextMeshProUGUI elementalSkillText;
    public TextMeshProUGUI elementalBurstText;


    [Header("Details")] 
    public TextMeshProUGUI weaponInfo;

}
