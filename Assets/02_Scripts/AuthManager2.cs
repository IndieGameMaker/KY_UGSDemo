using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.Services.Core;

public class AuthManager2 : MonoBehaviour
{
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
}
