using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MapEvent))]
public abstract class TacticsCursor : MonoBehaviour, InputListener {

    private const float ScrollSnapTime = 0.2f;

    protected Scanner scanner;

    public virtual void Enable(Vector2Int initialPosition) {
        gameObject.SetActive(true);
        GetComponent<MapEvent>().SetPosition(initialPosition);
        Global.Instance().Input.PushListener(this);
        TacticsCam.Instance().target = GetComponent<MapEvent>();
        TacticsCam.Instance().snapTime = ScrollSnapTime;
    }

    public virtual void Disable() {
        Global.Instance().Input.RemoveListener(this);
        if (TacticsCam.Instance() != null && TacticsCam.Instance().target == GetComponent<MapEvent>()) {
            TacticsCam.Instance().target = null;
        }
        if (scanner != null) {
            scanner.close();
        }
        gameObject.SetActive(false);
    }

    public bool OnCommand(InputManager.Command command, InputManager.Event eventType) {
        if (GetComponent<MapEvent>().tracking || eventType != InputManager.Event.Down) {
            return true;
        }
        switch (command) {
            case InputManager.Command.Down:
                AttemptDirection(OrthoDir.South);
                break;
            case InputManager.Command.Left:
                AttemptDirection(OrthoDir.West);
                break;
            case InputManager.Command.Right:
                AttemptDirection(OrthoDir.East);
                break;
            case InputManager.Command.Up:
                AttemptDirection(OrthoDir.North);
                break;
            case InputManager.Command.Confirm:
                OnConfirm();
                break;
            case InputManager.Command.Cancel:
                OnCancel();
                break;
        }
        return true;
    }

    protected abstract void AttemptDirection(OrthoDir dir);
    protected abstract void OnCancel();
    protected abstract void OnConfirm();

    protected void ScanIfNeeded() {
        if (scanner != null) {
            scanner.scan(GetComponent<MapEvent>().position);
        }
    }
}
