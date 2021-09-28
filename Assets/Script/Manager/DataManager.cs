using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField]
    public Player Player;

    public void SaveData()
    {

    }

    public void LoadData()
    {
        for (int i = 0; i < Player.Star.Orbits.Count; i++)
        {
            if (Player.Star.Orbits[i].IsHave)
            {
                GameManager.Instance.Star.AddOrbit();
            }
            
            GameManager.Instance.Star.Orbits[0].GetComponent<Orbit>().SetValue(Player.Star.Orbits[0]);
        }
        GameManager.Instance.UI.UpdateUI();
    }
}
