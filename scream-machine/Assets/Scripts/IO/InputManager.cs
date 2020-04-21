using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InputManager : MonoBehaviour {

    public enum Command {
        Left,
        Right,
        Up,
        Down,
        Confirm,
        Menu,
        Cancel,
        Debug,
        Phune,
        Quit
    };

    public enum Event {
        Down,
        Up,
        Hold,
        Repeat,
    };

    private static readonly float KeyRepeatSeconds = 0.5f;

    private Dictionary<Command, List<KeyCode>> keybinds;
    private List<IInputListener> listeners;
    private List<IInputListener> disabledListeners;
    private Dictionary<Command, float> holdStartTimes;
    private List<KeyCode> fastKeys;
    private Dictionary<string, IInputListener> anonymousListeners;

    public void Awake() {
        keybinds = new Dictionary<Command, List<KeyCode>>();
        keybinds[Command.Left] = new List<KeyCode>(new[] { KeyCode.LeftArrow, KeyCode.A, KeyCode.Keypad4 });
        keybinds[Command.Right] = new List<KeyCode>(new[] { KeyCode.RightArrow, KeyCode.D, KeyCode.Keypad6 });
        keybinds[Command.Up] = new List<KeyCode>(new[] { KeyCode.UpArrow, KeyCode.W, KeyCode.Keypad8 });
        keybinds[Command.Down] = new List<KeyCode>(new[] { KeyCode.DownArrow, KeyCode.S, KeyCode.Keypad2 });
        keybinds[Command.Confirm] = new List<KeyCode>(new[] { KeyCode.Space, KeyCode.Z, KeyCode.Return, KeyCode.KeypadEnter });
        keybinds[Command.Cancel] = new List<KeyCode>(new[] { KeyCode.B, KeyCode.X, KeyCode.Backspace });
        keybinds[Command.Debug] = new List<KeyCode>(new[] { KeyCode.F9 });
        keybinds[Command.Menu] = new List<KeyCode>(new[] { KeyCode.Escape });
        keybinds[Command.Phune] = new List<KeyCode>(new[] { KeyCode.Tab });
        keybinds[Command.Quit] = new List<KeyCode>(new[] { KeyCode.Q });
        fastKeys = new List<KeyCode>(new[] { KeyCode.LeftControl, KeyCode.RightControl, KeyCode.Space, KeyCode.Z });

        listeners = new List<IInputListener>();
        disabledListeners = new List<IInputListener>();

        listeners = new List<IInputListener>();
        holdStartTimes = new Dictionary<Command, float>();

        anonymousListeners = new Dictionary<string, IInputListener>();
    }

    private List<IInputListener> listenersTemp = new List<IInputListener>();
    public void Update() {
        listenersTemp.Clear();
        listenersTemp.AddRange(listeners);

        try {
            foreach (Command command in Enum.GetValues(typeof(Command))) {
                foreach (IInputListener listener in listenersTemp) {
                    if (disabledListeners.Contains(listener)) {
                        continue;
                    }
                    bool endProcessing = false; // ew.
                    foreach (KeyCode code in keybinds[command]) {
                        if (Input.GetKeyDown(code)) {
                            endProcessing |= listener.OnCommand(command, Event.Down);
                        }
                        if (Input.GetKeyUp(code)) {
                            endProcessing |= listener.OnCommand(command, Event.Up);
                            holdStartTimes.Remove(command);
                        }
                        if (Input.GetKey(code)) {
                            if (!holdStartTimes.ContainsKey(command)) {
                                holdStartTimes[command] = Time.time;
                            }
                            endProcessing |= listener.OnCommand(command, Event.Hold);
                            if (Time.time - holdStartTimes[command] > KeyRepeatSeconds) {
                                endProcessing |= listener.OnCommand(command, Event.Repeat);
                            }
                        }
                        if (endProcessing) break;
                    }
                    if (endProcessing) break;
                }
            }
        } catch (Exception e) {
            Debug.LogError("Error in main input loop: " + e);
        }
    }

    public void PushListener(string id, Func<Command, Event, bool> responder) {
        IInputListener listener = new AnonymousListener(responder);
        anonymousListeners[id] = listener;
        PushListener(listener);
    }
    public void PushListener(IInputListener listener) {
        listeners.Insert(0, listener);
    }

    public void RemoveListener(string id) {
        listeners.Remove(anonymousListeners[id]);
    }
    public void RemoveListener(IInputListener listener) {
        listeners.Remove(listener);
    }

    public void DisableListener(IInputListener listener) {
        disabledListeners.Add(listener);
    }

    public void EnableListener(IInputListener listener) {
        if (disabledListeners.Contains(listener)) {
            disabledListeners.Remove(listener);
        }
    }

    public bool IsFastKeyDown() {
        foreach (KeyCode code in fastKeys) {
            if (Input.GetKey(code)) {
                return true;
            }
        }
        return false;
    }

    public IEnumerator ConfirmRoutine() {
        var id = "confirm";
        var done = false;
        PushListener(id, (command, type) => {
            if (type == Event.Down) {
                RemoveListener(id);
                done = true;
            }
            return true;
        });
        while (!done) yield return null;
    }
}
