using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_CharacterStats : MonoBehaviour
{
    public RawImage rawMesh;
    
    [Header("Infor")] 
    public Animator statsAnimator;
    public TextMeshProUGUI charNameText;
    public TextMeshProUGUI charLevelText;
    public TextMeshProUGUI charCurrentEXPText;
    public Image charChapterIcon;
    
    [Header("BaseStats")]
    public TextBar maxHPText;
    public TextBar maxSTText;
    public TextBar runSpeedText;
    public TextBar elementalSkillText;
    public TextBar elementalBurstText;
    
    [Header("Attack")] 
    public TextBar atkText;
    
    [Header("Defense")] 
    public TextBar defText;
    


    public void OpenCharacterRenderTexture()
    {
        if (!MenuController.Instance.Player) return;
        var _playerRenderTexture = MenuController.Instance.Player.PlayerData.PlayerRenderTexture;
        _playerRenderTexture.OpenRenderUI(PlayerRenderTexture.RenderType.Character);
        rawMesh.texture = _playerRenderTexture.renderTexture;
    }
    
}
