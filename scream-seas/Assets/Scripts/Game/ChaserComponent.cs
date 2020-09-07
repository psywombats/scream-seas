using System.Collections;
using UnityEngine;

public class ChaserComponent : MonoBehaviour {

    private MapEvent @event;
    public SpriteRenderer backer;
    public float range = 2.5f;
    public float min = .3f;
    public float scale = 2.0f;

    public void Start() {
        @event = GetComponent<MapEvent>();
        @event.SpeedMult = 0.5f;

        GetComponent<Dispatch>().RegisterListener("collide", _ => {
            Debug.Log("GAME OVER");
        });
    }

    public void Update() {
        if (!@event.Tracking) {
            Stalk();
        }
        @event = GetComponent<MapEvent>();
        if (@event.Position == Global.Instance().Maps.Avatar.Event.Position && !going) {
            going = true;
            Global.Instance().StartCoroutine(GameOverRoutine());
        }

        var dist = (Global.Instance().Maps.Avatar.Event.PositionPx - @event.PositionPx).magnitude;
        var r = 1.0f - (dist / range);
        if (r > 1) r = 1;
        if (r <= min) {
            backer.transform.localScale = new Vector3(min * scale, min * scale, 1);
        } else {
            backer.transform.localScale = new Vector3( r* scale, r * scale, 1);
        }
    }

    private bool going = false;
    private IEnumerator GameOverRoutine() {
        Global.Instance().Maps.Avatar.PauseInput();
        Global.Instance().Data.SetSwitch("chaser_active", false);
        Global.Instance().Data.SetSwitch("chaser_spawning", false);
        Global.Instance().Audio.PlaySFX("chaser_howl");
        Global.Instance().Audio.PlayBGM("none");
        string map;
        if (!Global.Instance().Data.GetSwitch("begin_night_2")) {
            map = "RadioRoom";
            Global.Instance().Data.SetSwitch("night1_chaser", false);
        } else {
            map = "deck2";
            Global.Instance().Data.SetSwitch("chaser_redux", false);
            Global.Instance().Data.SetSwitch("redux_once", false);
        }
        yield return MapOverlayUI.Instance().go.GameOverRoutine();
        yield return Global.Instance().Maps.TeleportRoutine(map, "restart");
        yield return MapOverlayUI.Instance().go.ResumeNormalRoutine();
        going = false;
    }

    public void Stalk() {
        OrthoDir dir;
        var dist = (@event.Position - Global.Instance().Maps.Avatar.Event.Position).magnitude;
        if (dist > 12) return;
        var tooClose = false;
        if (@event.Position.x > Global.Instance().Maps.Avatar.Event.Position.x ^ tooClose) {
            dir = OrthoDir.West;
        } else {
            dir = OrthoDir.East;
        }
        var newPos = @event.Position + dir.XY2D();
        if (@event.CanPassAt(newPos)) {
            var context = Global.Instance().Maps.Lua;
            @event.StartCoroutine(@event.StepRoutine(dir));
        }
    }
}
