using UnityEngine;
using UnityEngine.UI;

public class TimeEffectComponent : MonoBehaviour {

    [SerializeField] private Image img = null;

    public bool Enabled { get; set; }
    
    private float elapsed;

    public void Update() {
        elapsed += Time.deltaTime;
        if (elapsed > 100) elapsed = 0;
        
        img.material.SetFloat("_Elapsed", elapsed);
    }
}
