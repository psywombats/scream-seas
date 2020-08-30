using UnityEngine;
using System;

[CreateAssetMenu(fileName = "BGMIndexData", menuName = "Data/Index/BGMIndexData")]
public class BGMIndexData : GenericIndex<BGMData> {

}

[Serializable]
public class BGMData : GenericDataObject {

    public AudioClip track;
    public int loopStartPoint;
}
