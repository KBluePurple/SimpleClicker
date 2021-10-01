using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    [SerializeField]
    RectTransform SettingUI = null;

    private void Start()
    {
        SettingUI.localPosition = new Vector2(1440, 0);
    }

    public void OpenSetting()
    {
        SettingUI.DOLocalMoveX(0, .5f);
        GameManager.Instance.Tasks.Quit.AddTask(() => CloseSetting());
    }

    public void CloseSetting()
    {
        SettingUI.DOLocalMoveX(1440, .5f);
    }
}
