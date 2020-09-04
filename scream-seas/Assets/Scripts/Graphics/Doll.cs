using UnityEngine;
using System.Collections;

public class Doll : MonoBehaviour {

    public SpriteRenderer Renderer;

    [SerializeField] private SpriteRenderer charaRenderer;
    [SerializeField] private SpriteRenderer eventRenderer;

    public void SetRenderer(bool isEvent) {
        Renderer = isEvent ? eventRenderer : charaRenderer;
        charaRenderer.gameObject.SetActive(!isEvent);
        eventRenderer.gameObject.SetActive(isEvent);
    }
}
