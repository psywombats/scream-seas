using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeakerIndexData", menuName = "Data/Index/SpeakerIndexData")]
public class SpeakerIndexData : GenericIndex<SpeakerData> {

}

[Serializable]
public class SpeakerData : GenericDataObject {

    public Sprite image;
    public Sprite altimage;
}
