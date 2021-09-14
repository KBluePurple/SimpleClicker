using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    GameObject Prefab = null;
    GameObject Parant = null;
    public PoolManager(GameObject prefab)
    {
        Parant = new GameObject(prefab.name + "Pool");
        Prefab = prefab;
    }

    public GameObject GetObject()
    {
        if (Parant.transform.childCount <= 0)
        {
            return GameManager.Instance.InstantiateObj(Prefab);
        }
        else
        {
            Parant.transform.GetChild(0).gameObject.SetActive(true);
            return Parant.transform.GetChild(0).gameObject;
        }
    }

    public void PutObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(Parant.transform);
    }
}
