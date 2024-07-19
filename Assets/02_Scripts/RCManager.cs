using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class RCManager : MonoBehaviour
{
    [SerializeField] private float mummyScale;

    private async Task Awake()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Player ID :" + AuthenticationService.Instance.PlayerId);
        };
        // 로그인
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        // Remote Config 이벤트 연결
        RemoteConfigService.Instance.FetchCompleted += (response) =>
        {
            mummyScale = RemoteConfigService.Instance.appConfig.GetFloat("mummy_scale");
            Debug.Log("Mummy Scale :" + mummyScale);
        };

        // 리모트 값 조회
        await GetRemoteConfigData();
    }

    public struct userAttributes { };
    public struct appAttributes { };

    private async Task GetRemoteConfigData()
    {
        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
    }
}
