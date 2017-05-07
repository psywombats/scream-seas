﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour, InputListener {

    public List<Image> Cursors;

    private ColorEffect fader;
    private ColorEffect Fader {
        get {
            if (fader == null) {
                fader = FindObjectOfType<ColorEffect>();
            }
            return fader;
        }
    }

    private int cursorIndex;

    public void Start() {
        cursorIndex = 0;
        UpdateDisplay();
        Global.Instance().Input.PushListener(this);
    }

    public void UpdateDisplay() {
        for (int i = 0; i < Cursors.Count; i += 1) {
            Image cursor = Cursors[i];
            cursor.enabled = (i == cursorIndex);
        }
    }
    public bool OnCommand(InputManager.Command command, InputManager.Event eventType) {
        if (eventType != InputManager.Event.Down) {
            return true;
        }
        switch (command) {
            case InputManager.Command.Down:
            case InputManager.Command.Right:
                MoveCursor(1);
                return true;
            case InputManager.Command.Left:
            case InputManager.Command.Up:
                MoveCursor(-1);
                return true;
            case InputManager.Command.Confirm:
                Confirm();
                return true;
            default:
                return true;
        }
    }

    private void MoveCursor(int delta) {
        cursorIndex += delta;
        if (cursorIndex < 0) {
            cursorIndex = Cursors.Count;
        } else if (cursorIndex > Cursors.Count) {
            cursorIndex = 0;
        }
        UpdateDisplay();
    }

    private void Confirm() {
        switch (cursorIndex) {
            case 0:
                NewGame();
                break;
            case 1:
                LoadGame();
                break;
        }
    }

    private void NewGame() {
        Global.Instance().Input.RemoveListener(this);
        StartCoroutine(CoUtils.RunWithCallback(TransitionOutRoutine(), this, () => {
            SceneManager.LoadScene("Scenes/Main", LoadSceneMode.Single);
        }));
    }

    private void LoadGame() {
        StartCoroutine(CoUtils.RunWithCallback(TransitionOutRoutine(), this, () => {
            SceneManager.LoadScene("Scenes/Main", LoadSceneMode.Single);
            Global.Instance().Memory.StartCoroutine(CoUtils.RunAfterDelay(0.0f, () => {
                Global.Instance().Memory.LoadMemory(Global.Instance().Memory.GetMemoryForSlot(0));
            }));
        }));
    }

    private IEnumerator TransitionOutRoutine() {
        yield return StartCoroutine(Fader.ChangeColorRoutine(Color.black, 1.0f));
    }
}
