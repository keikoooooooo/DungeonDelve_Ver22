using System;
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
    
    public UserData UserData;
    public PlayerConfiguration PlayerConfig;
    
    private bool _isLogin;
    
  
    public enum PF_Key : byte // PlayerFab KeyValue
    {
        UserData_Key,
        PlayerConfigData_Key
    }
    
    
    private void Start()
    {
        _isLogin = false;
        PlayerConfig = ScriptableObject.CreateInstance<PlayerConfiguration>();
        PlayFabController.Instance.OnLoginSuccessEvent.AddListener(OnLoginSuccess);
    }
    private void OnDestroy()
    {
        PlayFabController.Instance.OnLoginSuccessEvent.RemoveListener(OnLoginSuccess);
    }
    
    private void OnLoginSuccess()
    {
        _isLogin = true;
        GetUserData();
    }
    
    public void SetUserData(PF_Key _keySave)
    {
        if(!_isLogin) return;

        var jsonText = "";
        switch (_keySave)
        {
            case PF_Key.UserData_Key:
                jsonText = JsonConvert.SerializeObject(UserData, Formatting.Indented);
                break;
            case PF_Key.PlayerConfigData_Key:
                jsonText = JsonConvert.SerializeObject(PlayerConfig, Formatting.Indented);
                
                break;
            default:
                Debug.LogWarning("Non save data to playFab");
                break;
        }

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {
                    _keySave.ToString(),
                    jsonText
                }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, _resul =>
        {
            Debug.Log("Player Update User Data Success");
            GetUserData();
        }, ErrorCallback);
    }
    public void SetUserData<T> (T _data, PF_Key _keySave)
    {
        if(!_isLogin) return;

        var jsonText = JsonUtility.ToJson(_data, true);
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {
                    _keySave.ToString(),
                    jsonText
                }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, _result =>
        {
            Debug.Log("Player Update User Data Success");
            GetUserData();
        }, ErrorCallback);
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
            Debug.Log("Get Data Failure.\nCreate new Userdata");

            OnLoadUserDataFailureEvent?.Invoke();
            return;
        }
        
        OnLoadUserDataSuccessEvent?.Invoke();
        Debug.Log("Get Data Success");
        
        if (_result.Data.TryGetValue($"{PF_Key.UserData_Key}", out var userDataRecord))
        {
            UserData = JsonConvert.DeserializeObject<UserData>(userDataRecord.Value);
        }
        if (_result.Data.TryGetValue($"{PF_Key.PlayerConfigData_Key}", out var playerConfigDataRecord))
        {
            PlayerConfig = JsonConvert.DeserializeObject<PlayerConfiguration>(playerConfigDataRecord.Value);
        }
    }
    
    
    private static void ErrorCallback(PlayFabError _error) => Debug.LogWarning(_error.Error);
}
