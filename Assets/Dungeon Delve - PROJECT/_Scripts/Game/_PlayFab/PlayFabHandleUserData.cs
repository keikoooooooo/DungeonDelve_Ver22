using System.Collections.Generic;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;


public class PlayFabHandleUserData : Singleton<PlayFabHandleUserData>
{
    public UnityEvent OnLoadUserDataSuccessEvent;
    public UnityEvent OnLoadUserDataFailureEvent;
    
    private bool _isLogin;
    public UserData UserData;
    public SO_PlayerConfiguration PlayerConfig;
    
    public enum PF_Key : byte // PlayerFab KeyValue
    {
        UserData_Key,
        PlayerConfigData_Key
    }
    
    private void Start()
    {
        _isLogin = false;
        PlayFabController.Instance.OnLoginSuccessEvent += OnLoginSuccess;
    }
    private void OnDestroy()
    {
        PlayFabController.Instance.OnLoginSuccessEvent -= OnLoginSuccess;
    }

    
    private void OnLoginSuccess()
    {
        _isLogin = true;
        GetUserData();
    }


    /// <summary> Lưu dữ liệu người dùng. </summary>
    public void SaveData()
    {
        SetUserData(PF_Key.UserData_Key);
        SetUserData(PF_Key.PlayerConfigData_Key);
    }
    private void SetUserData(PF_Key _keySave)
    {
        if(!_isLogin) return;

        var jsonText = _keySave switch
        {
            PF_Key.UserData_Key => JsonConvert.SerializeObject(UserData, Formatting.Indented),
            PF_Key.PlayerConfigData_Key => JsonConvert.SerializeObject(PlayerConfig, Formatting.Indented),
            _ => ""
        };

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { _keySave.ToString(), jsonText }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, 
            _ =>  Debug.Log("Update Data Success: " + _keySave) , 
            _ => Debug.Log("Update Data Failure: " + _keySave) );
    }
    
    private void GetUserData()
    {
        if(!_isLogin) return;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnGetUserDataSuccess, ErrorCallback);
    }
    private void OnGetUserDataSuccess(GetUserDataResult _result)
    {
        if (_result.Data == null || !_result.Data.ContainsKey($"{PF_Key.UserData_Key}")) 
        {
            UserData = new UserData(PlayFabController.Instance.username, 5000);
            OnLoadUserDataFailureEvent?.Invoke();
            return;
        }
        
        if (_result.Data.TryGetValue($"{PF_Key.UserData_Key}", out var userDataRecord))
        {
            UserData = JsonConvert.DeserializeObject<UserData>(userDataRecord.Value);
        }
        if (_result.Data.TryGetValue($"{PF_Key.PlayerConfigData_Key}", out var playerConfigDataRecord))
        {
            PlayerConfig = JsonConvert.DeserializeObject<SO_PlayerConfiguration>(playerConfigDataRecord.Value);
        }
        
        OnLoadUserDataSuccessEvent?.Invoke();
        Debug.Log("Get Data Success");
    }
    
    private static void ErrorCallback(PlayFabError _error) => Debug.LogWarning(_error.Error);
}
