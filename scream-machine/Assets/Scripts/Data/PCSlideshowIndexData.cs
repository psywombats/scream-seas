using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PCSlideshowIndexData", menuName = "Data/Index/PCSlideshow")]
public class PCSlideshowIndexData : GenericIndex<PCSlideshowData> {

}

[Serializable]
public class PCSlideshowData : IKeyedDataObject {

    public string Key => key;

    public string key;
    public List<PCSlide> slides;
    public string postLua;
}

[Serializable]
public class PCSlide {

    public Sprite sprite;
    public bool invertColor;
    [TextArea(10, 10)]public string text;
}
