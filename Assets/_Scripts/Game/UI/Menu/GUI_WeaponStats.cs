using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_WeaponStats : MonoBehaviour
{
    public RawImage rawMesh;
    
    [Header("Infor")]
    public TextMeshProUGUI weaponNameText;
    public TextMeshProUGUI weaponLevelText;

    [Header("Base")]
    public TextBar critRateText;
    public TextBar critDMGText;
    [Header("Attack")]
    public TextBar hit1_DMGText;
    public TextBar hit2_DMGText;
    public TextBar hit3_DMGText;
    public TextBar hit4_DMGText;
    public TextBar hit5_DMGText;
    public TextBar chargedAttack_DMGText;
    public TextBar chargedAttack_STCostText;
    public TextBar elementalSkillText;
    public TextBar elementalBurstText;
    [Header("Details")] 
    public TextBar weaponInfo;

    
    
    public void OpenWeaponRenderTexture()
    {
        if (!MenuController.Instance.Player) return;
        var _playerRenderTexture = MenuController.Instance.Player.PlayerData.PlayerRenderTexture;
        _playerRenderTexture.OpenRenderUI(PlayerRenderTexture.RenderType.Weapon);
        rawMesh.texture = _playerRenderTexture.renderTexture;
    }
    
}
