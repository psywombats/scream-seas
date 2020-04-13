using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorPulseComponent : MonoBehaviour {

    [SerializeField] private float duration = 2.0f;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private bool startsActive = false;

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

    private Image image;
    private Image Image {
        get {
            if (image == null) {
                image = GetComponent<Image>();
            }
            return image;
        }
    }

    public void Start() {
        Active = startsActive;
        startColor = Image.color;
    }

    public void Update() {
        if (!Active) {
            image.color = startColor;
        } else {
            elapsed += Time.deltaTime;
            float t = Mathf.Sin(elapsed * (2 * Mathf.PI) / duration);
            image.color = new Color(
                t * color.r + (1.0f - t) * startColor.r, 
                t * color.g + (1.0f - t) * startColor.g, 
                t * color.b + (1.0f - t) * startColor.b,
                t * color.a + (1.0f - t) * startColor.a);
        }
    }
}
