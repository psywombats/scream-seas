using UnityEngine;
using System.Collections;

public class ChaserBGMComponent : MonoBehaviour {

    private bool ChaserEnabled => Global.Instance().Data.GetSwitch("chaser_active");
    private bool ChaserSpawning => Global.Instance().Data.GetSwitch("chaser_spawning");
    private MapEvent Chaser => Global.Instance().Maps.Chaser;
    private MapEvent Avatar => Global.Instance().Maps.Avatar.Event;

    public AudioSource ChaserBGM;
    public AudioSource TensionBGM;

    [Space]
    public float minDist = 4.0f;
    public float maxDist = 8.0f;

    private float targetChaser = 0.0f;
    private float targetTension = 1.0f;

    public void Update() {
        if (ChaserSpawning || ChaserEnabled) {
            if (!Global.Instance().Data.GetSwitch("chaser_stealth")) {
                Global.Instance().Audio.PlayBGM("none");
            }
            if (!ChaserBGM.isPlaying) {
                ChaserBGM.Play();
                TensionBGM.Play();
            }

            float distance;
            if (Chaser == null) {
                distance = (Global.Instance().Maps.ChaserSpawnsAt - Time.time) * 2f;
                var chaserX = Global.Instance().Data.GetVariable("chaser_x");
                var chaserY = Global.Instance().Data.GetVariable("chaser_y");
                distance += (Avatar.Position - new Vector2Int(chaserX, chaserY)).magnitude;
            } else {
                distance = (Chaser.PositionPx - Avatar.PositionPx).magnitude;
            }

            var d = (Mathf.Clamp(distance, minDist, maxDist) - minDist) / (maxDist - minDist);
            targetChaser = 1.0f - d;
            targetTension = d;

            ChaserBGM.volume = targetChaser;
            if (Global.Instance().Data.GetSwitch("chaser_stealth")) {
                TensionBGM.volume = 0.0f;
                Global.Instance().Audio.bgmSource.volume = targetTension;
            } else {
                TensionBGM.volume = targetTension;
            }
        } else {
            ChaserBGM.Stop();
            TensionBGM.Stop();
            targetChaser = 0.0f;
            targetTension = 1.0f;
        }
    }
}
