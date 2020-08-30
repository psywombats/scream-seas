using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class SpritePulseComponent : MonoBehaviour {

    [SerializeField] private float duration = 2.0f;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private bool startsActive = true;

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

    private SpriteRenderer sprite;
    private SpriteRenderer Sprite {
        get {
            if (sprite == null) {
                sprite = GetComponent<SpriteRenderer>();
            }
            return sprite;
        }
    }

    public void Start() {
        Active = startsActive;
        startColor = Sprite.color;
    }

    public void Update() {
        if (!Active) {
            sprite.color = startColor;
        } else {
            elapsed += Time.deltaTime;
            float t = Mathf.Sin(elapsed * (2 * Mathf.PI) / duration);
            sprite.color = new Color(
                t * color.r + (1.0f - t) * startColor.r,
                t * color.g + (1.0f - t) * startColor.g,
                t * color.b + (1.0f - t) * startColor.b,
                t * color.a + (1.0f - t) * startColor.a);
        }
    }
}
