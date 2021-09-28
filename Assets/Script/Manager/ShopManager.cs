using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    RectTransform items = null;

    private void Start()
    {
        for (int i = 0; i < items.childCount; i++)
        {
            items.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
            {
                onClickItem(i);
            });
        }
    }

    public void onClickItem(int index)
    {
        switch (index)
        {
            case 0:

                break;
        }
    }
}
