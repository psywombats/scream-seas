using UnityEngine;

public class DebugPanel : MonoBehaviour {
    
    public float WalkSpeedMult { get; set; } = 1.0f;

    public static DebugPanel Instance => Global.Instance().debug;
}