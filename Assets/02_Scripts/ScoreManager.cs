using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private string playerId;
    private const string leaderboardId = "Ranking";

    [Header("UI Setting")]
    [SerializeField] private TMP_InputField scoreIf;
    [SerializeField] private Button scoreSaveButton;

    private async void Awake()
    {
        // 유니티 서비스 초기화
        await UnityServices.InitializeAsync();

        // 익명로그인 처리
        await SignIn();

        // 버튼 이벤트 연결
        scoreSaveButton.onClick.AddListener(() => AddScore(int.Parse(scoreIf.text)));
    }

    private async Task SignIn()
    {
        // 로그인 성공시에 호출할 이벤트를 연결
        AuthenticationService.Instance.SignedIn += async () =>
        {
            playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log($"로그인 성공 \nPlayer Id: {playerId}");

            // 기존 점수 조회
            await GetPlayerScore();
        };

        // 로그인
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // 플레이어 점수 조회
    private async Task GetPlayerScore()
    {
        try
        {
            var result = await LeaderboardsService.Instance.GetPlayerScoreAsync(leaderboardId);
            // 점수 표시

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    private async void AddScore(int score)
    {
        // 점수 기록
        var response = await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboardId, score);

        Debug.Log($"점수저장 완료 \n {JsonConvert.SerializeObject(response)}");
    }
}
