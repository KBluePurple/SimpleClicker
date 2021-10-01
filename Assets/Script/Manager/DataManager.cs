using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    public Player Player;

    string path = "";
    string settingPath = "";

    private void Awake() {
        path = $"{Application.persistentDataPath}/save/data.json";
        settingPath = $"{Application.persistentDataPath}/save/settings.json";
        Debug.Log(path);
    }

    private void Start() {
        InvokeRepeating("SaveData", 60f, 60f);
    }

    public void SaveData()
    {
        // TODO : 저장 구현
        string data = JsonUtility.ToJson(this.Player);
        File.WriteAllText(path, data, Encoding.UTF8);
    }

    public void LoadData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Player = JsonUtility.FromJson<Player>(json);
        }
        else
            SaveData();
        
        for (int i = 0; i < Player.Star.Orbits.Count; i++)
        {
            if (Player.Star.Orbits[i].IsHave)
            {
                GameManager.Instance.Star.AddOrbit(true);
                GameManager.Instance.Star.Orbits[i].gameObject.SetActive(true);
            }
            GameManager.Instance.Star.Orbits[i].GetComponent<Orbit>().SetValue(Player.Star.Orbits[i]);
        }
        GameManager.Instance.UI.UpdateUI();
    }

    private void OnApplicationQuit() {
        SaveData();
    }
}
