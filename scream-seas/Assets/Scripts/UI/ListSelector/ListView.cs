﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

/// <summary>
/// Adds children and populates them
/// </summary>
/// <remarks>
/// Unoptimized. Should work from an object pool in the future.
/// </remarks>
public class ListView : MonoBehaviour {

    [SerializeField] private GameObject prefab = null;

    public event Action<int> OnPopulate;

    public virtual void Populate<T>(IEnumerable<T> data, Action<GameObject, T> populater) {
        var index = 0;
        foreach (var datum in data) {
            GameObject gameObject;
            if (index >= transform.childCount) {
                gameObject = Instantiate(prefab);
                gameObject.transform.SetParent(transform, worldPositionStays: false);
            } else {
                var child = transform.GetChild(index);
                gameObject = child.gameObject;
            }
            
            populater(gameObject, datum);
            
            index += 1;
        }

        int dataSize = index;

        // destroy extra children
        List<GameObject> toDestroy = new List<GameObject>();
        for (; index < transform.childCount; index += 1) {
            toDestroy.Add(transform.GetChild(index).gameObject);
        }
        foreach (var obj in toDestroy) {
            DestroyImmediate(obj);
        }

        OnPopulate?.Invoke(dataSize);
    }

    public IEnumerable GetCells() {
        return transform;
    }

    public GameObject GetCell(int index) {
        return transform.GetChild(index).gameObject;
    }
}
