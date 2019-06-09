using UnityEngine;
using System.Collections;

public class ListCell : MonoBehaviour {

    public GameObject selectedState;

    public virtual void SetSelected(bool selected) {
        selectedState.SetActive(selected);
    }
}
