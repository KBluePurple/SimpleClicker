using PopupType;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace PopupType
{
    public enum ButtonType
    {
        While,
        Black
    }

    public class PopupButton
    {
        public string Text;
        public Action OnPress;
        public ButtonType Type;

        public PopupButton(string text, Action onPress, ButtonType type = ButtonType.While)
        {
            Text = text;
            OnPress = onPress;
            Type = type;
        }
    }
}

public class PopupManager : MonoBehaviour
{
    [SerializeField]
    private Canvas popupCanvus = null;

    [SerializeField]
    Text titleText = null;

    [SerializeField]
    Text descriptionText = null;

    [SerializeField]
    Button button1 = null;

    [SerializeField]
    Text button1Text = null;

    [SerializeField]
    Button button2 = null;

    [SerializeField]
    Text button2Text = null;

    [SerializeField]
    CanvasGroup popupGroup = null;

    [SerializeField]
    Image bluePanel = null;

    private void Start()
    {
        button1.onClick.AddListener(() => Hide());
    }

    public void Show(string title, string description, PopupButton button1, PopupButton button2)
    {
        if (popupCanvus.gameObject.activeSelf) return;

        popupGroup.alpha = 0;
        popupCanvus.gameObject.SetActive(true);
        titleText.text = title;
        descriptionText.text = description;

        this.button1.onClick.RemoveAllListeners();
        this.button1.onClick.AddListener(button1.OnPress.Invoke);
        button1Text.text = button1.Text;
        this.button1.GetComponent<Image>().color = button1.Type switch
        {
            ButtonType.Black => new Color(200f / 255f, 200f / 255f, 200f / 255f),
            _ => new Color(255f / 255f, 255f / 255f, 255f / 255f)
        };
        button1Text.color = button1.Type switch
        {
            ButtonType.Black => new Color(255f / 255f, 255f / 255f, 255f / 255f),
            _ => new Color(160f / 255f, 160f / 255f, 160f / 255f)
        };

        this.button2.onClick.RemoveAllListeners();
        this.button2.onClick.AddListener(button2.OnPress.Invoke);
        button2Text.text = button2.Text;
        this.button2.GetComponent<Image>().color = button2.Type switch
        {
            ButtonType.Black => new Color(200f / 255f, 200f / 255f, 200f / 255f),
            _ => new Color(255f / 255f, 255f / 255f, 255f / 255f)
        };
        button2Text.color = button2.Type switch
        {
            ButtonType.Black => new Color(255f / 255f, 255f / 255f, 255f / 255f),
            _ => new Color(160f / 255f, 160f / 255f, 160f / 255f)
        };

        GameManager.Instance.Tasks.Quit.AddTask(() =>
        {
            Sequence fadeoutSequence = DOTween.Sequence();
            fadeoutSequence.Append(popupGroup.DOFade(0, .5f));
            fadeoutSequence.AppendCallback(() => popupCanvus.gameObject.SetActive(false));
        });
        popupGroup.DOFade(1, .5f);

    }

    public void Hide()
    {
        GameManager.Instance.Tasks.Quit.BackButtonHandler();
    }
}
