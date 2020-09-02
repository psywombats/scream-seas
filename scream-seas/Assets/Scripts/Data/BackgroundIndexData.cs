using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundIndexData", menuName = "Data/Index/BackgroundIndexData")]
public class BackgroundIndexData : GenericIndex<BackgroundData> {

}

[Serializable]
public class BackgroundData : GenericDataObject {

    public Sprite bg;
}
