using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.UI;

public class CloudSaveManager : MonoBehaviour
{
    [SerializeField] private Button saveButton;

    void Awake()
    {
        saveButton.onClick.AddListener(async () => await SaveSingleData());
    }

    private async Task SaveSingleData()
    {
        // 저장할 데이터
        var data = new Dictionary<string, object>
        {
            {"player_name", "Zack"},
            {"level", 30},
            {"xp", 20000},
            {"gold", 200}
        };

        try
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("데이터 저장 완료");
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e.Message);
        }
    }
}
