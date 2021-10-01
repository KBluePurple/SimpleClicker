using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StarManager : MonoBehaviour
{
    [SerializeField]
    public Orbit[] Orbits = null;

    List<Vector3> starScales = new List<Vector3>();

    private int Count = 0;

    private void Awake() {
        foreach (var orbit in Orbits)
        {
            orbit.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
        }

        starScales.Add(new Vector3(1.1f, 1.1f, 1.1f));
        starScales.Add(new Vector3(1.0f, 1.0f, 1.0f));
        starScales.Add(new Vector3(0.9f, 0.9f, 0.9f));
        starScales.Add(new Vector3(0.7f, 0.7f, 0.7f));
    }

    public void AddOrbit(bool isNoAnimation = false)
    {
        var Star = GameManager.Instance.UI.star;
        if (isNoAnimation)
        {
            Star.localScale = starScales[Count];
            Count++;
        }
        else
        {
            Sequence sequence = DOTween.Sequence();
            if (GameManager.Instance.UI.isShopOpened)
            {
                sequence.AppendInterval(.5f);
                GameManager.Instance.UI.CloseShopButton();
            }
            Orbits[Count].gameObject.SetActive(true);
            Orbits[Count].Enabled = true;
            sequence.Append(Orbits[Count].GetComponent<Image>().DOFade(1, 1).From(0));
            sequence.Join(Star.DOScale(starScales[Count], 1f));
            sequence.Join(Orbits[Count].SubStars[0].DOAnchorPosX(Orbits[Count].SubStars[0].anchoredPosition.x, 3f).From(new Vector2(1000, 0)));
            sequence.Play();
            Count++;
        }
    }
}
