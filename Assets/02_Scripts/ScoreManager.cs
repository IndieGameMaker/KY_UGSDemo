using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private string playerId;

    [Header("UI Setting")]
    [SerializeField] private TMP_InputField scoreIf;
    [SerializeField] private Button scoreSaveButton;

    private async void Awake()
    {
        // 유니티 서비스 초기화
        await UnityServices.InitializeAsync();

        // 익명로그인 처리
        await SignIn();
    }

    private async Task SignIn()
    {
        // 로그인 성공시에 호출할 이벤트를 연결
        AuthenticationService.Instance.SignedIn += () =>
        {
            playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log($"로그인 성공 \nPlayer Id: {playerId}");
        };

        // 로그인
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
}
