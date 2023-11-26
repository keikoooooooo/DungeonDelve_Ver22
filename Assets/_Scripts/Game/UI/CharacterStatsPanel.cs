using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsPanel : MonoBehaviour
{
    [Header("Infor")] 
    public Animator statsAnimator;
    public TextMeshProUGUI charNameText;
    public TextMeshProUGUI charLevelText;
    public TextMeshProUGUI charCurrentEXPText;
    public Image charChapterIcon;
    
    [Header("BaseStats")]
    public TextMeshProUGUI maxHPText;
    public TextMeshProUGUI maxSTText;
    public TextMeshProUGUI runSpeedText;
    public TextMeshProUGUI elementalSkillText;
    public TextMeshProUGUI elementalBurstText;
    
    [Header("Attack")] 
    public TextMeshProUGUI atkText;
    
    [Header("Defense")] 
    public TextMeshProUGUI defText;
    
    
    
    
}
