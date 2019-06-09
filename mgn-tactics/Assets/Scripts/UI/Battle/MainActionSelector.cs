using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(ListSelector))]
public class MainActionSelector : MonoBehaviour {

    private ListSelector selector { get { return GetComponent<ListSelector>(); } }

    public ListStringCell cellPrefab;

    public IEnumerator SelectMainActionRoutine(Result<MainActionType> result) {
        var a = Enum.GetValues(typeof(MainActionType));
        yield return selector.SelectRoutine(result, new List<MainActionType>((MainActionType[])a), CellConstructor);
    } 

    private ListCell CellConstructor(MainActionType type) {
        ListStringCell cell = Instantiate(cellPrefab.gameObject).GetComponent<ListStringCell>();
        cell.Populate(type.ToString());
        return cell;
    }
}
