using Newtonsoft.Json;
using System;

[JsonObject(MemberSerialization.OptIn)]
public class Setting<T> {

    [JsonProperty]
    private T value;
    public T Value {
        get {
            return value;
        }
        set {
            this.value = value;
            OnModify?.Invoke();
        }
    }

    public event Action OnModify;
    
    public Setting(T defaultValue) {
        Value = defaultValue;
    }
}