using UnityEngine;
using System.Collections;
using DG.Tweening;

public class PauseView : MonoBehaviour {

    [SerializeField] private LuaSerializedScript fixitScript = null;

    private LuaCutsceneContext lua = new LuaCutsceneContext();

    public IEnumerator MenuRoutine() {
        CharaEvent.disableStalk = true;
        yield return CoUtils.RunTween(GetComponent<CanvasGroup>().DOFade(1.0f, 0.8f));
        Global.Instance().Input.PushListener("pause", (cmd, ev) => {
            if (ev != InputManager.Event.Up) return true;
            if (cmd == InputManager.Command.Quit) {
                Application.Quit();
            }
            if (cmd == InputManager.Command.Confirm) {
                StartCoroutine(DieRoutine());
            }
            if (cmd == InputManager.Command.Debug) {
                lua.Initialize();
                StartCoroutine(lua.RunRoutine(new LuaScript(lua, fixitScript.luaString), true));
                StartCoroutine(DieRoutine());
            }
            return true;
        });
    }

    public IEnumerator DieRoutine() {
        Global.Instance().Input.RemoveListener("pause");
        yield return CoUtils.RunTween(GetComponent<CanvasGroup>().DOFade(0.0f, 0.8f));
        CharaEvent.disableStalk = false;
    }
}
