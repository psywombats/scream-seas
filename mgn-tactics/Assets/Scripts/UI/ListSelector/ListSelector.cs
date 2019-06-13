using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;
using UnityEngine.UI;

public class ListSelector : MonoBehaviour {

    private float HideShowDuration = 0.4f;

    public GameObject childAttachPoint;
    public int selection { get; private set; }

    public void Awake() {
        DestroyAllChildren();
    }

    public List<ListCell> PopulateCells<T>(List<T> elements, Func<T, ListCell> cellConstructor) {
        DestroyAllChildren();
        List<ListCell> allCells = new List<ListCell>();
        foreach (T element in elements) {
            ListCell cell = cellConstructor(element);
            cell.SetSelected(false);
            cell.transform.SetParent(childAttachPoint.transform);
            allCells.Add(cell);
        }
        return allCells;
    }

    public IEnumerator SelectAndCloseRoutine<T>(Result<T> result, List<T> elements, Func<T, ListCell> cellConstructor) {
        yield return SelectAndPersistRoutine(result, elements, cellConstructor);
        yield return ShowHideRoutine(true);
        gameObject.SetActive(false);
    }

    public IEnumerator SelectAndPersistRoutine<T>(Result<T> result, List<T> elements, Func<T, ListCell> cellConstructor) {
        if (!gameObject.activeSelf) {
            gameObject.SetActive(true);
            yield return ShowHideRoutine();
        }
        List<ListCell> cells = PopulateCells(elements, cellConstructor);
        Result<int> subResult = new Result<int>();
        yield return RawSelectRoutine(subResult);
        if (subResult.canceled) {
            yield return ShowHideRoutine(true);
            result.Cancel();
        } else {
            result.value = elements[subResult.value];
        }
    }

    public IEnumerator RawSelectRoutine(Result<int> result) {
        GetCell(selection).SetSelected(true);

        string listenerId = "ListSelector" + gameObject.name;
        Global.Instance().Input.PushListener(listenerId, (InputManager.Command command, InputManager.Event ev) => {
            if (ev != InputManager.Event.Up && ev != InputManager.Event.Repeat) {
                return true;
            }
            switch (command) {
                case InputManager.Command.Cancel:
                    result.Cancel();
                    Global.Instance().Input.RemoveListener(listenerId);
                    break;
                case InputManager.Command.Confirm:
                    result.value = selection;
                    Global.Instance().Input.RemoveListener(listenerId);
                    break;
                case InputManager.Command.Up:
                    selection = MoveSelection(-1);
                    break;
                case InputManager.Command.Down:
                    selection = MoveSelection(1);
                    break;
            }
            return true;
        });

        while (!result.finished) {
            yield return null;
        }
    }

    public IEnumerator ShowHideRoutine(bool hide = false) {
        if (hide && !gameObject.activeSelf) {
            yield break;
        }

        GetComponent<ContentSizeFitter>().enabled = false;
        CanvasGroup group = childAttachPoint.GetComponent<CanvasGroup>();
        RectTransform rect = GetComponent<RectTransform>();

        float endHeight = hide ? 0.0f : LayoutUtility.GetPreferredHeight(rect);
        Vector2 endSize = new Vector2(rect.sizeDelta.x, endHeight);

        if (!hide) {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, 0.0f);
            group.alpha = 0.0f;
        } else {
            selection = 0;
        }
        
        var expandTween = rect.DOSizeDelta(endSize, HideShowDuration / 2.0f);
        var fadeTween = group.DOFade(hide ? 0.0f : 1.0f, HideShowDuration / 2.0f);
        fadeTween.SetEase(Ease.Linear);

        if (hide) {
            yield return CoUtils.RunTween(fadeTween);
            yield return CoUtils.RunTween(expandTween);
        } else {
            yield return CoUtils.RunTween(expandTween);
            yield return CoUtils.RunTween(fadeTween);
        }

        GetComponent<ContentSizeFitter>().enabled = true;
    }

    private void DestroyAllChildren() {
        foreach (Transform child in childAttachPoint.transform) {
            Destroy(child.gameObject);
        }
    }

    private int MoveSelection(int delta) {
        GetCell(selection).SetSelected(false);
        int newSelection = selection + delta;
        if (newSelection < 0) newSelection = childAttachPoint.transform.childCount - 1;
        if (newSelection >= childAttachPoint.transform.childCount) newSelection = 0;
        GetCell(newSelection).SetSelected(true);
        return newSelection;
    }

    private ListCell GetCell(int index) {
        return childAttachPoint.transform.GetChild(index).GetComponent<ListCell>();
    }
}
