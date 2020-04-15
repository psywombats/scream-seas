using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MapOverlayUI : MonoBehaviour {

    private static MapOverlayUI _instance;
    private static Scene _lastScene;
    public static MapOverlayUI Instance() {
        Scene scene = SceneManager.GetActiveScene();
        if (_lastScene != scene) {
            _lastScene = scene;
            _instance = null;
        }
        if (_instance == null) {
            _instance = FindObjectOfType<MapOverlayUI>();
        }
        return _instance;
    }

    public Textbox textbox;
    public BigPhoneComponent bigPhone;
    public BigPhoneComponent foreignPhone;
    public PhoneComponent miniPhone;
    public PhoneSystem phoneSystem;
}
