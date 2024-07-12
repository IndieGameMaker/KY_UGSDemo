using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct PlayerData
{
    public string name;
    public int level;
    public int xp;

    public List<ItemData> items;
}

[Serializable]
public struct ItemData
{
    public string name;
    public int value;
    public string icon;
}

public class CloudSaveManager : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private PlayerData playerData;

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
