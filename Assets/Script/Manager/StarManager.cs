using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StarManager : MonoBehaviour
{
    [SerializeField]
    Orbit[] orbits = null;

    List<Vector3> starScales = new List<Vector3>();

    private int Count = 0;

    private void Start() {
        foreach (var orbit in orbits)
        {
            orbit.transform.Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
        }

        starScales.Add(new Vector3(1.1f, 1.1f, 1.1f));
        starScales.Add(new Vector3(1.0f, 1.0f, 1.0f));
        starScales.Add(new Vector3(0.9f, 0.9f, 0.9f));
        starScales.Add(new Vector3(0.7f, 0.7f, 0.7f));
    }

    private void FixedUpdate() {
        if (orbits[0].Enabled)
            orbits[0].transform.Rotate(new Vector3(0, 0, 1.4f));
        if (orbits[1].Enabled)
            orbits[1].transform.Rotate(new Vector3(0, 0, 1.3f));
        if (orbits[2].Enabled)
            orbits[2].transform.Rotate(new Vector3(0, 0, 1.2f));
        if (orbits[3].Enabled)
            orbits[3].transform.Rotate(new Vector3(0, 0, 1.1f));
    }

    public void AddOrbit()
    {
        Sequence sequence = DOTween.Sequence();
        if (GameManager.Instance.UI.isShopOpened)
        {
            sequence.AppendInterval(.5f);
            GameManager.Instance.UI.CloseShopButton();
        }
        var Star = GameManager.Instance.UI.star;
        orbits[Count].gameObject.SetActive(true);
        orbits[Count].Enabled = true;
        sequence.Append(orbits[Count].GetComponent<Image>().DOFade(1, 1).From(0));
        sequence.Join(Star.DOScale(starScales[Count], 1f));
        sequence.Join(orbits[Count].SubStar[0].DOAnchorPosX(-6f, 3f).From(new Vector2(1000, 0)));
        sequence.Play();
        Count++;
    }
}
