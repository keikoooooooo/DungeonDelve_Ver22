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

    private readonly string[] _toolTitles = { "PLAYERS", "ENEMIES" };
    private int _selectedTool = -1;
    private Vector2 scrollView;
    
    private void OnGUI()
    {
        _selectedTool = GUILayout.Toolbar(_selectedTool, _toolTitles, Width(200), Height(40));
        switch (_selectedTool)
        {
            case 0:
                HandlePanelPlayers();
                break;
            
            case 1:
                HandlePanelEnemies();
                break;
        }
    }
    
    
    #region PLAYERS
    private readonly string[] _playerNames = { "Arlan", "Lynx" };
    private int _selectedPlayer = -1;
    private void HandlePanelPlayers()
    {
        Space(10);
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
    }
    
    private PlayerConfiguration arlanConfig;
    private PlayerConfiguration lynxConfig;
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
        dataPlayerConfig.Infor = EditorGUILayout.TextField("Infor", dataPlayerConfig.Infor, Width(500));
        
        Space(30);
        GUILayout.Label("CHARACTER STATS ----------------------", EditorStyles.boldLabel);
        dataPlayerConfig.MaxHealth = EditorGUILayout.IntField("Max HP", dataPlayerConfig.MaxHealth, Width(500));
        dataPlayerConfig.MaxStamina = EditorGUILayout.IntField("Max ST", dataPlayerConfig.MaxStamina, Width(500));
        dataPlayerConfig.ATK = EditorGUILayout.IntField("ATK", dataPlayerConfig.ATK, Width(500));
        dataPlayerConfig.CRITRate = EditorGUILayout.FloatField("CRIT Rate", dataPlayerConfig.CRITRate, Width(500));
        dataPlayerConfig.CRITDMG = EditorGUILayout.IntField("CRIT DMG", dataPlayerConfig.CRITDMG, Width(500));
        dataPlayerConfig.DEF = EditorGUILayout.IntField("DEF", dataPlayerConfig.DEF, Width(500));
        dataPlayerConfig.WalkSpeed = EditorGUILayout.FloatField("Walk Speed", dataPlayerConfig.WalkSpeed, Width(500));
        dataPlayerConfig.RunSpeed = EditorGUILayout.FloatField("Run Speed", dataPlayerConfig.RunSpeed, Width(500));
        dataPlayerConfig.RunFastSpeed = EditorGUILayout.FloatField("Run Fast Speed", dataPlayerConfig.RunFastSpeed, Width(500));
        dataPlayerConfig.DashEnergy = EditorGUILayout.IntField("Dash Energy", dataPlayerConfig.DashEnergy, Width(500));
        dataPlayerConfig.JumpHeight = EditorGUILayout.FloatField("Jump Height", dataPlayerConfig.JumpHeight, Width(500));

        Space(30);
        GUILayout.Label("WEAPON STATS -------------------------", EditorStyles.boldLabel);
        dataPlayerConfig.WeaponLevel = EditorGUILayout.IntField("Weapon Level", dataPlayerConfig.WeaponLevel, Width(500));
        
        Space(30);
        GUILayout.Label("COOLDOWN -------------------------", EditorStyles.boldLabel);
        dataPlayerConfig.JumpCD = EditorGUILayout.FloatField("Jump CD", dataPlayerConfig.JumpCD, Width(500));
        dataPlayerConfig.SkillCD = EditorGUILayout.FloatField("Elemental Skill CD", dataPlayerConfig.SkillCD, Width(500));
        dataPlayerConfig.SpecialCD = EditorGUILayout.FloatField("Elemental Burst CD", dataPlayerConfig.SpecialCD, Width(500));
        
        Space(30);
        GUILayout.Label("MULTIPLIER -------------------------", EditorStyles.boldLabel);
        
        #region Normal Attack
        Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("NORMAL ATTACK MULTIPLIER", Width(200), Height(25));
        for (var i = 1; i <= 10; i++)
        {
            GUILayout.Box($"{i}", Width(60), Height(25));
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
                    EditorGUILayout.FloatField("", dataPlayerConfig.NormalAttackMultiplier[i].Multiplier[j], EditorStyles.numberField, Width(60), Height(27));
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
            GUILayout.Box($"{i}", Width(60), Height(25));
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
                        EditorStyles.numberField, Width(60), Height(27));
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
            GUILayout.Box($"{i}", Width(60), Height(25));
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
                        EditorStyles.numberField, Width(60), Height(27));
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
            GUILayout.Box($"{i}", Width(60), Height(25));
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
                        EditorStyles.numberField, Width(60), Height(27));
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

        #region Character Upgrade
        Space(30);
        GUILayout.Label("CHARACTER UPGRADE -------------------------", EditorStyles.boldLabel);
        Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("", Width(70), Height(25));
        
        GUILayout.Box("Level Upgrade", Width(150), Height(25));
        GUILayout.Box("Upgrade Cost", Width(150), Height(25));
        GUILayout.Box("Max Experience Upgrade", Width(150), Height(25));
        GUILayout.Box("Value", Width(150), Height(25));
        GUILayout.Box("Type", Width(150), Height(25));
        GUILayout.EndHorizontal();
        
        for (var i = 0; i < dataPlayerConfig.CharacterUpgrade.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Box($"{i}", Width(70));
            var playerUpgrade = dataPlayerConfig.CharacterUpgrade[i];
            playerUpgrade.LevelUpgrade = EditorGUILayout.IntField("", playerUpgrade.LevelUpgrade, Width(150));
            playerUpgrade.UpgradeCost = EditorGUILayout.IntField("", playerUpgrade.UpgradeCost, Width(150));
            playerUpgrade.MaxExperienceUpgrade = EditorGUILayout.IntField("", playerUpgrade.MaxExperienceUpgrade, Width(150));
            
            foreach (var Materials in playerUpgrade.MaterialsUpgrade)
            {
                Materials.Value = EditorGUILayout.IntField("", Materials.Value, Width(150));
                Materials.Type = (ItemType)EditorGUILayout.EnumPopup("", Materials.Type, Width(150));
            }
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+", Width(45)))
            {
                var itemUpgrade = new ItemUpgrade();
                playerUpgrade.MaterialsUpgrade.Add(itemUpgrade);
            }
            if(GUILayout.Button("-", Width(45)) && playerUpgrade.MaterialsUpgrade.Count != 0)
            {
                playerUpgrade.MaterialsUpgrade.Remove(playerUpgrade.MaterialsUpgrade[^1]);
            }
            GUILayout.EndHorizontal();

            
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            var playerUpgrade = new PlayerUpgrade
            {
                MaterialsUpgrade = new List<ItemUpgrade>()
            };
            dataPlayerConfig.CharacterUpgrade.Add(playerUpgrade);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && dataPlayerConfig.CharacterUpgrade.Count != 0)
        {
            dataPlayerConfig.CharacterUpgrade.Remove(dataPlayerConfig.CharacterUpgrade[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        #region Weapon Upgrade
        Space(30);
        GUILayout.Label("WEAPON UPGRADE -------------------------", EditorStyles.boldLabel);
        Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Label("", Width(70), Height(25));
        
        GUILayout.Box("Level Upgrade", Width(150), Height(25));
        GUILayout.Box("Upgrade Cost", Width(150), Height(25));
        GUILayout.Box("Max Experience Upgrade", Width(150), Height(25));
        GUILayout.Box("Value", Width(150), Height(25));
        GUILayout.Box("Type", Width(150), Height(25));
        GUILayout.EndHorizontal();
        
        for (var i = 0; i < dataPlayerConfig.WeaponUpgrade.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Box($"{i}", Width(70));
            var weaponUpgrade = dataPlayerConfig.WeaponUpgrade[i];
            weaponUpgrade.LevelUpgrade = EditorGUILayout.IntField("", weaponUpgrade.LevelUpgrade, Width(150));
            weaponUpgrade.UpgradeCost = EditorGUILayout.IntField("", weaponUpgrade.UpgradeCost, Width(150));
            weaponUpgrade.MaxExperienceUpgrade = EditorGUILayout.IntField("", weaponUpgrade.MaxExperienceUpgrade, Width(150));
            
            foreach (var Materials in weaponUpgrade.MaterialsUpgrade)
            {
                Materials.Value = EditorGUILayout.IntField("", Materials.Value, Width(150));
                Materials.Type = (ItemType)EditorGUILayout.EnumPopup("", Materials.Type, Width(150));
            }
            
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+", Width(45)))
            {
                var itemUpgrade = new ItemUpgrade();
                weaponUpgrade.MaterialsUpgrade.Add(itemUpgrade);
            }
            if(GUILayout.Button("-", Width(45)) && weaponUpgrade.MaterialsUpgrade.Count != 0)
            {
                weaponUpgrade.MaterialsUpgrade.Remove(weaponUpgrade.MaterialsUpgrade[^1]);
            }
            GUILayout.EndHorizontal();

            
            GUILayout.EndHorizontal();
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+", Width(45), Height(25)))
        {
            var weaponUpgrade = new PlayerUpgrade
            {
                MaterialsUpgrade = new List<ItemUpgrade>()
            };
            dataPlayerConfig.WeaponUpgrade.Add(weaponUpgrade);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && dataPlayerConfig.WeaponUpgrade.Count != 0)
        {
            dataPlayerConfig.WeaponUpgrade.Remove(dataPlayerConfig.WeaponUpgrade[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(dataPlayerConfig);
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
        _selectedEnemy = GUILayout.Toolbar(_selectedEnemy, _enemiesNames, Width(200), Height(30));
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
        _enemyConfiguration.MaxHealth = EditorGUILayout.IntField("Max HP", _enemyConfiguration.MaxHealth, Width(500));
        _enemyConfiguration.ATK = EditorGUILayout.IntField("ATK", _enemyConfiguration.ATK, Width(500));
        _enemyConfiguration.DEF = EditorGUILayout.IntField("DEF", _enemyConfiguration.DEF, Width(500));
        _enemyConfiguration.WalkSpeed = EditorGUILayout.FloatField("Walk Speed", _enemyConfiguration.WalkSpeed, Width(500));
        _enemyConfiguration.RunSpeed = EditorGUILayout.FloatField("Run Speed", _enemyConfiguration.RunSpeed, Width(500));
        
        Space(30);
        GUILayout.Label("COOLDOWN -------------------------", EditorStyles.boldLabel);
        _enemyConfiguration.NormalAttackCD = EditorGUILayout.FloatField("Normal Attack CD", _enemyConfiguration.NormalAttackCD, Width(500));
        _enemyConfiguration.SkillAttackCD = EditorGUILayout.FloatField("Skill Attack CD", _enemyConfiguration.SkillAttackCD, Width(500));
       
        Space(30);
        if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(_enemyConfiguration);
        GUILayout.EndScrollView();
    }
    
    #endregion


    

    private static void Space(float space) => GUILayout.Space(space);
    private static GUILayoutOption Width(float width) => GUILayout.Width(width);
    private static GUILayoutOption Height(float height) => GUILayout.Height(height);

}
