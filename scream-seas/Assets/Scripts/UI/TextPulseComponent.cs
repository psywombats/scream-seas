using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextPulseComponent : MonoBehaviour {

    [SerializeField] private float duration = 2.0f;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private bool startsActive = false;
    [SerializeField] private bool endless = false;

    private bool active;
    public bool Active {
        get => active;
        set {
            active = value;
            elapsed = 0.0f;
        }
    }

    private float elapsed = 0.0f;
    private Color startColor;

    private Text txt;
    private Text Text {
        get {
            if (txt == null) {
                txt = GetComponent<Text>();
            }
            return txt;
        }
    }

    public void Start() {
        Active = startsActive;
        startColor = Text.color;
    }

    private bool hitMax;
    public void Update() {
        if (!Active) {
            Text.color = startColor;
        } else {
            elapsed += Time.deltaTime;
            float t = Mathf.Sin(elapsed * (2 * Mathf.PI) / duration);
            if (!endless && hitMax) t = 1.0f;
            hitMax = t >= .99f;
            Text.color = new Color(
                t * color.r + (1.0f - t) * startColor.r, 
                t * color.g + (1.0f - t) * startColor.g, 
                t * color.b + (1.0f - t) * startColor.b,
                t * color.a + (1.0f - t) * startColor.a);
        }
    }
}
