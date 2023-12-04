using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameplayEditorWindow : EditorWindow
{

    [MenuItem("Tools/GAMEPLAY CUSTOM")]
    public static void ShowCustom()
    {
        var editor = GetWindow<GameplayEditorWindow>();
        editor.titleContent = new GUIContent("Gameplay Custom");
    }

    private readonly string[] _toolTitles = { "PLAYERS", "ENEMIES", "GAME CUSTOM" };
    private int _selectedTool = -1;
    private Vector2 scrollView;
    
    
    private void OnGUI()
    {
        _selectedTool = GUILayout.Toolbar(_selectedTool, _toolTitles, Width(500), Height(50));
        switch (_selectedTool)
        {
            case 0:
                HandlePanelPlayers();
                break;
            
            case 1:
                HandlePanelEnemies();
                break;
            
            case 2:
                HandlePanelGameCustom();
                break;
        }
    }
    
    
    #region PLAYERS
    private int _selectedType = -1;
    private readonly string[] _type = { "STATS CONFIG", "CHARACTER UPGRADE DATA" };
    private int _selectedPlayer = -1;
    private readonly string[] _playerNames = { "Arlan", "Lynx" };
    
    private SO_PlayerConfiguration _arlanConfig;
    private SO_PlayerConfiguration _lynxConfig;
    private SO_CharacterUpgradeData _characterUpgradeData;
    private SO_RequiresWeaponUpgradeConfiguration _requiresWeaponUpgrade;

    
    private void HandlePanelPlayers()
    {
        Space(15);
        _selectedType = GUILayout.Toolbar(_selectedType, _type, Width(250), Height(35));
        switch (_selectedType)
        {
            case 0:
                _selectedPlayer = GUILayout.Toolbar(_selectedPlayer, _playerNames, Width(200), Height(30));
                switch (_selectedPlayer)
                {
                    case 0:
                        _arlanConfig = EditorGUIUtility.Load("Assets/Resources/Player/1. Arlan/Prefab/Arlan Config.asset") as SO_PlayerConfiguration;
                        _requiresWeaponUpgrade = EditorGUIUtility.Load("Assets/Resources/Player/1. Arlan/Prefab/Weapon Upgrade Config.asset") as SO_RequiresWeaponUpgradeConfiguration;
                        ShowPlayerConfig(_arlanConfig);
                        break;
                    
                    case 1:
                        _lynxConfig = EditorGUIUtility.Load("Assets/Resources/Player/2. Lynx/Prefab/Lynx Config.asset") as SO_PlayerConfiguration;
                        _requiresWeaponUpgrade = EditorGUIUtility.Load("Assets/Resources/Player/2. Lynx/Prefab/Weapon Upgrade Config.asset") as SO_RequiresWeaponUpgradeConfiguration;
                        ShowPlayerConfig(_lynxConfig);
                        break;
                }
                break;
            case 1:
                _characterUpgradeData = EditorGUIUtility.Load("Assets/Resources/GameData/Character Upgrade Data.asset") as SO_CharacterUpgradeData;
                ShowUpgradeDetails(_characterUpgradeData);
                break;

        }
    }

    private void ShowPlayerConfig(SO_PlayerConfiguration _playerConfig) 
    {
        if (_playerConfig == null)
        {
            EditorGUILayout.HelpBox("Assign a PlayerConfig.", MessageType.Warning);
            return;
        }

        scrollView = GUILayout.BeginScrollView(scrollView);
        EditorGUI.BeginChangeCheck();

        #region Stats
        Space(25);
        GUILayout.Label("NAME CODE -----------------------------", EditorStyles.boldLabel);
        _playerConfig.NameCode = (CharacterNameCode)EditorGUILayout.EnumPopup("Code", _playerConfig.NameCode, Width(500));
        
        Space(30);
        GUILayout.Label("INFORMATION ------------------------", EditorStyles.boldLabel);
        _playerConfig.SetName(EditorGUILayout.TextField("Name", $"{_playerConfig.NameCode}", Width(500)));
        _playerConfig.SetLevel(EditorGUILayout.IntField("Level", _playerConfig.GetLevel(), Width(500)));
        _playerConfig.SetCurrentEXP(EditorGUILayout.IntField("Current EXP", _playerConfig.GetCurrentEXP(), Width(500)));
        _playerConfig.SetInfor(EditorGUILayout.TextField("Infor", _playerConfig.GetInfor(), Width(500)));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Chapter Icon", Width(148));
        _playerConfig.ChapterIcon = (Sprite)EditorGUILayout.ObjectField(_playerConfig.ChapterIcon, typeof(Sprite), false, Width(50), Height(50));
        GUILayout.EndHorizontal();
        
        Space(30);
        GUILayout.Label("CHARACTER STATS ----------------------", EditorStyles.boldLabel);
        _playerConfig.SetHP(EditorGUILayout.IntField("Max HP", _playerConfig.GetHP(), Width(500)));
        _playerConfig.SetST(EditorGUILayout.IntField("Max ST", _playerConfig.GetST(), Width(500)));
        _playerConfig.SetATK(EditorGUILayout.IntField("ATK", _playerConfig.GetATK(), Width(500)));
        
        _playerConfig.SetCRITRate(EditorGUILayout.FloatField("CRIT Rate(%)", _playerConfig.GetCRITRate(), Width(500)));
        _playerConfig.SetCRITDMG(EditorGUILayout.IntField("CRIT DMG(%)", _playerConfig.GetCRITDMG(), Width(500)));
        _playerConfig.SetDEF(EditorGUILayout.IntField("DEF", _playerConfig.GetDEF(), Width(500)));
        _playerConfig.SetChargedAttackSTCost(EditorGUILayout.IntField("Charged Attack ST Cost", _playerConfig.GetChargedAttackSTCost(), Width(500)));
        _playerConfig.SetWalkSpeed(EditorGUILayout.FloatField("Walk Speed", _playerConfig.GetWalkSpeed(), Width(500)));
        _playerConfig.SetRunSpeed(EditorGUILayout.FloatField("Walk Speed", _playerConfig.GetRunSpeed(), Width(500)));
        _playerConfig.SetRunFastSpeed(EditorGUILayout.FloatField("Run Fast Speed", _playerConfig.GetRunFastSpeed(), Width(500)));
        _playerConfig.SetDashSTCost(EditorGUILayout.IntField("Dash ST Cost", _playerConfig.GetDashSTCost(), Width(500)));
        _playerConfig.SetJumpHeight(EditorGUILayout.FloatField("Jump Height", _playerConfig.GetJumpHeight(), Width(500)));

        Space(30);
        GUILayout.Label("WEAPON -------------------------", EditorStyles.boldLabel);
        _playerConfig.SetWeaponName(EditorGUILayout.TextField("Weapon Name", _playerConfig.GetWeaponName(), Width(500)));
        _playerConfig.SetWeaponInfo(EditorGUILayout.TextField("Weapon Info", _playerConfig.GetWeaponInfo(), Width(500)));
        _playerConfig.SetWeaponLevel(EditorGUILayout.IntSlider("Weapon Level", _playerConfig.GetWeaponLevel(), 1, 10, Width(500)));
        
        Space(30);
        GUILayout.Label("COOLDOWN -------------------------", EditorStyles.boldLabel);
        _playerConfig.SetJumpCD(EditorGUILayout.FloatField("Jump CD(s)", _playerConfig.GetJumpCD(), Width(500)));
        _playerConfig.SetElementalSkillCD(EditorGUILayout.FloatField("Elemental Skill CD(s)", _playerConfig.GetElementalSkillCD(), Width(500)));
        _playerConfig.SetElementalBurstCD(EditorGUILayout.FloatField("Elemental Burst CD(s)", _playerConfig.GetElementalBurstCD(), Width(500)));
        #endregion

        Space(30);
        GUILayout.Label("MULTIPLIER -------------------------", EditorStyles.boldLabel);
        
        #region Normal Attack
        Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("NORMAL ATTACK MULTIPLIER", Width(200), Height(25));
        for (var i = 1; i <= 10; i++)
        {
            GUILayout.Box($"Wea Lv{i}", Width(65), Height(25));
        }
        GUILayout.EndHorizontal();
        
        for (var i = 0; i < _playerConfig.GetNormalAttackMultiplier().Count; i++)
        {
            GUILayout.BeginHorizontal();
             _playerConfig.NormalAttackMultiplier[i].MultiplierTypeName = $"{i + 1} - Hit DMG (%)";
             GUILayout.Box($"{i + 1} - Hit DMG (%)" ,Width(200), Height(27));
             for (var j = 0; j < _playerConfig.NormalAttackMultiplier[i].Multiplier.Count; j++)
             {
                 _playerConfig.NormalAttackMultiplier[i].Multiplier[j] = 
                     EditorGUILayout.FloatField("", _playerConfig.NormalAttackMultiplier[i].Multiplier[j], EditorStyles.numberField, Width(65), Height(27));
                 Space(1);
             }
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            var FloatMultiplier = new FloatMultiplier
            {
                Multiplier = new List<float> { 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0}
            };
            _playerConfig.NormalAttackMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && _playerConfig.NormalAttackMultiplier.Count != 0)
        {
            _playerConfig.NormalAttackMultiplier.Remove(_playerConfig.NormalAttackMultiplier[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        #region Charged Attack
        Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("CHARGED ATTACK MULTIPLIER", Width(200), Height(25));
        for (var i = 1; i <= 10; i++)
        {
            GUILayout.Box($"Wea Lv{i}", Width(65), Height(25));
        }
        GUILayout.EndHorizontal();
        for (var i = 0; i < _playerConfig.ChargedAttackMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            _playerConfig.ChargedAttackMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", _playerConfig.ChargedAttackMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < _playerConfig.ChargedAttackMultiplier[i].Multiplier.Count; j++)
            {
                _playerConfig.ChargedAttackMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", _playerConfig.ChargedAttackMultiplier[i].Multiplier[j],
                        EditorStyles.numberField, Width(65), Height(27));
                Space(1);
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            var FloatMultiplier = new FloatMultiplier
            {
                Multiplier = new List<float> { 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0}
            };
            _playerConfig.ChargedAttackMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && _playerConfig.ChargedAttackMultiplier.Count != 0)
        {
            _playerConfig.ChargedAttackMultiplier.Remove(_playerConfig.ChargedAttackMultiplier[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        #region Elemental Skill
        Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("ELEMENTAL SKILL MULTIPLIER", Width(200), Height(25));
        for (var i = 1; i <= 10; i++)
        {
            GUILayout.Box($"Wea Lv{i}", Width(65), Height(25));
        }
        GUILayout.EndHorizontal();
        
        for (var i = 0; i < _playerConfig.SkillMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            _playerConfig.SkillMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", _playerConfig.SkillMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < _playerConfig.SkillMultiplier[i].Multiplier.Count; j++)
            {
                _playerConfig.SkillMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", _playerConfig.SkillMultiplier[i].Multiplier[j],
                        EditorStyles.numberField, Width(65), Height(27));
                Space(1);
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            var FloatMultiplier = new FloatMultiplier
            {
                Multiplier = new List<float> { 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0}
            };
            _playerConfig.SkillMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && _playerConfig.SkillMultiplier.Count != 0)
        {
            _playerConfig.SkillMultiplier.Remove(_playerConfig.SkillMultiplier[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        #region Elemental Burst
        Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("ELEMENTAL BURST MULTIPLIER", Width(200), Height(25));
        for (var i = 1; i <= 10; i++)
        {
            GUILayout.Box($"Wea Lv{i}", Width(65), Height(25));
        }
        GUILayout.EndHorizontal();
        
        for (var i = 0; i < _playerConfig.SpecialMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            _playerConfig.SpecialMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", _playerConfig.SpecialMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < _playerConfig.SpecialMultiplier[i].Multiplier.Count; j++)
            {
                _playerConfig.SpecialMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", _playerConfig.SpecialMultiplier[i].Multiplier[j],
                        EditorStyles.numberField, Width(65), Height(27));
                Space(1);
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            var FloatMultiplier = new FloatMultiplier
            {
                Multiplier = new List<float> { 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0}
            };
            _playerConfig.SpecialMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && _playerConfig.SpecialMultiplier.Count != 0)
        {
            _playerConfig.SpecialMultiplier.Remove(_playerConfig.SpecialMultiplier[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        ShowRequiesWeaponUpgrade(_requiresWeaponUpgrade);

        if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(_playerConfig);
        GUILayout.EndScrollView();
    }
    private void ShowRequiesWeaponUpgrade(SO_RequiresWeaponUpgradeConfiguration _requiresData)
    {
        Space(30);
        GUILayout.Label("REQUIRES WEAPON UPGRADE -----------", EditorStyles.boldLabel);
        Space(5);
        
        if(_requiresData == null) return;

        GUILayout.BeginHorizontal();
        GUILayout.Box("Level Upgrade", Width(100));
        GUILayout.Box("Coin Upgrade Cost", Width(120));
        GUILayout.Box("Item Code", Width(150));
        GUILayout.Box("Item Value", Width(75));
        GUILayout.EndHorizontal();
        
        EditorGUI.BeginChangeCheck();
        for (var i = 0; i < _requiresData.RequiresDatas.Count; i++)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.BeginHorizontal();
            var _data = _requiresData.RequiresDatas[i];
            _data.levelUpgrade = i + 2;
            GUILayout.Box($"{i + 1} -> {i + 2}",BoxColorText(Color.red) , Width(100));
            _data.coinCost = EditorGUILayout.IntField("", _data.coinCost, TextFieldColorText(Color.cyan) , Width(120));
            
            GUILayout.BeginVertical();
            foreach (var _item in _data.requiresItem)
            {
                GUILayout.BeginHorizontal();
                _item.code = (ItemNameCode)EditorGUILayout.EnumPopup("", _item.code, Width(150));
                _item.value = EditorGUILayout.IntField("", _item.value, Width(75));
                GUILayout.EndHorizontal();
            }
            
            #region Button
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+", Width(45), Height(20)) && _data.requiresItem.Count < 9)
            {
                _data.requiresItem.Add(new SO_RequiresWeaponUpgradeConfiguration.UpgradeItem());
            }
            if(GUILayout.Button("-", Width(45), Height(20)) && _data.requiresItem.Count != 0)
            {
                _data.requiresItem.Remove(_data.requiresItem[^1]);
            }
            GUILayout.EndHorizontal();
            Space(10);
            #endregion
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            
            GUILayout.EndHorizontal();
        }
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            _requiresData.RequiresDatas.Add(new SO_RequiresWeaponUpgradeConfiguration.RequiresData());
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && _requiresData.RequiresDatas.Count != 0)
        {
            _requiresData.RequiresDatas.Remove(_requiresData.RequiresDatas[^1]);
        }
        GUILayout.EndHorizontal();
        Space(30);        
        
        if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(_requiresData);
    }
    private void ShowUpgradeDetails(SO_CharacterUpgradeData _upgradeData)
    {

        
        if(_upgradeData == null) 
            return;
        _upgradeData.SetData();
        Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Box("Level", Width(150));
        GUILayout.Box("Level Next Upgrade", Width(140));
        GUILayout.Box("EXP Needed", Width(150));
        GUILayout.Label("", Width(30));
        GUILayout.Box("Total EXP Cost", Width(150));
        GUILayout.Label("", Width(50));
        if(GUILayout.Button("Renew Value",ButtonColorText(Color.white), Width(120), Height(20)))
        {
            Debug.Log("Reset Value Success!");
            _upgradeData.RenewValue();
        }
        if (GUILayout.Button("Show ConsoleLog", ButtonColorText(Color.white), Width(120), Height(20)))
        {
            foreach (var upgradeCustom in _upgradeData.Data)
            {
                Debug.Log($"Level: {upgradeCustom.Level}/EXP: {upgradeCustom.EXP}/Total EXP Cost: {upgradeCustom.TotalExp}");
            }
        }
        GUILayout.EndHorizontal();
        
        scrollView = GUILayout.BeginScrollView(scrollView);
        for (var i = 0; i < _upgradeData.Data.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Box($"{ _upgradeData.Data[i].Level}", Width(150));
            GUILayout.Box(i + 1 >= SO_CharacterUpgradeData.levelMax ? "~" : $"{_upgradeData.Data[i + 1].Level}", BoxColorText(Color.red), Width(140));
            GUILayout.Box($"{ _upgradeData.Data[i].EXP}", BoxColorText(Color.cyan),Width(150));
            GUILayout.Label("  ->  ", Width(30));
            GUILayout.Box($"{_upgradeData.Data[i].TotalExp}" , BoxColorText(Color.magenta), Width(150));
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }
    #endregion

    
    #region ENEMIES
    private readonly string[] _enemiesNames = { "Goblin", "BOSS: Reaper" };
    private int _selectedEnemy = -1;
    private readonly string[] _goblinType = { "Goblin: Sword", "Goblin: Slingshot", "Goblin: Daggers" };
    private int _selectedTypeEnemy = -1;
    
    private SO_EnemyConfiguration goblin_SwordConfig;
    private SO_EnemyConfiguration goblin_SlingshotConfig;
    private SO_EnemyConfiguration goblin_DaggersConfig;
    private SO_EnemyConfiguration BOReaperCongfig;
    
    private void HandlePanelEnemies()
    {
        Space(10);
        _selectedEnemy = GUILayout.Toolbar(_selectedEnemy, _enemiesNames, Width(250), Height(35));
        switch (_selectedEnemy)
        {
            case 0:
                Space(10);
                _selectedTypeEnemy = GUILayout.Toolbar(_selectedTypeEnemy, _goblinType,Width(500), Height(50));
                switch (_selectedTypeEnemy)
                {
                    case 0:
                        goblin_SwordConfig = EditorGUIUtility.Load("Assets/Resources/Enemies/Goblin/Sword/Prefab/Goblin_Sword Config.asset") as SO_EnemyConfiguration;
                        ShowEnemyConfig(goblin_SwordConfig);
                        break;
                    case 1: 
                        goblin_SlingshotConfig = EditorGUIUtility.Load("Assets/Resources/Enemies/Goblin/Slingshot/Prefab/Goblin_Slingshot Config.asset") as SO_EnemyConfiguration;
                        ShowEnemyConfig(goblin_SlingshotConfig);
                        break;
                    case 2: 
                        goblin_DaggersConfig = EditorGUIUtility.Load("Assets/Resources/Enemies/Goblin/Daggers/Prefab/Goblin_Daggers Config.asset") as SO_EnemyConfiguration;
                        ShowEnemyConfig(goblin_DaggersConfig);
                        break;
                }
                break;
            
            case 1: 
                BOReaperCongfig = EditorGUIUtility.Load("Assets/Resources/Enemies/Reaper/Prefab/BOReaper Config.asset") as SO_EnemyConfiguration;
                ShowEnemyConfig(BOReaperCongfig);
                break;
        }
    }
    private void ShowEnemyConfig(SO_EnemyConfiguration _enemyConfig)
    {
        if (_enemyConfig == null)
        {
            EditorGUILayout.HelpBox("Assign a EnemyConfig.", MessageType.Warning);
            return;
        }

        scrollView = GUILayout.BeginScrollView(scrollView);
        EditorGUI.BeginChangeCheck();
        
        Space(30);
        GUILayout.Label("INFORMATION ------------------------", EditorStyles.boldLabel);
        _enemyConfig.SetName(EditorGUILayout.TextField("Name", _enemyConfig.GetName(), Width(500)));
        _enemyConfig.SetLevel(EditorGUILayout.IntField("Level", _enemyConfig.GetLevel(), Width(500)));
        _enemyConfig.SetInfor(EditorGUILayout.TextField("Infor", _enemyConfig.GetInfor(), Width(500)));
        
        Space(30);
        GUILayout.Label("STATS -------------------------------", EditorStyles.boldLabel);
        _enemyConfig.SetHP(EditorGUILayout.IntField("Max HP", _enemyConfig.GetHP(), Width(500)));
        _enemyConfig.SetATK(EditorGUILayout.IntField("ATK", _enemyConfig.GetATK(), Width(500)));
        _enemyConfig.SetCRITRate(EditorGUILayout.FloatField("CRIT Rate(%)", _enemyConfig.GetCRITRate(), Width(500)));
        _enemyConfig.SetCRITDMG(EditorGUILayout.IntField("CRIT DMG(%)", _enemyConfig.GetCRITDMG(), Width(500)));
        _enemyConfig.SetDEF(EditorGUILayout.IntField("DEF", _enemyConfig.GetDEF(), Width(500)));
        _enemyConfig.SetWalkSpeed(EditorGUILayout.FloatField("Walk Speed", _enemyConfig.GetWalkSpeed(), Width(500)));
        _enemyConfig.SetRunSpeed(EditorGUILayout.FloatField("Run Speed", _enemyConfig.GetRunSpeed(), Width(500)));
        
        Space(30);
        GUILayout.Label("COOLDOWN -------------------------", EditorStyles.boldLabel);
        _enemyConfig.NormalAttackCD = EditorGUILayout.FloatField("Normal Attack CD(s)", _enemyConfig.NormalAttackCD, Width(500));
        _enemyConfig.SkillAttackCD = EditorGUILayout.FloatField("Skill Attack CD(s)", _enemyConfig.SkillAttackCD, Width(500));
        _enemyConfig.SpecialAttackCD = EditorGUILayout.FloatField("Special Attack CD(s)", _enemyConfig.SpecialAttackCD, Width(500));
        
        Space(30);
        GUILayout.Label("MULTIPLIER -------------------------", EditorStyles.boldLabel);
        #region Normal Attack
        Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("NORMAL ATTACK MULTIPLIER", Width(200), Height(25));
        for (var i = 1; i <= 100; i++)
        {
            GUILayout.Box($"Lv {i} - {i + 9}", Width(80), Height(25));
            i += 9;
        }
        GUILayout.EndHorizontal();
        
        for (var i = 0; i < _enemyConfig.NormalAttackMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            _enemyConfig.NormalAttackMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", _enemyConfig.NormalAttackMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < _enemyConfig.NormalAttackMultiplier[i].Multiplier.Count; j++)
            {
                _enemyConfig.NormalAttackMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", _enemyConfig.NormalAttackMultiplier[i].Multiplier[j], EditorStyles.numberField, Width(80), Height(27));
                Space(1);
            }
            GUILayout.EndHorizontal();
        }
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            var FloatMultiplier = new FloatMultiplier
            {
                Multiplier = new List<float> { 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0}
            };
            _enemyConfig.NormalAttackMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && _enemyConfig.NormalAttackMultiplier.Count != 0)
        {
            _enemyConfig.NormalAttackMultiplier.Remove(_enemyConfig.NormalAttackMultiplier[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        #region Elemental Skill
        Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("ELEMENTAL SKILL MULTIPLIER", Width(200), Height(25));
        for (var i = 1; i <= 100; i++)
        {
            GUILayout.Box($"Lv {i} - {i + 9}", Width(80), Height(25));
            i += 9;
        }
        GUILayout.EndHorizontal();
        
        for (var i = 0; i < _enemyConfig.SkillMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            _enemyConfig.SkillMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", _enemyConfig.SkillMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < _enemyConfig.SkillMultiplier[i].Multiplier.Count; j++)
            {
                _enemyConfig.SkillMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", _enemyConfig.SkillMultiplier[i].Multiplier[j],
                        EditorStyles.numberField, Width(80), Height(27));
                Space(1);
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            var FloatMultiplier = new FloatMultiplier
            {
                Multiplier = new List<float> { 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0}
            };
            _enemyConfig.SkillMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && _enemyConfig.SkillMultiplier.Count != 0)
        {
            _enemyConfig.SkillMultiplier.Remove(_enemyConfig.SkillMultiplier[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        #region Elemental Burst
        Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("ELEMENTAL BURST MULTIPLIER", Width(200), Height(25));
        for (var i = 1; i <= 100; i++)
        {
            GUILayout.Box($"Lv {i} - {i + 9}", Width(80), Height(25));
            i += 9;
        }
        GUILayout.EndHorizontal();
        
        for (var i = 0; i < _enemyConfig.SpecialMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            _enemyConfig.SpecialMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", _enemyConfig.SpecialMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < _enemyConfig.SpecialMultiplier[i].Multiplier.Count; j++)
            {
                _enemyConfig.SpecialMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", _enemyConfig.SpecialMultiplier[i].Multiplier[j],
                        EditorStyles.numberField, Width(80), Height(27));
                Space(1);
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            var FloatMultiplier = new FloatMultiplier
            {
                Multiplier = new List<float> { 0, 0, 0, 0, 0, 0, 0 ,0, 0, 0}
            };
            _enemyConfig.SpecialMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && _enemyConfig.SpecialMultiplier.Count != 0)
        {
            _enemyConfig.SpecialMultiplier.Remove(_enemyConfig.SpecialMultiplier[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(_enemyConfig);
        GUILayout.EndScrollView();
    }
    #endregion


    #region GAMECUSTOM
    private int _selectedPanelGameCustomType = -1;
    private readonly string[] _gameCustomtype = { "ITEM DATA" , "CHARACTER DATA"};
    
    private SO_GameItemData _soGameItemData;
    private SO_CharacterData _soCharacterData;
    
    private void HandlePanelGameCustom()
    {
        Space(15);
        _selectedPanelGameCustomType = GUILayout.Toolbar(_selectedPanelGameCustomType, _gameCustomtype, Width(250), Height(35));
        switch (_selectedPanelGameCustomType)
        {
            case 0:
                _soGameItemData= EditorGUIUtility.Load("Assets/Resources/GameData/Game Item Data.asset") as SO_GameItemData;
                ShowItemsDetails(_soGameItemData);
                break;
            case 1:
                _soCharacterData= EditorGUIUtility.Load("Assets/Resources/GameData/Character Data.asset") as SO_CharacterData;
                ShowCharacterData(_soCharacterData);
                break;
        }
    }
    private void ShowItemsDetails(SO_GameItemData _gameItemData)
    {
        if(_gameItemData == null) return;
        
        EditorGUI.BeginChangeCheck();
        
        Space(10);
        scrollView = GUILayout.BeginScrollView(scrollView);
        
        GUILayout.BeginHorizontal();
        for (var i = 0; i < _gameItemData.GameItemDatas.Count; i++)
        {
            if (i % 8 == 0 && i != 0)
            {
                GUILayout.EndHorizontal();
                Space(25);
                GUILayout.BeginHorizontal();
            }
            GUILayout.BeginVertical();
            var itemDefault = _gameItemData.GameItemDatas[i];
            itemDefault.code = (ItemNameCode)EditorGUILayout.EnumPopup("", itemDefault.code, Width(150));
            itemDefault.sprite = (Sprite)EditorGUILayout.ObjectField(itemDefault.sprite, typeof(Sprite), false, Width(150), Height(150));
            Space(2);
            itemDefault.ratity = (ItemRarity)EditorGUILayout.EnumPopup("", itemDefault.ratity, Width(150));
            GUILayout.EndVertical();
        }
        
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        
        Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            _gameItemData.GameItemDatas.Add(new ItemCustom());
        }
        GUILayout.Box("Add new Item");
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("-", Width(45), Height(25)) && _gameItemData.GameItemDatas.Count != 0)
        {
            _gameItemData.GameItemDatas.RemoveAt(_gameItemData.GameItemDatas.Count - 1);
        }
        GUILayout.Box("Remove Item");
        GUILayout.EndHorizontal();
        
        if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(_gameItemData);
    }
    private void ShowCharacterData(SO_CharacterData _characterData)
    {
        if(_characterData == null) return;
        
        EditorGUI.BeginChangeCheck();
        
        Space(10);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("NameCode", Width(150));
        GUILayout.Label("", Width(30));
        GUILayout.Label("Player Prefab", Width(150));
        GUILayout.EndHorizontal();
        
        scrollView = GUILayout.BeginScrollView(scrollView);
        foreach (var characterCustom in _characterData.CharactersData)
        {
            GUILayout.BeginHorizontal();
            characterCustom.nameCode = (CharacterNameCode)EditorGUILayout.EnumPopup("", characterCustom.nameCode, Width(150));
            GUILayout.Label("  ->  ", Width(30));
            characterCustom.prefab = (PlayerController)EditorGUILayout.ObjectField(characterCustom.prefab, typeof(PlayerController), false, Width(200));
            GUILayout.EndHorizontal();
            Space(10);
        }
        GUILayout.EndScrollView();
        
        Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            _characterData.CharactersData.Add(new CharacterCustom());
        }
        GUILayout.Box("Add new Character");
        GUILayout.EndHorizontal();
        
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("-", Width(45), Height(25)) && _characterData.CharactersData.Count != 0)
        {
            _characterData.CharactersData.RemoveAt(_characterData.CharactersData.Count - 1);
        }
        GUILayout.Box("Remove Character");
        GUILayout.EndHorizontal();
        
        if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(_characterData);
    }
    #endregion



    private static GUIStyle BoxColorText(Color _color) => new(GUI.skin.box) { normal = { textColor = _color } };
    private static GUIStyle LabelColorText(Color _color) => new(GUI.skin.label) { normal = { textColor = _color } };
    private static GUIStyle ButtonColorText(Color _color) => new(GUI.skin.button) { normal = { textColor = _color } };
    private static GUIStyle TextFieldColorText(Color _color) => new(EditorStyles.textField) { normal = { textColor = _color } };
    
    
    private static void Space(float space) => GUILayout.Space(space);
    private static GUILayoutOption Width(float width) => GUILayout.Width(width);
    private static GUILayoutOption Height(float height) => GUILayout.Height(height);

}
