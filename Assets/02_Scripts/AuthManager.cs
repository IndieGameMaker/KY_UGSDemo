using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Authentication;

public class AuthManager : MonoBehaviour
{
    [SerializeField] private Button signInButton;
    [SerializeField] private Button signOutButton;
    [SerializeField] private Button playerNameSaveButton;

    [SerializeField] private TMP_Text messageText;
    [SerializeField] private TMP_InputField playerNameIf;

    private async void Awake()
    {
        // UGS 초기화
        await UnityServices.InitializeAsync();

        // 이벤트 연결
        EventConfig();

        // 버튼 이벤트 연결
        // 로그인
        signInButton.onClick.AddListener(async () =>
        {
            // 익명 로그인 메소드 호출
            await SignInAsync();
        });

        // 로그 아웃
        signOutButton.onClick.AddListener(() =>
        {
            AuthenticationService.Instance.SignOut();
        });

        // 플레이어 이름 변경
        playerNameSaveButton.onClick.AddListener(async () =>
        {
            await SetPlayerName(playerNameIf.text);
        });
    }

    private async Task SetPlayerName(string playerName)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
            Debug.Log($"{playerName} updated!");

            messageText.text = $"\nPlayerName : {AuthenticationService.Instance.PlayerName}";
        }
        catch (AuthenticationException e)
        {
            Debug.LogError(e.Message);
        }
    }

    private void EventConfig()
    {
        // 로그인
        AuthenticationService.Instance.SignedIn += () =>
        {
            messageText.text = $"\nPlayer ID:{AuthenticationService.Instance.PlayerId}";
        };

        // 로그 아웃
        AuthenticationService.Instance.SignedOut += () =>
        {
            messageText.text = "\nLogout !!!";
        };

        // 로그인 실패
        AuthenticationService.Instance.SignInFailed += (ex) =>
        {
            messageText.text = $"\nLogin Failed : {ex.Message}";
        };

        // 세션 종료
        AuthenticationService.Instance.Expired += () =>
        {
            messageText.text = "\nPlayer Session Expired !!!";
        };

    }

    // 익명 로그인
    private async Task SignInAsync()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("익명 사용자 로그인 완료");
        }
        catch (AuthenticationException e)
        {
            Debug.Log(e.Message);
        }
    }

}
