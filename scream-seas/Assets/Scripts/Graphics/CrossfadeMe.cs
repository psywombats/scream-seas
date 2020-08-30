using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CrossfadeMe : MonoBehaviour {

    public SpriteRenderer rend;

    public void OnEnable() {
        rend.DOFade(0.0f, 220f).Play();
    }
}
