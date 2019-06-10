using UnityEngine;
using System.Collections;

public class ListCell : MonoBehaviour {

    public GameObject selectedState;

    private bool selectable;

    public virtual void SetSelected(bool selected) {
        selectedState.SetActive(selected);
    }

    public virtual void SetSelectable(bool selectable) {
        this.selectable = selectable;
    }
}
