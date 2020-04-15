using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ForeignPhoneData : IKeyedDataObject {

    public string Key => key;

    public string key;
    public List<SmsScript> toRun;
    public Sprite frame;
}
