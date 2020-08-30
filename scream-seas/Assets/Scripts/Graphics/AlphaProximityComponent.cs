using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class AlphaProximityComponent : MonoBehaviour {

    public MapEvent anchor;
    public float minAlpha = 0.0f;
    public float maxAlpha = 1.0f;
    public float minDist = 1.0f;
    public float maxDist = 5.0f;
    
    void Update() {
        float d = Vector3.Distance(anchor.PositionPx, Global.Instance().Maps.Avatar.GetComponent<MapEvent>().PositionPx);
        float a = (Mathf.Clamp(d, minDist, maxDist) - minDist) / (maxDist - minDist);
        a = a * (maxAlpha - minAlpha) + minAlpha;
        Color c = GetComponent<SpriteRenderer>().color;
        c = new Color(c.r, c.g, c.b, a);
        GetComponent<SpriteRenderer>().color = c;
    }
}
