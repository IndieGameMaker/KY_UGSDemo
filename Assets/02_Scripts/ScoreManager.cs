using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private string playerId;
    private const string leaderboardId = "Ranking";

    [Header("UI Setting")]
    [SerializeField] private TMP_InputField scoreIf;
    [SerializeField] private Button scoreSaveButton;
    [SerializeField] private Button scoreViewButton;

    private async void Awake()
    {
        // 유니티 서비스 초기화
        await UnityServices.InitializeAsync();

        // 익명로그인 처리
        await SignIn();

        // 버튼 이벤트 연결
        scoreSaveButton.onClick.AddListener(() =>
        {
            AddScore(int.Parse(scoreIf.text));
        });

        // 점수 조회 버튼 이벤트 연결
        scoreViewButton.onClick.AddListener(async () => await GetScores());
    }

    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

    private async Task GetScores()
    {
        var option = new GetScoresOptions { Offset = 25, Limit = 50 };
        var result = await LeaderboardsService.Instance.GetScoresAsync(leaderboardId, option);

        Debug.Log($"Json : {JsonConvert.SerializeObject(result)}");

        entries = result.Results;

        string rank = "";

        // 모든 점수를 표시
        foreach (var entry in entries)
        {
            rank += $"<color=#00ff00>[{entry.Rank}]</color> {entry.PlayerId} : {entry.Score}\n";
        }
        Debug.Log(rank);
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
            scoreIf.text = result.Score.ToString();
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
