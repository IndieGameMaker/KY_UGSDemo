using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        await
    }
}
