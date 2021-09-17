using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Canvas UICanvas = null;

    [SerializeField]
    RectTransform shopUI = null;

    [SerializeField]
    Image shopButton = null;

    [SerializeField]
    public Transform star = null;

    [SerializeField]
    public RectTransform starImage = null;

    [SerializeField]
    Image shopPanel = null;

    [SerializeField]
    Transform getEffect = null;

    [SerializeField]
    GameObject getEffectPrefab = null;

    [SerializeField]
    Transform MoneyTextPos = null;
    
    [SerializeField]
    RectTransform SETextPos = null;

    [SerializeField]
    Text MoneyStackText = null;

    [SerializeField]
    RectTransform OrbitsRTF = null;
    
    private int MoneyStack = 0;
    private Vector2 originalScale;
    private float shopOriginPos;
    public bool isShopOpened = false;
    PoolManager getMoneyEffectPool = null;

    private void Start() {
        getMoneyEffectPool = new PoolManager(getEffectPrefab);
        RectTransform shopPanelRT = shopPanel.GetComponent<RectTransform>();
        float temp = UICanvas.GetComponent<RectTransform>().sizeDelta.y - shopPanelRT.sizeDelta.y;
        shopPanelRT.anchoredPosition -= new Vector2(0, temp / 2);
        shopOriginPos = shopUI.anchoredPosition.y;
        shopPanelRT.sizeDelta = new Vector2(shopPanelRT.sizeDelta.x, UICanvas.GetComponent<RectTransform>().sizeDelta.y);
    }
    public void OpenShop()
    {
        originalScale = star.localScale;
        isShopOpened = true;
        Sequence sequence = DOTween.Sequence();
        sequence.Join(starImage.DOLocalMoveY(-200, .5f));
        sequence.Join(OrbitsRTF.DOLocalMoveY(-200, .5f));
        sequence.AppendInterval(.05f);
        sequence.Append(starImage.DOLocalMoveY(520, .5f));
        sequence.Join(star.DOScale(new Vector2(1f, 1f), .5f));
        sequence.Join(shopUI.DOAnchorPosY(UICanvas.GetComponent<RectTransform>().sizeDelta.y * 1.4f, .5f));
        sequence.Join(shopButton.DOFade(0, .25f));
        sequence.Append(OrbitsRTF.DOLocalMove(Vector3.zero, .5f));
        GameManager.Instance.Tasks.Quit.AddTask(() => CloseShop());
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

    int timer = 0;
    bool isStackLooping = false;
    private void showStackText()
    {
        timer = 0;
        if (MoneyStack == 0)
        {
            SETextPos.DOAnchorPosY(30, .5f).From(Vector2.zero);
            MoneyStackText.GetComponent<RectTransform>().anchoredPosition = new Vector2(-492, -226);
            Sequence sequence = DOTween.Sequence();
            sequence.Append(MoneyStackText.DOFade(1f, .5f).From(0));
            sequence.AppendCallback(() => {
                if (!isStackLooping) StartCoroutine(StackTimerLoop());
            });
        }
    }

    private void hideStackText()
    {
        // TODO 나중에 시간 되면 펑 하면서 들어가는거 만들기
        //MoneyStackText.GetComponent<RectTransform>().DOAnchorPosY(-175, 0.5f);
        MoneyStackText.DOFade(0f, .5f).From(1);
        SETextPos.DOAnchorPosY(0, .5f);
        MoneyStack = 0;
    }

    private IEnumerator StackTimerLoop()
    {
        if (isStackLooping) yield break;
        while(timer <= 10)
        {
            timer++;
            yield return new WaitForSeconds(0.1f);
        }
        isStackLooping = false;
        hideStackText();
    }

    public void GetMoneyEffect(int value, Vector3 startPos)
    {
        var EffectObject = getMoneyEffectPool.GetObject();
        // EffectObject.GetComponent<Text>().text = $"+ {value} SE";
        EffectObject.transform.SetParent(getEffect);
        EffectObject.transform.localScale = Vector3.one;
        EffectObject.GetComponent<Image>().color += new Color(0, 0, 1);
        var EffectPos = startPos;
        EffectPos.z = 100;
        EffectObject.transform.position = EffectPos;
        Sequence sequence = DOTween.Sequence();
        var RandomPos = new Vector3(EffectPos.x + Random.Range(-.5f, .5f), EffectPos.y + Random.Range(-.5f, .5f), 100);
        sequence.Append(EffectObject.transform.DOMove(RandomPos, .5f));
        sequence.Append(EffectObject.transform.DOMove(MoneyTextPos.position, .5f));
        sequence.Join(EffectObject.GetComponent<Image>().DOFade(0, .5f));
        sequence.AppendCallback(() => {
            getMoneyEffectPool.PutObject(EffectObject);
            showStackText();
            MoneyStack += value;
            UpdateUI();
        });
    }

    public void UpdateUI()
    {
        MoneyStackText.text = $"+ {MoneyStack} SE";
    }
}
