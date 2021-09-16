using System;
using UnityEngine;
using DG.Tweening;

public class Orbit : MonoBehaviour {
    [SerializeField]
    public RectTransform[] SubStar;
    
    public bool Enabled = false;

    public void AddSubStar()
    {
        // TODO 그 위성 추가하는거
    }

    private void FixedUpdate() {
        // TODO 막 막 어 그 뭐냐 닿으면 돈 뿌려
        // TODO 세이브에서 값 읽어서 회전시키기
    }
}