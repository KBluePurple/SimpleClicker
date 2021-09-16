using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Canvas UICanvas = null;

    [SerializeField]
    RectTransform shopUI = null;

    [SerializeField]
    Image shopButton = null;

    [SerializeField]
    Transform star = null;

    [SerializeField]
    RectTransform starImage = null;

    [SerializeField]
    Image shopPanel = null;

    [SerializeField]
    Transform getEffect = null;

    [SerializeField]
    GameObject getEffectPrefab = null;

    [SerializeField]
    Transform MoneyTextPos = null;

    private Vector2 originalScale;
    private float shopOriginPos;
    public bool isShopOpened = false;
    PoolManager getMoneyEffectPool = null;

    private void Start() {
        getMoneyEffectPool = new PoolManager(getEffectPrefab);

        RectTransform shopPanelRT = shopPanel.GetComponent<RectTransform>();
        float temp = UICanvas.GetComponent<RectTransform>().sizeDelta.y - shopPanelRT.sizeDelta.y;
        shopPanelRT.anchoredPosition -= new Vector2(0, temp / 2);

        originalScale = star.localScale;
        shopOriginPos = shopUI.anchoredPosition.y;
        shopPanelRT.sizeDelta = new Vector2(shopPanelRT.sizeDelta.x, UICanvas.GetComponent<RectTransform>().sizeDelta.y);
    }
    public void OpenShop()
    {
        GameManager.Instance.Tasks.Quit.AddTask(() => CloseShop());
        shopUI.DOAnchorPosY(UICanvas.GetComponent<RectTransform>().sizeDelta.y * 1.4f, .5f);
        starImage.DOLocalMoveY(520, .5f);
        star.DOScale(new Vector2(1f, 1f), .5f);
        shopButton.DOFade(0, .25f);
        isShopOpened = true;
    }

    public void CloseShop()
    {
        shopUI.DOAnchorPosY(shopOriginPos, .5f);
        starImage.DOLocalMoveY(0, .5f);
        star.DOScale(originalScale, .5f);
        shopButton.DOFade(1, .5f);
        isShopOpened = false;
    }

    public void CloseShopButton()
    {
        isShopOpened = false;
        GameManager.Instance.Tasks.Quit.BackButtonHandler();
    }

    public void GetMoneyEffect(int value, Vector3 startPos)
    {
        var EffectObject = getMoneyEffectPool.GetObject();
        EffectObject.transform.SetParent(getEffect);
        EffectObject.transform.localScale = Vector3.one;
        EffectObject.GetComponent<Text>().color += new Color(0, 0, 1);
        var EffectPos = startPos;
        EffectPos.z = 100;
        EffectObject.transform.position = EffectPos;
        Sequence sequence = DOTween.Sequence();
        var RandomPos = new Vector3(EffectPos.x + Random.Range(-1f, 1f), EffectPos.y + Random.Range(-1f, 1f), 100);
        sequence.Append(EffectObject.transform.DOMove(RandomPos, .5f));
        sequence.Append(EffectObject.transform.DOMove(MoneyTextPos.position, .5f));
        sequence.Join(EffectObject.GetComponent<Text>().DOFade(0, .5f));
        sequence.AppendCallback(() => {
            getMoneyEffectPool.PutObject(EffectObject);
        });
    }

    public void UpdateUI()
    {

    }
}
