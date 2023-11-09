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
                
                break;
        }
    }


    #region PLAYERS
    private readonly string[] _playerNames = { "Arlan", "Lynx" };
    private int _selectedPlayer = -1;
    private void HandlePanelPlayers()
    {
        Space(10);
        _selectedPlayer = GUILayout.Toolbar(_selectedPlayer, _playerNames, Width(160), Height(30));

        switch (_selectedPlayer)
        {
            case 0:
                arlanConfig = (PlayerConfiguration)EditorGUILayout.ObjectField("Character Configuration", arlanConfig, typeof(CharacterConfiguration), false, Width(700));
                dataConfig = arlanConfig;
                ShowPlayerConfig();
                break;
            case 1:
                lynxConfig = (PlayerConfiguration)EditorGUILayout.ObjectField("Character Configuration", lynxConfig, typeof(CharacterConfiguration), false, Width(700));
                dataConfig = lynxConfig;
                ShowPlayerConfig();
                break;
        }
    }
    
    private PlayerConfiguration arlanConfig;
    private PlayerConfiguration lynxConfig;
    private PlayerConfiguration dataConfig;

    private void ShowPlayerConfig()
    {
        if (dataConfig == null)
        {
            EditorGUILayout.HelpBox("Assign a PlayerConfig.", MessageType.Warning);
            return;
        }

        scrollView = GUILayout.BeginScrollView(scrollView);
        EditorGUI.BeginChangeCheck();
        
        Space(30);
        GUILayout.Label("INFORMATION ------------------------", EditorStyles.boldLabel);
        dataConfig.Name = EditorGUILayout.TextField("Name", dataConfig.Name, Width(500));
        dataConfig.Level = EditorGUILayout.IntField("Level", dataConfig.Level, Width(500));
        dataConfig.Infor = EditorGUILayout.TextField("Infor", dataConfig.Infor, Width(500));
        
        Space(30);
        GUILayout.Label("STATS -------------------------------", EditorStyles.boldLabel);
        dataConfig.MaxHealth = EditorGUILayout.IntField("Max HP", dataConfig.MaxHealth, Width(500));
        dataConfig.MaxStamina = EditorGUILayout.IntField("Max ST", dataConfig.MaxStamina, Width(500));
        dataConfig.ATK = EditorGUILayout.IntField("ATK", dataConfig.ATK, Width(500));
        dataConfig.DEF = EditorGUILayout.IntField("DEF", dataConfig.DEF, Width(500));
        dataConfig.WalkSpeed = EditorGUILayout.FloatField("Walk Speed", dataConfig.WalkSpeed, Width(500));
        dataConfig.RunSpeed = EditorGUILayout.FloatField("Run Speed", dataConfig.RunSpeed, Width(500));
        dataConfig.RunFastSpeed = EditorGUILayout.FloatField("Run Fast Speed", dataConfig.RunFastSpeed, Width(500));
        dataConfig.DashEnergy = EditorGUILayout.IntField("Dash Energy", dataConfig.DashEnergy, Width(500));
        dataConfig.JumpHeight = EditorGUILayout.FloatField("Jump Height", dataConfig.JumpHeight, Width(500));
        
        Space(30);
        GUILayout.Label("COOLDOWN -------------------------", EditorStyles.boldLabel);
        dataConfig.JumpCD = EditorGUILayout.FloatField("Jump CD", dataConfig.JumpCD, Width(500));
        dataConfig.SkillCD = EditorGUILayout.FloatField("Skill CD", dataConfig.SkillCD, Width(500));
        dataConfig.SpecialCD = EditorGUILayout.FloatField("Special CD", dataConfig.SpecialCD, Width(500));
        
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
        
        for (var i = 0; i < dataConfig.NormalAttackMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            dataConfig.NormalAttackMultiplier[i].MultiplierTypeName = $"{i + 1} - Hit DMG (%)";
            GUILayout.Box($"{i + 1} - Hit DMG (%)" ,Width(200), Height(27));
            for (var j = 0; j < dataConfig.NormalAttackMultiplier[i].Multiplier.Count; j++)
            {
                dataConfig.NormalAttackMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", dataConfig.NormalAttackMultiplier[i].Multiplier[j], EditorStyles.numberField, Width(60), Height(27));
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
            dataConfig.NormalAttackMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && dataConfig.NormalAttackMultiplier.Count != 0)
        {
            dataConfig.NormalAttackMultiplier.Remove(dataConfig.NormalAttackMultiplier[^1]);
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
        
        for (var i = 0; i < dataConfig.ChargedAttackMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            dataConfig.ChargedAttackMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", dataConfig.ChargedAttackMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < dataConfig.ChargedAttackMultiplier[i].Multiplier.Count; j++)
            {
                dataConfig.ChargedAttackMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", dataConfig.ChargedAttackMultiplier[i].Multiplier[j],
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
            dataConfig.ChargedAttackMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && dataConfig.ChargedAttackMultiplier.Count != 0)
        {
            dataConfig.ChargedAttackMultiplier.Remove(dataConfig.ChargedAttackMultiplier[^1]);
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
        
        for (var i = 0; i < dataConfig.SkillMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            dataConfig.SkillMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", dataConfig.SkillMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < dataConfig.SkillMultiplier[i].Multiplier.Count; j++)
            {
                dataConfig.SkillMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", dataConfig.SkillMultiplier[i].Multiplier[j],
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
            dataConfig.SkillMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && dataConfig.SkillMultiplier.Count != 0)
        {
            dataConfig.SkillMultiplier.Remove(dataConfig.SkillMultiplier[^1]);
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
        
        for (var i = 0; i < dataConfig.SpecialMultiplier.Count; i++)
        {
            GUILayout.BeginHorizontal();
            dataConfig.SpecialMultiplier[i].MultiplierTypeName = 
                EditorGUILayout.TextField($"", dataConfig.SpecialMultiplier[i].MultiplierTypeName ,Width(202), Height(27));
            for (var j = 0; j < dataConfig.SpecialMultiplier[i].Multiplier.Count; j++)
            {
                dataConfig.SpecialMultiplier[i].Multiplier[j] = 
                    EditorGUILayout.FloatField("", dataConfig.SpecialMultiplier[i].Multiplier[j],
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
            dataConfig.SpecialMultiplier.Add(FloatMultiplier);
        }
        if(GUILayout.Button("-", Width(45), Height(25)) && dataConfig.SpecialMultiplier.Count != 0)
        {
            dataConfig.SpecialMultiplier.Remove(dataConfig.SpecialMultiplier[^1]);
        }
        GUILayout.EndHorizontal();
        #endregion
        
        if(EditorGUI.EndChangeCheck()) EditorUtility.SetDirty(dataConfig);
        
        GUILayout.EndScrollView();
    }
    #endregion







    private static void Space(float space) => GUILayout.Space(space);
    private static GUILayoutOption Width(float width) => GUILayout.Width(width);
    private static GUILayoutOption Height(float height) => GUILayout.Height(height);

}
