using UnityEngine;
using System.Collections.Generic;
using System;

public class ScrollingListSelector<T> : DynamicListSelector {
    
    [SerializeField] private GameObject upArrow = null;
    [SerializeField] private GameObject downArrow = null;

    protected List<T> data;
    protected Action<GameObject, T> populater;
    protected int offset = 0;

    public override int Selection {
        set {
            if (CellCount() == 0) {
                Debug.LogError("No selection possible");
            }
            if (selection >= 0 && selection < CellCount()) {
                GetCell(selection).SetSelected(false);
            }
            selection = value;
            if (selection == 0) {
                if (offset > 0) {
                    offset -= 1;
                    selection += 1;
                    Repopulate();
                }
            }
            if (selection < 0) {
                selection = CellCount() - 1;
                offset = Math.Max(0, data.Count - CellCount());
                Repopulate();
            }
            if (selection == CellCount() - 2) {
                if (offset + CellCount() < data.Count) {
                    offset += 1;
                    Repopulate();
                }
            }
            if (selection == CellCount()) {
                offset = 0;
                selection = 0;
                Repopulate();
            }

            GetCell(selection).SetSelected(true);
            FireSelectionChange();
        }
    }

    public void Populate(List<T> data, Action<GameObject, T> populater) {
        this.data = data;
        this.populater = populater;
        Repopulate();
    }

    public T SelectedData() {
        return data[offset + Selection];
    }

    private void Repopulate() {
        List.Populate(data.GetRange(offset, Math.Min(CellCount(), data.Count - offset)), populater);
        upArrow.SetActive(offset > 0);
        downArrow.SetActive(offset + CellCount() < data.Count);
    }
}
