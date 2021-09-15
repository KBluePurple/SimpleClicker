using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    [SerializeField]
    RectTransform Orbit1 = null;
    [SerializeField]
    RectTransform Orbit2 = null;
    [SerializeField]
    RectTransform Orbit3 = null;
    [SerializeField]
    RectTransform Orbit4 = null;

    private void Start() {
        Orbit1.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        Orbit2.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        Orbit3.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        Orbit4.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }

    private void Update() {
        Orbit1.Rotate(new Vector3(0, 0, 0.4f));
        Orbit2.Rotate(new Vector3(0, 0, 0.3f));
        Orbit3.Rotate(new Vector3(0, 0, 0.2f));
        Orbit4.Rotate(new Vector3(0, 0, 0.1f));
    }

    public void AddStar()
    {

    }
}
