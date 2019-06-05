using UnityEngine;

[RequireComponent(typeof(Grid))]
public class ObjectLayer : MonoBehaviour {

    public Map parent {
        get {
            return GetComponentInParent<Map>();
        }
    }

}
