using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapEvent))]
public class EventSwitchComponent : MonoBehaviour {

    public MapEvent Parent { get { return GetComponent<MapEvent>(); } }

    [SerializeField] private List<GameObject> controlled = null;

    public void Start() {
        GetComponent<Dispatch>().RegisterListener(MapEvent.EventEnabled, (object payload) => {
            UpdateEnabled((bool)payload);
        });
        UpdateEnabled(Parent.IsSwitchEnabled);
    }

    public void UpdateEnabled(bool enabled) {
        foreach (var toControl in controlled) {
            toControl.SetActive(enabled);
        }
    }
}
