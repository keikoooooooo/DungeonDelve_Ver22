using System;
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
    
    private PlayerConfiguration arlanConfig;
    private PlayerConfiguration lynxConfig;
    private UpgradeData _upgradeData;
    
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
                        arlanConfig = EditorGUIUtility.Load("Assets/Resources/Player/1. Arlan/Prefab/Arlan Config.asset") as PlayerConfiguration;
                        ShowPlayerConfig(arlanConfig);
                        break;
                    
                    case 1:
                        lynxConfig = EditorGUIUtility.Load("Assets/Resources/Player/2. Lynx/Prefab/Lynx Config.asset") as PlayerConfiguration;
                        ShowPlayerConfig(lynxConfig);
                        break;
                }
                break;
            case 1:
                _upgradeData = EditorGUIUtility.Load("Assets/Resources/Player/Character Upgrade Data.asset") as UpgradeData;
                ShowUpgradeDetails(_upgradeData);
                break;

        }
    }
    private void ShowPlayerConfig(PlayerConfiguration dataPlayerConfig) 
    {
        if (dataPlayerConfig == null)
        {
            EditorGUILayout.HelpBox("Assign a PlayerConfig.", MessageType.Warning);
            return;
        }

        scrollView = GUILayout.BeginScrollView(scrollView);
        EditorGUI.BeginChangeCheck();
        
        Space(30);
        GUILayout.Label("INFORMATION ------------------------", EditorStyles.boldLabel);
        dataPlayerConfig.Name = EditorGUILayout.TextField("Name", dataPlayerConfig.Name, Width(500));
        dataPlayerConfig.Level = EditorGUILayout.IntField("Level", dataPlayerConfig.Level, Width(500));
        dataPlayerConfig.CurrentEXP = EditorGUILayout.IntField("Current EXP", dataPlayerConfig.CurrentEXP, Width(500));
        dataPlayerConfig.Infor = EditorGUILayout.TextField("Infor", dataPlayerConfig.Infor, Width(500));
        
        Space(30);
        GUILayout.Label("CHARACTER STATS ----------------------", EditorStyles.boldLabel);
        dataPlayerConfig.MaxHP = EditorGUILayout.IntField("Max HP", dataPlayerConfig.MaxHP, Width(500));
        dataPlayerConfig.MaxST = EditorGUILayout.IntField("Max ST", dataPlayerConfig.MaxST, Width(500));
        dataPlayerConfig.ATK = EditorGUILayout.IntField("ATK", dataPlayerConfig.ATK, Width(500));
        dataPlayerConfig.CRITRate = EditorGUILayout.FloatField("CRIT Rate(%)", dataPlayerConfig.CRITRate, Width(500));
        dataPlayerConfig.CRITDMG = EditorGUILayout.IntField("CRIT DMG(%)", dataPlayerConfig.CRITDMG, Width(500));
        dataPlayerConfig.DEF = EditorGUILayout.IntField("DEF", dataPlayerConfig.DEF, Width(500));
        dataPlayerConfig.ChargedAttackStaminaCost = EditorGUILayout.IntField("Charged Attack ST Cost", dataPlayerConfig.ChargedAttackStaminaCost, Width(500));
        dataPlayerConfig.WalkSpeed = EditorGUILayout.FloatField("Walk Speed", dataPlayerConfig.WalkSpeed, Width(500));
        dataPlayerConfig.RunSpeed = EditorGUILayout.FloatField("Run Speed", dataPlayerConfig.RunSpeed, Width(500));
        dataPlayerConfig.RunFastSpeed = EditorGUILayout.FloatField("Run Fast Speed", dataPlayerConfig.RunFastSpeed, Width(500));
        dataPlayerConfig.DashEnergy = EditorGUILayout.IntField("Dash Energy", dataPlayerConfig.DashEnergy, Width(500));
        dataPlayerConfig.JumpHeight = EditorGUILayout.FloatField("Jump Height", dataPlayerConfig.JumpHeight, Width(500));

        Space(30);
        GUILayout.Label("WEAPON -------------------------", EditorStyles.boldLabel);
        dataPlayerConfig.WeaponName = EditorGUILayout.TextField("Weapon Name", dataPlayerConfig.WeaponName, Width(500));
        dataPlayerConfig.WeaponInfo = EditorGUILayout.TextField("Weapon Info", dataPlayerConfig.WeaponInfo, Width(500));
        dataPlayerConfig.WeaponLevel = EditorGUILayout.IntSlider("Weapon Level", dataPlayerConfig.WeaponLevel, 1, 10, Width(500));
        
        Space(30);
        GUILayout.Label("COOLDOWN -------------------------", EditorStyles.boldLabel);
        dataPlayerConfig.JumpCD = EditorGUILayout.FloatField("Jump CD(s)", dataPlayerConfig.JumpCD, Width(500));
        dataPlayerConfig.ElementalSkillCD = EditorGUILayout.FloatField("Elemental Skill CD(s)", dataPlayerConfig.ElementalSkillCD, Width(500));
        dataPlayerConfig.ElementalBurstlCD = EditorGUILayout.FloatField("Elemental Burst CD(s)", dataPlayerConfig.ElementalBurstlCD, Width(500));
        
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
        
        for (var i = 0; i < dataPlayerConfig.NormalAttackMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            dataPlayerConfig.NormalAttackMultiplier[i].MultiplierTypeName = $"{i + 1} - Hit DMG (%)";
            GUILayout.Box($"{i + 1} - Hit DMG (%)" ,Width(200), Height(27));
            for (var j = 0; j < dataPlayerConfig.NormalAttackMultiplier[i].Multiplier.Count; j++)
            {
                dataPlayerConfig.NormalAttackMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", dataPlayerConfig.NormalAttackMultiplier[i].Multiplier[j], EditorStyles.numberField, Width(65), Height(27));
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
            dataPlayerConfig.NormalAttackMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && dataPlayerConfig.NormalAttackMultiplier.Count != 0)
        {
            dataPlayerConfig.NormalAttackMultiplier.Remove(dataPlayerConfig.NormalAttackMultiplier[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        #region Charged Attack
        Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("CHARGED ATTACK MULTIPLIER", Width(200), Height(25));
        for (var i = 1; i <= 10; i++)
        {
            GUILayout.Box($"Wea Lv{i}", Width(65), Height(25));
        }
        GUILayout.EndHorizontal();
        
        for (var i = 0; i < dataPlayerConfig.ChargedAttackMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            dataPlayerConfig.ChargedAttackMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", dataPlayerConfig.ChargedAttackMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < dataPlayerConfig.ChargedAttackMultiplier[i].Multiplier.Count; j++)
            {
                dataPlayerConfig.ChargedAttackMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", dataPlayerConfig.ChargedAttackMultiplier[i].Multiplier[j],
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
            dataPlayerConfig.ChargedAttackMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && dataPlayerConfig.ChargedAttackMultiplier.Count != 0)
        {
            dataPlayerConfig.ChargedAttackMultiplier.Remove(dataPlayerConfig.ChargedAttackMultiplier[^1]);
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
        
        for (var i = 0; i < dataPlayerConfig.SkillMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            dataPlayerConfig.SkillMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", dataPlayerConfig.SkillMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < dataPlayerConfig.SkillMultiplier[i].Multiplier.Count; j++)
            {
                dataPlayerConfig.SkillMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", dataPlayerConfig.SkillMultiplier[i].Multiplier[j],
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
            dataPlayerConfig.SkillMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && dataPlayerConfig.SkillMultiplier.Count != 0)
        {
            dataPlayerConfig.SkillMultiplier.Remove(dataPlayerConfig.SkillMultiplier[^1]);
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
        
        for (var i = 0; i < dataPlayerConfig.SpecialMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            dataPlayerConfig.SpecialMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", dataPlayerConfig.SpecialMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < dataPlayerConfig.SpecialMultiplier[i].Multiplier.Count; j++)
            {
                dataPlayerConfig.SpecialMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", dataPlayerConfig.SpecialMultiplier[i].Multiplier[j],
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
            dataPlayerConfig.SpecialMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && dataPlayerConfig.SpecialMultiplier.Count != 0)
        {
            dataPlayerConfig.SpecialMultiplier.Remove(dataPlayerConfig.SpecialMultiplier[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(dataPlayerConfig);
        GUILayout.EndScrollView();
    }
    private void ShowUpgradeDetails(UpgradeData upgradeData)
    {
        if(_upgradeData == null) 
            return;
        _upgradeData.SetData();   
        
        Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Box("Level", Width(150));
        GUILayout.Box("To Next", Width(150));
        GUILayout.Box("Total EXP", Width(150));
        GUILayout.EndHorizontal();
        
        scrollView = GUILayout.BeginScrollView(scrollView);
        for (var i = 0; i < _upgradeData.defaultDatas.Count; i++)
        {
            int totalEXP = i == 0 ? 0 : _upgradeData.defaultDatas[i].EXP + _upgradeData.defaultDatas[i - 1].EXP;
            GUILayout.BeginHorizontal();
            _upgradeData.defaultDatas[i].Level = EditorGUILayout.IntField("", _upgradeData.defaultDatas[i].Level, Width(150));
            _upgradeData.defaultDatas[i].EXP = EditorGUILayout.IntField("", _upgradeData.defaultDatas[i].EXP, Width(150));
            GUILayout.Box($"{totalEXP}" , Width(150));
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
    
    private EnemyConfiguration goblin_SwordConfig;
    private EnemyConfiguration goblin_SlingshotConfig;
    private EnemyConfiguration goblin_DaggersConfig;
    private EnemyConfiguration BOReaperCongfig;
    
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
                        goblin_SwordConfig = EditorGUIUtility.Load("Assets/Resources/Enemies/Goblin/Sword/Prefab/Goblin_Sword Config.asset") as EnemyConfiguration;
                        ShowEnemyConfig(goblin_SwordConfig);
                        break;
                    case 1: 
                        goblin_SlingshotConfig = EditorGUIUtility.Load("Assets/Resources/Enemies/Goblin/Slingshot/Prefab/Goblin_Slingshot Config.asset") as EnemyConfiguration;
                        ShowEnemyConfig(goblin_SlingshotConfig);
                        break;
                    case 2: 
                        goblin_DaggersConfig = EditorGUIUtility.Load("Assets/Resources/Enemies/Goblin/Daggers/Prefab/Goblin_Daggers Config.asset") as EnemyConfiguration;
                        ShowEnemyConfig(goblin_DaggersConfig);
                        break;
                }
                break;
            
            case 1: 
                BOReaperCongfig = EditorGUIUtility.Load("Assets/Resources/Enemies/Reaper/Prefab/BOReaper Config.asset") as EnemyConfiguration;
                ShowEnemyConfig(BOReaperCongfig);
                break;
        }
    }
    private void ShowEnemyConfig(EnemyConfiguration _enemyConfiguration)
    {
        if (_enemyConfiguration == null)
        {
            EditorGUILayout.HelpBox("Assign a EnemyConfig.", MessageType.Warning);
            return;
        }

        scrollView = GUILayout.BeginScrollView(scrollView);
        EditorGUI.BeginChangeCheck();
        
        Space(30);
        GUILayout.Label("INFORMATION ------------------------", EditorStyles.boldLabel);
        _enemyConfiguration.Name = EditorGUILayout.TextField("Name", _enemyConfiguration.Name, Width(500));
        _enemyConfiguration.Level = EditorGUILayout.IntField("Level", _enemyConfiguration.Level, Width(500));
        _enemyConfiguration.Infor = EditorGUILayout.TextField("Infor", _enemyConfiguration.Infor, Width(500));
        
        Space(30);
        GUILayout.Label("STATS -------------------------------", EditorStyles.boldLabel);
        _enemyConfiguration.MaxHP = EditorGUILayout.IntField("Max HP", _enemyConfiguration.MaxHP, Width(500));
        _enemyConfiguration.ATK = EditorGUILayout.IntField("ATK", _enemyConfiguration.ATK, Width(500));
        _enemyConfiguration.CRITRate = EditorGUILayout.FloatField("CRIT Rate(%)", _enemyConfiguration.CRITRate, Width(500));
        _enemyConfiguration.CRITDMG = EditorGUILayout.IntField("CRIT DMG(%)", _enemyConfiguration.CRITDMG, Width(500));
        _enemyConfiguration.DEF = EditorGUILayout.IntField("DEF", _enemyConfiguration.DEF, Width(500));
        _enemyConfiguration.WalkSpeed = EditorGUILayout.FloatField("Walk Speed", _enemyConfiguration.WalkSpeed, Width(500));
        _enemyConfiguration.RunSpeed = EditorGUILayout.FloatField("Run Speed", _enemyConfiguration.RunSpeed, Width(500));
        
        Space(30);
        GUILayout.Label("COOLDOWN -------------------------", EditorStyles.boldLabel);
        _enemyConfiguration.NormalAttackCD = EditorGUILayout.FloatField("Normal Attack CD(s)", _enemyConfiguration.NormalAttackCD, Width(500));
        _enemyConfiguration.SkillAttackCD = EditorGUILayout.FloatField("Skill Attack CD(s)", _enemyConfiguration.SkillAttackCD, Width(500));
        _enemyConfiguration.SpecialAttackCD = EditorGUILayout.FloatField("Special Attack CD(s)", _enemyConfiguration.SpecialAttackCD, Width(500));
        
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
        
        for (var i = 0; i < _enemyConfiguration.NormalAttackMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            _enemyConfiguration.NormalAttackMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", _enemyConfiguration.NormalAttackMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < _enemyConfiguration.NormalAttackMultiplier[i].Multiplier.Count; j++)
            {
                _enemyConfiguration.NormalAttackMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", _enemyConfiguration.NormalAttackMultiplier[i].Multiplier[j], EditorStyles.numberField, Width(80), Height(27));
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
            _enemyConfiguration.NormalAttackMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && _enemyConfiguration.NormalAttackMultiplier.Count != 0)
        {
            _enemyConfiguration.NormalAttackMultiplier.Remove(_enemyConfiguration.NormalAttackMultiplier[^1]);
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
        
        for (var i = 0; i < _enemyConfiguration.SkillMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            _enemyConfiguration.SkillMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", _enemyConfiguration.SkillMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < _enemyConfiguration.SkillMultiplier[i].Multiplier.Count; j++)
            {
                _enemyConfiguration.SkillMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", _enemyConfiguration.SkillMultiplier[i].Multiplier[j],
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
            _enemyConfiguration.SkillMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && _enemyConfiguration.SkillMultiplier.Count != 0)
        {
            _enemyConfiguration.SkillMultiplier.Remove(_enemyConfiguration.SkillMultiplier[^1]);
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
        
        for (var i = 0; i < _enemyConfiguration.SpecialMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            _enemyConfiguration.SpecialMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", _enemyConfiguration.SpecialMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < _enemyConfiguration.SpecialMultiplier[i].Multiplier.Count; j++)
            {
                _enemyConfiguration.SpecialMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", _enemyConfiguration.SpecialMultiplier[i].Multiplier[j],
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
            _enemyConfiguration.SpecialMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && _enemyConfiguration.SpecialMultiplier.Count != 0)
        {
            _enemyConfiguration.SpecialMultiplier.Remove(_enemyConfiguration.SpecialMultiplier[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(_enemyConfiguration);
        GUILayout.EndScrollView();
    }
    
    #endregion


    #region GAMECUSTOM
    
    private GameItemData _gameItemData;
    private int _selectedPanelGameCustomType = -1;
    private readonly string[] _gameCustomtype = { "ITEM DATA" };
    
    private void HandlePanelGameCustom()
    {
        Space(15);
        _selectedPanelGameCustomType = GUILayout.Toolbar(_selectedPanelGameCustomType, _gameCustomtype, Width(250), Height(35));
        switch (_selectedPanelGameCustomType)
        {
            case 0:
                _gameItemData= EditorGUIUtility.Load("Assets/Resources/Player/Game Item Data.asset") as GameItemData;
                ShowItemsDetails(_gameItemData);
                break;
        }
    }
    private void ShowItemsDetails(GameItemData gameItemData)
    {
        if(gameItemData == null) return;
        
        EditorGUI.BeginChangeCheck();
        
        Space(10);
        scrollView = GUILayout.BeginScrollView(scrollView);
        
        GUILayout.BeginHorizontal();
        for (var i = 0; i < gameItemData.GameItemDatas.Count; i++)
        {
            if (i % 8 == 0 && i != 0)
            {
                GUILayout.EndHorizontal();
                Space(10);
                GUILayout.BeginHorizontal();
            }
            GUILayout.BeginVertical();
            var itemDefault = gameItemData.GameItemDatas[i];
            itemDefault.Type = (ItemType)EditorGUILayout.EnumPopup("", itemDefault.Type, Width(100));
            itemDefault.Sprite = (Sprite)EditorGUILayout.ObjectField(itemDefault.Sprite, typeof(Sprite), false, Width(100), Height(100));
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        
        Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            gameItemData.GameItemDatas.Add(new ItemCustom());
        }
        GUILayout.Box("Add new Item");
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("-", Width(45), Height(25)) && gameItemData.GameItemDatas.Count != 0)
        {
            gameItemData.GameItemDatas.RemoveAt(gameItemData.GameItemDatas.Count - 1);
        }
        GUILayout.Box("Remove Item");
        GUILayout.EndHorizontal();
        
        if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(gameItemData);
    }
    #endregion

    

    private static void Space(float space) => GUILayout.Space(space);
    private static GUILayoutOption Width(float width) => GUILayout.Width(width);
    private static GUILayoutOption Height(float height) => GUILayout.Height(height);

}
