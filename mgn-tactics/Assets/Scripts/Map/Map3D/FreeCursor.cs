using UnityEngine;
using System.Collections;
using System;

public class FreeCursor : TacticsCursor {

    private const string PrefabPath = "Prefabs/Tactics/Cursor";

    public float minTimeBetweenMoves = 0.1f;
    public GameObject reticules;
    
    private float lastStepTime;
    private Result<Vector2Int> awaitingSelect;
    
    public static FreeCursor GetInstance() {
        GameObject prefab = Resources.Load<GameObject>(PrefabPath);
        return Instantiate(prefab).GetComponent<FreeCursor>();
    }

    public override void Enable(Vector2Int initialPosition) {
        base.Enable(initialPosition);
        EnableReticules();
    }

    // waits for the cursor to select
    public IEnumerator AwaitSelectionRoutine(Result<Vector2Int> result, Scanner scanner = null) {
        this.scanner = scanner;
        ScanIfNeeded();

        awaitingSelect = result;
        while (!result.finished) {
            yield return null;
        }
        awaitingSelect = null;
    }

    public void EnableReticules() {
        reticules.SetActive(true);
    }
    public void DisableReticules() {
        reticules.SetActive(false);
    }

    protected override void AttemptDirection(OrthoDir dir) {
        if (Time.fixedTime - lastStepTime < minTimeBetweenMoves) {
            return;
        }
        Vector2Int target = GetComponent<MapEvent>().position + dir.XY3D();
        if (GetComponent<MapEvent>().CanPassAt(target)) {
            StartCoroutine(GetComponent<MapEvent>().StepRoutine(dir));
            lastStepTime = Time.fixedTime;
            ScanIfNeeded();
        }
    }

    protected override void OnCancel() {
        if (awaitingSelect != null) {
            awaitingSelect.Cancel();
        }
    }

    protected override void OnConfirm() {
        if (awaitingSelect != null) {
            awaitingSelect.value = GetComponent<MapEvent>().position;
        }
    }
}
