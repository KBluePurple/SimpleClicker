using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;

public class Orbit : MonoBehaviour {
    [SerializeField]
    public List<RectTransform> SubStars;

    [SerializeField]
    float r;

    public float Speed = 1;
    
    public bool Enabled = false;

    public void AddSubStar()
    {
        // TODO 그 위성 추가하는거
        SubStars.Add(Instantiate(SubStars[0], transform));
        for (int i = 0; i < SubStars.Count; i++)
        {
            float degree = (360f/SubStars.Count) * (float)i;
            var x1 = (Mathf.Cos(degree * Mathf.Deg2Rad) * r);
            var y1 = (Mathf.Sin(degree * Mathf.Deg2Rad) * r);

            if (i == SubStars.Count - 1)
            {
                SubStars[i].DOAnchorPos(new Vector2(x1, y1), 3f).From(Vector2.zero);
                var SubStarsLight = SubStars[i].GetComponent<Light2D>();
                DOTween.To(() => SubStarsLight.intensity, x => SubStarsLight.intensity = x, .6f, 3).From(0f);
            }
            else
            {
                SubStars[i].DOAnchorPos(new Vector2(x1, y1), 3f);
            }
        }
    }

    private void FixedUpdate() {
        // TODO 막 막 어 그 뭐냐 닿으면 돈 뿌려
        // TODO 세이브에서 값 읽어서 회전시키기
    }
}