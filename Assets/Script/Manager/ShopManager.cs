using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ShopManager : MonoBehaviour
{
    [SerializeField]
    RectTransform items = null;

    [SerializeField]
    RectTransform[] orbitTitles = null;

    [SerializeField]
    Button[] arrows = null;

    [SerializeField]
    Text descriptionText = null;

    [SerializeField]
    NumberCounter valueText = null;

    [SerializeField]
    RectTransform upgradeButton = null;

    int curOrbitIndex = 0;
    int curItemIndex = 1;
    bool isPlayingAnimation = false;

    CanvasGroup[] itemsCanvasGroups = new CanvasGroup[3];
    Button[] itemsButtons = new Button[3];
    private void Start()
    {
        for(int i = 0; i < 3; i++)
            itemsCanvasGroups[i] = items.GetChild(i).GetComponent<CanvasGroup>();
        for(int i = 0; i < 3; i++)
            itemsButtons[i] = items.GetChild(i).GetComponent<Button>();

        for (int i = 0; i < items.childCount; i++)
        {
            int temp = i;
            items.GetChild(temp).GetComponent<Button>().onClick.AddListener(() =>
            {
                GameManager.Instance.ClickEffect
                .ClickEffectObject(items.GetChild(temp).GetChild(0).GetChild(0), 3f);

                onClickItem(temp);
            });
        }

        orbitTitles[0].GetComponent<Text>().text = "Core\nStar";
        orbitTitles[0].DOLocalMoveX(0, .5f);

        arrows[0].interactable = false;
        arrows[0].GetComponent<CanvasGroup>().alpha = 0;
        items.GetComponent<CanvasGroup>().alpha = 0;
        items.anchoredPosition = new Vector2(-450, items.anchoredPosition.y);
        itemsCanvasGroups[0].alpha = 0f;
        itemsButtons[0].interactable = false;
        itemsCanvasGroups[1].alpha = 0f;
        itemsButtons[1].interactable = false;
        itemsCanvasGroups[2].alpha = 1f;
        itemsButtons[2].interactable = true;
        items.GetChild(2).GetChild(0)
        .localScale = Vector2.one;
        curItemIndex = 2;
        items.GetComponent<CanvasGroup>().alpha = 1;

        arrows[0].onClick.AddListener(() => changeOrbitSelect(true));
        arrows[1].onClick.AddListener(() => changeOrbitSelect(false));

        UpdateShopUI();
    }

    public void UpdateOrbitSelect()
    {
        int orbitCount = 0;
        foreach (var orbit in GameManager.Instance.Star.Orbits)
        {
            if (orbit.Enabled)
                orbitCount++;
        }

        if (curOrbitIndex == 0)
        {
            arrows[0].interactable = false;
            arrows[1].interactable = true;
            Sequence sequence1 = DOTween.Sequence();
            sequence1.Join(arrows[0].GetComponent<CanvasGroup>().DOFade(0, 0));
            sequence1.Join(arrows[1].GetComponent<CanvasGroup>().DOFade(1, 0));
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(0, 0));
            sequence1.AppendCallback(() =>
            {
                items.anchoredPosition = new Vector2(-450, items.anchoredPosition.y);
                itemsCanvasGroups[0].alpha = 0f;
                itemsButtons[0].interactable = false;
                itemsCanvasGroups[1].alpha = 0f;
                itemsButtons[1].interactable = false;
                itemsCanvasGroups[2].alpha = 1f;
                itemsButtons[2].interactable = true;
                items.GetChild(2).GetChild(0)
                .localScale = Vector2.one;
            });
            curItemIndex = 2;
            sequence1.AppendCallback(() => upgradeButton.GetChild(1).GetComponent<Text>().text = "UPGRADE");
            sequence1.AppendCallback(() => descriptionText.text = "업그레이드 비용");
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(1, 0));
        }
        else if (curOrbitIndex <= orbitCount)
        {
            arrows[0].interactable = true;
            Sequence sequence1 = DOTween.Sequence();
            sequence1.Join(arrows[0].GetComponent<CanvasGroup>().DOFade(1, 0));
            if (curOrbitIndex < 4)
            {
                arrows[1].interactable = true;
                sequence1.Join(arrows[1].GetComponent<CanvasGroup>().DOFade(1, 0));
            }
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(0, 0));
            sequence1.AppendCallback(() =>
            {
                items.anchoredPosition = new Vector2(450, items.anchoredPosition.y);
                itemsCanvasGroups[0].alpha = 1f;
                itemsButtons[0].interactable = true;
                items.GetChild(0).GetChild(0)
                .localScale = Vector2.one;
                itemsCanvasGroups[1].alpha = .5f;
                itemsButtons[1].interactable = true;
                items.GetChild(1).GetChild(0)
                .localScale = new Vector2(.8f, .8f);
                itemsCanvasGroups[2].alpha = 0f;
                itemsButtons[2].interactable = true;
                items.GetChild(2).GetChild(0)
                .localScale = new Vector2(.8f, .8f);
            });
            curItemIndex = 0;
            sequence1.AppendCallback(() => upgradeButton.GetChild(1).GetComponent<Text>().text = "UPGRADE");
            sequence1.AppendCallback(() => descriptionText.text = "업그레이드 비용");
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(1, 0));
        }
        else
        {
            arrows[0].interactable = true;
            arrows[1].interactable = false;
            Sequence sequence1 = DOTween.Sequence();
            sequence1.Join(arrows[0].GetComponent<CanvasGroup>().DOFade(1, .25f));
            sequence1.Join(arrows[1].GetComponent<CanvasGroup>().DOFade(0, .25f));
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(0, .25f));

            // TODO: 자물쇠 아이콘 표시

            sequence1.AppendCallback(() => upgradeButton.GetChild(1).GetComponent<Text>().text = "LOCKED");
            sequence1.AppendCallback(() => descriptionText.text = "잠금 해제 비용");

            curItemIndex = 0;
        }

        if (curOrbitIndex == 4)
        {
            arrows[1].interactable = false;
        }
        UpdateShopUI();
    }

    private void changeOrbitSelect(bool isLeft)
    {
        int orbitCount = 0;
        foreach (var orbit in GameManager.Instance.Star.Orbits)
        {
            if (orbit.Enabled)
                orbitCount++;
        }

        if (isPlayingAnimation) return;
        isPlayingAnimation = true;
        Sequence sequence = DOTween.Sequence();
        if (isLeft && curOrbitIndex > 0)
        {
            curOrbitIndex--;
            if (curOrbitIndex == 0)
                orbitTitles[1].GetComponent<Text>().text = "Core\nStar";
            else
                orbitTitles[1].GetComponent<Text>().text = $"{curOrbitIndex}st\nOrbit";

            orbitTitles[1].localPosition = new Vector2(-500, 0);
            sequence.Join(orbitTitles[0].DOLocalMoveX(500, .5f));
            sequence.Join(orbitTitles[1].DOLocalMoveX(0, .5f));
            sequence.AppendCallback(() =>
            {
                orbitTitles[0].GetComponent<Text>().text = orbitTitles[1].GetComponent<Text>().text;
                orbitTitles[0].localPosition = orbitTitles[1].localPosition;
                orbitTitles[1].localPosition = new Vector2(-500, 0);
            });
        }
        else if (!isLeft && curItemIndex < 4)
        {
            curOrbitIndex++;
            orbitTitles[1].GetComponent<Text>().text = $"{curOrbitIndex}st\nOrbit";

            orbitTitles[1].localPosition = new Vector2(500, 0);
            sequence.Join(orbitTitles[0].DOLocalMoveX(-500, .5f));
            sequence.Join(orbitTitles[1].DOLocalMoveX(0, .5f));
            sequence.AppendCallback(() =>
            {
                orbitTitles[0].GetComponent<Text>().text = orbitTitles[1].GetComponent<Text>().text;
                orbitTitles[0].localPosition = orbitTitles[1].localPosition;
                orbitTitles[1].localPosition = new Vector2(-500, 0);
            });
        }

        if (curOrbitIndex == 0)
        {
            arrows[0].interactable = false;
            arrows[1].interactable = true;
            Sequence sequence1 = DOTween.Sequence();
            sequence1.Join(arrows[0].GetComponent<CanvasGroup>().DOFade(0, .25f));
            sequence1.Join(arrows[1].GetComponent<CanvasGroup>().DOFade(1, .25f));
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(0, .25f));
            sequence1.AppendCallback(() =>
            {
                items.anchoredPosition = new Vector2(-450, items.anchoredPosition.y);
                itemsCanvasGroups[0].alpha = 0f;
                itemsButtons[0].interactable = false;
                itemsCanvasGroups[1].alpha = 0f;
                itemsButtons[1].interactable = false;
                itemsCanvasGroups[2].alpha = 1f;
                itemsButtons[2].interactable = true;
                items.GetChild(2).GetChild(0)
                .localScale = Vector2.one;
            });
            curItemIndex = 2;
            sequence1.AppendCallback(() => upgradeButton.GetChild(1).GetComponent<Text>().text = "UPGRADE");
            sequence1.AppendCallback(() => descriptionText.text = "업그레이드 비용");
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(1, .25f));
        }
        else if (curOrbitIndex <= orbitCount)
        {
            arrows[0].interactable = true;
            Sequence sequence1 = DOTween.Sequence();
            sequence1.Join(arrows[0].GetComponent<CanvasGroup>().DOFade(1, .25f));
            if (curOrbitIndex < 4)
            {
                arrows[1].interactable = true;
                sequence1.Join(arrows[1].GetComponent<CanvasGroup>().DOFade(1, .25f));
            }
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(0, .25f));
            sequence1.AppendCallback(() =>
            {
                items.anchoredPosition = new Vector2(450, items.anchoredPosition.y);
                itemsCanvasGroups[0].alpha = 1f;
                itemsButtons[0].interactable = true;
                items.GetChild(0).GetChild(0)
                .localScale = Vector2.one;
                itemsCanvasGroups[1].alpha = .5f;
                itemsButtons[1].interactable = true;
                items.GetChild(1).GetChild(0)
                .localScale = new Vector2(.8f, .8f);
                itemsCanvasGroups[2].alpha = 0f;
                itemsButtons[2].interactable = true;
                items.GetChild(2).GetChild(0)
                .localScale = new Vector2(.8f, .8f);
            });
            curItemIndex = 0;
            sequence1.AppendCallback(() => upgradeButton.GetChild(1).GetComponent<Text>().text = "UPGRADE");
            sequence1.AppendCallback(() => descriptionText.text = "업그레이드 비용");
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(1, .25f));
        }
        else
        {
            arrows[0].interactable = true;
            arrows[1].interactable = false;
            Sequence sequence1 = DOTween.Sequence();
            sequence1.Join(arrows[0].GetComponent<CanvasGroup>().DOFade(1, .25f));
            sequence1.Join(arrows[1].GetComponent<CanvasGroup>().DOFade(0, .25f));
            sequence1.Join(items.GetComponent<CanvasGroup>().DOFade(0, .25f));

            // TODO: 자물쇠 아이콘 표시

            sequence1.AppendCallback(() => upgradeButton.GetChild(1).GetComponent<Text>().text = "LOCKED");
            sequence1.AppendCallback(() => descriptionText.text = "잠금 해제 비용");

            curItemIndex = 0;
        }

        if (curOrbitIndex == 4)
        {
            arrows[1].interactable = false;
        }

        sequence.AppendInterval(.25f);
        sequence.AppendCallback(() =>
        {
            isPlayingAnimation = false;
        });
        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        switch (curOrbitIndex)
        {
            case 0:
                valueText.Value = GameManager.Instance.Data.Player.Star.UpgradePrice;
                break;
            default:
                switch (curItemIndex)
                {
                    case 0:
                        valueText.Value = GameManager.Instance.Data.Player.Star.Orbits[curOrbitIndex - 1].SpeedPrice;
                        break;
                    case 1:
                        valueText.Value = GameManager.Instance.Data.Player.Star.Orbits[curOrbitIndex - 1].CountPrice;
                        break;
                    case 2:
                        valueText.Value = GameManager.Instance.Data.Player.Star.Orbits[curOrbitIndex - 1].ValuePrice;
                        break;
                }
                break;
        }
    }

    public void onClickItem(int index)
    {
        float animationTime = 0.5f;
        switch (index)
        {
            case 0:
                if (curItemIndex == 1)
                {
                    items.DOAnchorPosX(450, .5f);
                    itemsCanvasGroups[0].DOFade(1f, animationTime);
                    items.GetChild(0).GetChild(0)
                    .DOScale(Vector2.one, .5f);
                    itemsCanvasGroups[1].DOFade(.5f, animationTime);
                    items.GetChild(1).GetChild(0)
                    .DOScale(new Vector2(.8f, .8f), .5f);
                    itemsCanvasGroups[2].DOFade(0, animationTime);
                    curItemIndex = 0;
                }
                break;
            case 1:
                if (curItemIndex != 1)
                {
                    items.DOAnchorPosX(0, .5f);
                    itemsCanvasGroups[0].DOFade(.5f, animationTime);
                    items.GetChild(0).GetChild(0)
                    .DOScale(new Vector2(.8f, .8f), .5f);
                    itemsCanvasGroups[1].DOFade(1f, animationTime);
                    items.GetChild(1).GetChild(0)
                    .DOScale(Vector2.one, .5f);
                    itemsCanvasGroups[2].DOFade(.5f, animationTime);
                    items.GetChild(2).GetChild(0)
                    .DOScale(new Vector2(.8f, .8f), .5f);
                    curItemIndex = 1;
                }
                break;
            case 2:
                if (curItemIndex == 1)
                {
                    items.DOAnchorPosX(-450, .5f);
                    itemsCanvasGroups[0].DOFade(0, animationTime);
                    itemsCanvasGroups[1].DOFade(.5f, animationTime);
                    items.GetChild(1).GetChild(0)
                    .DOScale(new Vector2(.8f, .8f), .5f);
                    itemsCanvasGroups[2].DOFade(1f, animationTime);
                    items.GetChild(2).GetChild(0)
                    .DOScale(Vector2.one, .5f);
                    curItemIndex = 2;
                }
                break;
        }
        UpdateShopUI();
    }

    public void onClickUpgrade()
    {
        var playerDate = GameManager.Instance.Data.Player;
        int upgradePrice;
        Action tempFunction = () => {};
        switch (curOrbitIndex)
        {
            case 0:
                upgradePrice = playerDate.Star.UpgradePrice;
                tempFunction = () => playerDate.Star.UpgradeCount++;
                break;
            default:
                switch (curItemIndex)
                {
                    case 0:
                        if (playerDate.Star.Orbits[curOrbitIndex - 1].IsHave)
                        {
                            upgradePrice = playerDate.Star.Orbits[curOrbitIndex - 1].SpeedPrice;
	                        tempFunction = () => {
                                playerDate.Star.Orbits[curOrbitIndex - 1].SpeedUpgrade++;
                                GameManager.Instance.Star.Orbits[curOrbitIndex - 1].SetValue(playerDate.Star.Orbits[curOrbitIndex - 1], false);
                            };
                        }
                        else
                        {
                            upgradePrice = playerDate.Star.Orbits[curOrbitIndex - 1].BaseValuePrice;
                            tempFunction = () => {
                                playerDate.Star.Orbits[curOrbitIndex - 1].IsHave = true;
                                GameManager.Instance.Star.AddOrbit();
                            };
                        }
                        break;
                    case 1:
                        upgradePrice = playerDate.Star.Orbits[curOrbitIndex - 1].CountPrice;
                        tempFunction = () => {
                            playerDate.Star.Orbits[curOrbitIndex - 1].Count++;
                            GameManager.Instance.Star.Orbits[curOrbitIndex - 1].AddSubStar(false);
                        };
                        break;
                    case 2:
                        upgradePrice = playerDate.Star.Orbits[curOrbitIndex - 1].ValuePrice;
                        tempFunction = () => {
                            playerDate.Star.Orbits[curOrbitIndex - 1].ValueUpgrade++;
                            GameManager.Instance.Star.Orbits[curOrbitIndex - 1].SetValue(playerDate.Star.Orbits[curOrbitIndex - 1], false);
                        };;
                        break;
                    default:
                        upgradePrice = 0;
                        tempFunction = () => {};
                        break;
                }
                break;
        }

        if (playerDate.StarEnergy >= upgradePrice)
        {
            playerDate.StarEnergy -= upgradePrice;
            tempFunction.Invoke();
        }
        else
        {
            StartCoroutine(cantUpgradeAnimation());
        }

        UpdateShopUI();
        GameManager.Instance.UI.UpdateUI();
    }

    public IEnumerator cantUpgradeAnimation()
    {
        for(int i = 0; i < 6; i++)
        {
            if (i % 2 == 0)
                upgradeButton.localPosition = new Vector2(-20 - i, upgradeButton.localPosition.y);
            else
                upgradeButton.localPosition = new Vector2(20 - i, upgradeButton.localPosition.y);

            yield return new WaitForSeconds(0.04f);
        }

        upgradeButton.localPosition = new Vector2(0, upgradeButton.localPosition.y);
    }
}
