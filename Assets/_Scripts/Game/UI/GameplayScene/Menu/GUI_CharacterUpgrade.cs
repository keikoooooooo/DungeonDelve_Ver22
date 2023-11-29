using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI_CharacterUpgrade : MonoBehaviour, IPlayerRef
{
    [Header("Infor")]
    public TextMeshProUGUI charLevelText;
    public TextMeshProUGUI charCurrentEXPText;
    public TextMeshProUGUI userCurrencyText;

    [Space]
    public Slider mainProgressSliderBar, backProgressSliderBar;
    
    public TextMeshProUGUI amountUpgradeText;
    public TextMeshProUGUI itemEXP_1ValueText;
    public TextMeshProUGUI itemEXP_2ValueText;
    public TextMeshProUGUI itemEXP_3ValueText;

    private PlayerController _player;

    public void GetRef(PlayerController player)
    {
        _player = player;
        
        
    }

    private void UpdateUpgradeText(PlayerConfiguration _playerConfig)
    {
        var currentLv = _playerConfig.Level;
        charLevelText.text = $"Lv. {currentLv}";
        charCurrentEXPText.text = $"{_playerConfig.CurrentEXP} / {GameManager.Instance.CharacterUpgradeData.GetNextEXP(currentLv)}";
        
    }
    
}
