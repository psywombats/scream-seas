using UnityEngine;

public class PlayBGMComponent : MonoBehaviour {

    [SerializeField] private new string tag = null;

    public void OnEnable() {
        Global.Instance().Audio.PlayBGM(tag);
    }
}
