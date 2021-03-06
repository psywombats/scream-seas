﻿using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    public NVLComponent nvl;
    public ShantyComponent shanty;
    public GameOverUIComponent go;
    public CrypticMessageComponent message;
}
