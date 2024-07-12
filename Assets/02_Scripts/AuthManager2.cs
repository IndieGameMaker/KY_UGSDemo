using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.Services.Core;
using System.Threading.Tasks;
using Unity.Services.Authentication;

public class AuthManager2 : MonoBehaviour
{
    /*
        - 회원이름 : 대소문자 구별없음, 3자 ~ 20자 [- @ 사용가능]
        - 비밀번호 : 8자 ~ 30자, 대소문자 구별, 숫자 1, 대문자 1, 소문자 1, 특수문자 1 [! @ # _]
    
    */

    [SerializeField] private Button signUpButton, signInButton;
    [SerializeField] private TMP_InputField userNameText, passwordText;

    private async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("UGS 초기화 완료");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    void OnEnable()
    {
        signUpButton.onClick.AddListener(async () =>
        {
            await SignUpUsernamePassword(userNameText.text, passwordText.text);
        });
    }

    // 회원가입
    async Task SignUpUsernamePassword(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log($"회원가입 완료 : {AuthenticationService.Instance.PlayerId}");
            PlayerPrefs.SetString("USER_NAME", username);
        }
        catch (AuthenticationException ex)
        {
            Debug.LogError($"회원가입 실패 {ex.Message}");
        }
        catch (RequestFailedException ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}
