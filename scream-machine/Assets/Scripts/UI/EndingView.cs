using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndingView : MonoBehaviour {

    [SerializeField] private LuaSerializedScript script = null;
    [Space]
    [SerializeField] private SpriteRenderer protag = null;
    [SerializeField] private SpriteRenderer illumine = null;
    [SerializeField] private SpriteRenderer blocker = null;
    [SerializeField] private new Light light = null;
    [SerializeField] private Camera cam = null;
    [SerializeField] private CanvasGroup canvas = null;
    [Space]
    [SerializeField] private float delayInitial = 3.0f;
    [SerializeField] private float delayProtagColor = 5.0f;
    [SerializeField] private float delayLights = 5.0f;
    [SerializeField] private float delayRaise = 10.0f;
    [SerializeField] private float delayPan = 5.0f;
    [SerializeField] private float delayWhiteout = 12f;
    [SerializeField] private float timeProtagColor = 3.0f;
    [SerializeField] private float timeProtagUncolor = 3.0f;
    [SerializeField] private float timeLightBlast = 10.0f;
    [SerializeField] private float timeSwivel = 8.0f;
    [SerializeField] private float timeRaise = 10f;
    [SerializeField] private float timeReveal = 10f;
    [SerializeField] private float timePaneIllumine = 10f;
    [SerializeField] private float timeWhiteout = 10f;
    [SerializeField] private float timeTitle = 10f;
    [Space]
    [SerializeField] private float constLightBlast = 100.0f;
    [SerializeField] private float constSwivelAngel = -40.0f;
    [SerializeField] private float constCamHeight = 2.8f;
    [SerializeField] private float constIlluminePan = 2.31f;

    private LuaCutsceneContext lua = new LuaCutsceneContext();

    public void OnEnable() {
        lua.Initialize();
        RunScene();
    }

    public void PlayEnding() {
        StartCoroutine(EndingRoutine());
    }

    public IEnumerator EndingRoutine() {
        Global.Instance().Data.SetSwitch("ending_mode", true);

        // initial delay
        yield return CoUtils.Wait(delayInitial);

        // show the protagonist
        protag.DOColor(new Color(1, 1, 1), timeProtagColor).SetEase(Ease.Linear).Play();
        yield return CoUtils.Wait(delayProtagColor);

        // turn on the light
        // black out the protagonist
        DOTween.To(() => light.range, x => light.range = x, constLightBlast, timeLightBlast).SetEase(Ease.Linear).Play();
        protag.DOColor(new Color(0, 0, 0), timeProtagColor).SetEase(Ease.Linear).Play();
        yield return CoUtils.Wait(delayLights);

        // pan the camera upwards
        cam.transform.DOLocalRotate(new Vector3(constSwivelAngel, 0, 0), timeSwivel).SetEase(Ease.Linear).Play();
        yield return CoUtils.Wait(timeSwivel);

        // elevate the camera
        cam.transform.DOLocalMoveY(constCamHeight, timeRaise).SetEase(Ease.Linear).Play();
        yield return CoUtils.Wait(delayRaise);

        // fade to normal color
        // pan illumine
        blocker.DOFade(0.0f, timeReveal).SetEase(Ease.Linear).Play();
        illumine.transform.DOLocalMoveY(constIlluminePan, timePaneIllumine).SetEase(Ease.Linear).Play();
        yield return CoUtils.Wait(delayPan);

        // fade to white
        illumine.DOFade(0.0f, timeWhiteout).SetEase(Ease.Linear).Play();
        yield return CoUtils.Wait(delayWhiteout);

        // show the title
        canvas.DOFade(1.0f, timeTitle).SetEase(Ease.Linear).Play();
        yield return Global.Instance().Input.ConfirmRoutine();

        SceneManager.LoadScene("Title");
    }

    private void RunScene() {
        StartCoroutine(lua.RunRoutine(new LuaScript(lua, script.luaString), true));
    }

}
