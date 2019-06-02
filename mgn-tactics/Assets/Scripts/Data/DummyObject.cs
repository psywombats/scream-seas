using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "DummyObject", menuName = "DummyObject")]
public class DummyObject : ScriptableObject {

    [TextArea(12,24)]
    public string texty;
}
