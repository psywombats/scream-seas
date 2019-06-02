using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DummyObject", menuName = "DummyObject")]
public class DummyObject : ScriptableObject {

    [TextArea(12, 24)]
    public string texty;
}
