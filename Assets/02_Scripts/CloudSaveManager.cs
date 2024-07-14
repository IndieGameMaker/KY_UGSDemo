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
    [SerializeField] private Button multiDataSaveButton;
    [SerializeField] private Button loadDataButton;


    [SerializeField] private PlayerData playerData;

    void Awake()
    {
        saveButton.onClick.AddListener(async () => await SaveSingleData());
        multiDataSaveButton.onClick.AddListener(async () =>
        {
            await SaveMultiData<PlayerData>("PlayerData", playerData);
        });
        loadDataButton.onClick.AddListener(async () =>
        {
            await LoadData<string>("PlayerData");
        });
    }

    async Task LoadData<T>(string key)
    {
        var loadData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });

        loadData.TryGetValue(key, out var data);

        // JSon 출력
        string jsonStr = JsonUtility.ToJson(data.Value.GetAs<PlayerData>());
        Debug.Log(jsonStr);
        playerData = JsonUtility.FromJson<PlayerData>(jsonStr);
    }


    private async Task SaveMultiData<T>(string key, T saveData)
    {
        // 딕셔너리 데이터를 저장
        var data = new Dictionary<string, object>
        {
            {key, saveData}
        };

        // 저장 메소드 호출
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);

        var gameData = new Dictionary<string, object>{
            {"ver", "0.1"},
            {"lastUpdate", "2024-01-25"}
        };

        Debug.Log("저장 완료");
        playerData = new PlayerData();
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
