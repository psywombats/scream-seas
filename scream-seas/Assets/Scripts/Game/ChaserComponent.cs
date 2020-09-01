using UnityEngine;

public class ChaserComponent : MonoBehaviour {

    private MapEvent @event;

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
