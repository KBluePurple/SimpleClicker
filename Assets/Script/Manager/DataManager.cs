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
        path = $"{Application.persistentDataPath}/data.json";
        settingPath = $"{Application.persistentDataPath}/settings.json";
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
        data = JsonUtility.ToJson(GameManager.Instance.Setting.Settings);
        File.WriteAllText(settingPath, data, Encoding.UTF8);
    }

    public void LoadData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Player = JsonUtility.FromJson<Player>(json);
            json = File.ReadAllText(settingPath);
            GameManager.Instance.Setting.Settings = JsonUtility.FromJson<Settings>(json);
        }
        else
        {
            SaveData();
        }
        
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

        GameManager.Instance.Setting.masterSlider.value = GameManager.Instance.Setting.Settings.MasterVol;
        GameManager.Instance.Setting.musicSlider.value = GameManager.Instance.Setting.Settings.MusicVol;
        GameManager.Instance.Setting.effectSlider.value = GameManager.Instance.Setting.Settings.EffectVol;

        GameManager.Instance.ClickEffect.VibrationButtonEffectText.text = GameManager.Instance.Setting.Settings.Vibration ? "진동 끄기" : "진동 켜기";
        GameManager.Instance.ClickEffect.FastButtonEffectText.text = GameManager.Instance.Setting.Settings.FastMode ? "최적화 모드 끄기" : "최적화 모드 켜기";

        GameManager.Instance.Setting.FastMode(GameManager.Instance.Setting.Settings.FastMode);
    }

    private void OnApplicationQuit() {
        SaveData();
    }
}
