using System;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;

public class Orbit : MonoBehaviour 
{
    [SerializeField]
    public List<RectTransform> SubStars;

    [SerializeField]
    float r;

    public float Speed = 1;
    
    public bool Enabled = false;

    public int SE = 100;

    public void AddSubStar()
    {
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

    int lastSubStarIndex = 0;
    bool rotated = false;
    private void FixedUpdate() 
    {
        if (Enabled && !GameManager.Instance.UI.isShopOpened)
        {
            if (Speed > 0)
            {
                float zAngle = transform.rotation.eulerAngles.z - 90;
                zAngle = zAngle < 0 ? zAngle + 360 : zAngle;

                float slicedAngle = 360f/SubStars.Count;
                slicedAngle = slicedAngle >= 360f ? 0 : slicedAngle;

                float endAngle = (slicedAngle * lastSubStarIndex);


                if (zAngle > endAngle)
                {
                    if (!(lastSubStarIndex == 0 && !rotated))
                    {
                        if (lastSubStarIndex == 0)
                            rotated = false;    

                        RectTransform SubStar = SubStars[0];
                        foreach (var subStar in SubStars)
                        {
                            if (subStar.transform.position.y > SubStar.transform.position.y)
                            {
                                SubStar = subStar;
                            }
                        }

                        GameManager.Instance.UI.GetMoneyEffect(SE, SubStar.transform.position, true);
                        lastSubStarIndex++;
                        if (lastSubStarIndex >= SubStars.Count)
                            lastSubStarIndex = 0;
                    }
                }

                if (zAngle + Speed > 360)
                    rotated = true;

                transform.Rotate(new Vector3(0, 0, Speed));
            }
        }
    }
}