using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickEffectManager : MonoBehaviour
{
    [SerializeField]
    GameObject EffectPrefab = null;

    [SerializeField]
    Transform StarButtonEffect = null;

    PoolManager clickEffectPool = null;

    private void Start()
    {
        clickEffectPool = new PoolManager(EffectPrefab);
    }

    private GameObject ClickEffectObject(Transform parant, float size)
    {
        var EffectObject = clickEffectPool.GetObject();
        EffectObject.transform.SetParent(parant);
        var EffectPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        EffectPos.z = 100;
        EffectObject.transform.position = EffectPos;
        EffectObject.transform.localScale = new Vector2(0f, 0f);
        EffectObject.GetComponent<Image>().color = new Color(1, 1, 1, .3f);
        EffectObject.SetActive(true);
        Sequence effectSequence = DOTween.Sequence();
        effectSequence.Append(EffectObject.transform.DOScale(new Vector2(size, size), 1f));
        effectSequence.Join(EffectObject.GetComponent<Image>().DOFade(0f, 1f));
        effectSequence.AppendCallback(() =>
        {
            clickEffectPool.PutObject(EffectObject);
        });
        effectSequence.Play();
        return EffectObject;
    }

    public void OnClickStar()
    {
        ClickEffectObject(StarButtonEffect, 3);
        if (!GameManager.Instance.UI.isShopOpened)
        {
            GameManager.Instance.UI.GetMoneyEffect(100, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else
        {
            GameManager.Instance.UI.CloseShopButton();
        }
    }

    public void OnClickShop()
    {
        GameManager.Instance.UI.OpenShop();
    }
}
