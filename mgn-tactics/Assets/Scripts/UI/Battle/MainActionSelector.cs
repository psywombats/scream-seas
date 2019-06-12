using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(ListSelector))]
public class MainActionSelector : MonoBehaviour {

    private ListSelector selector { get { return GetComponent<ListSelector>(); } }

    public ListStringCell cellPrefab;

    private List<MainActionType> allowedActions;

    public IEnumerator SelectMainActionRoutine(Result<MainActionType> result, List<MainActionType> allowedActions) {
        this.allowedActions = allowedActions;
        var a = (MainActionType[])Enum.GetValues(typeof(MainActionType));
        yield return selector.SelectAndPersistRoutine(result, new List<MainActionType>(a), CellConstructor);
    } 

    private ListCell CellConstructor(MainActionType type) {
        ListStringCell cell = Instantiate(cellPrefab.gameObject).GetComponent<ListStringCell>();
        cell.Populate(type.ToString());
        cell.SetSelectable(allowedActions.Contains(type));
        return cell;
    }
}
