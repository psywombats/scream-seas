using UnityEngine;
using System.Collections;
using System;

/**
 *  When the player is waving a cursor all over the place we probably want some callbacks.
 */
public class Scanner {

    public Action<Vector2Int> scan;
    public Action close;

    public Scanner(Action<Vector2Int> scan, Action close) {
        this.scan = scan;
        this.close = close;
    }
}
