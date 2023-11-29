using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


public class PlayFabHandleUserData : Singleton<PlayFabHandleUserData>
{
    public UserData UserData;
    private bool _isLogin;
    
    // PlayerFab KeyValue
    private const string UserData_Key = "USER_Data";
    
    private void Start()
    {
        _isLogin = false;
        PlayFabController.Instance.OnLoginSuccessEvent.AddListener(OnLoginSuccess);
    }
    private void OnDestroy()
    {
        PlayFabController.Instance.OnLoginSuccessEvent.RemoveListener(OnLoginSuccess);
    }

    // private void Update()
    // {
    //     if(!_isLogin) return;
    //     if (Input.GetKeyDown(KeyCode.S))
    //     {
    //         SetUserData();
    //     }
    //     if (Input.GetKeyDown(KeyCode.L))
    //     {
    //         GetUserData();
    //     }
    // }

    private void OnLoginSuccess()
    {
        _isLogin = true;
        GetUserData();
    }
    
    private void SetUserData()
    {
        var request = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {
                     UserData_Key,
                     JsonUtility.ToJson(UserData)
                }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, result => { Debug.Log("Update User Data Success"); }, ErrorCallback);
    }

    private void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnGetUserDataSuccess, ErrorCallback);
    }

    private void OnGetUserDataSuccess(GetUserDataResult _result)
    {
        Debug.Log("Get User Data Success");
        if (_result.Data != null && _result.Data.TryGetValue(UserData_Key, out var userDataRecord))
        {
            UserData = JsonUtility.FromJson<UserData>(userDataRecord.Value);
        }
        else
        {
            // UserData = new UserData(PlayFabController.Instance.username, 1000, CharacterTypeEnums.Arlan);
            // UserData.inventories.Add(new Inventory(ItemTypeEnums.POHealth, 5));
            // UserData.inventories.Add(new Inventory(ItemTypeEnums.POStamina, 5));
        }
    }

    private void ErrorCallback(PlayFabError _error) => Debug.LogWarning(_error.Error);


}
