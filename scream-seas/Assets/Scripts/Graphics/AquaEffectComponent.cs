using UnityEngine;

public class AquaEffectComponent : MonoBehaviour {

    [SerializeField] private new Renderer renderer = null;
    [SerializeField] private bool startsEnabled = true;

    public bool Enabled { get; set; }

    private MaterialPropertyBlock propBlock;
    private float elapsed;

    public void Start() {
        propBlock = new MaterialPropertyBlock();
        Enabled = startsEnabled;
    }

    public void Update() {
        elapsed += Time.deltaTime;

        renderer.GetPropertyBlock(propBlock);
        propBlock.SetFloat("_AquaElapsed", Enabled ? elapsed : 0.0f);
        renderer.SetPropertyBlock(propBlock);
    }
}
