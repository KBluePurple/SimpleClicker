using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    RectTransform shopUI = null;

    [SerializeField]
    Image shopButton = null;

    [SerializeField]
    Transform star = null;

    [SerializeField]
    RectTransform starImage = null;

    [SerializeField]
    CanvasGroup ArrowButtons = null;

    private Vector2 originalScale;
    private float shopOriginPos;
    public bool isShopOpened = false;

    public void OpenShop()
    {
        GameManager.Instance.Tasks.Quit.AddTask(() => CloseShop());
        originalScale = star.localScale;
        shopOriginPos = shopUI.anchoredPosition.y;
        shopUI.DOAnchorPosY(Screen.height * 1.35f, .5f);
        starImage.DOLocalMoveY(520, .5f);
        star.DOScale(new Vector2(1f, 1f), .5f);
        shopButton.DOFade(0, .25f);
        ArrowButtons.DOFade(0, .25f);
        isShopOpened = true;
    }

    public void CloseShop()
    {
        shopUI.DOAnchorPosY(shopOriginPos, .5f);
        starImage.DOLocalMoveY(0, .5f);
        star.DOScale(originalScale, .5f);
        shopButton.DOFade(1, .5f);
        ArrowButtons.DOFade(1, .5f);
    }

    public void CloseShopButton()
    {
        isShopOpened = false;
        GameManager.Instance.Tasks.Quit.BackButtonHandler();
    }
}
