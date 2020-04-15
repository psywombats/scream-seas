using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PCNewsIndexData", menuName = "Data/Index/PCNews")]
public class PCNewsIndexData : GenericIndex<PCNewsData> {

}

[Serializable]
public class PCNewsData : IKeyedDataObject {

    public string Key => key;

    public string key;
    [Space]
    [TextArea] public string headline;
    [TextArea] public string story1;
    [TextArea] public string story2;
    [TextArea] public string story3;
    [TextArea] public string story4;
    public string date;

}
