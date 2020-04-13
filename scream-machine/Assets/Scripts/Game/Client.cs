using System;

/// <summary>
/// A person who sends messages
/// </summary>
[Serializable]
public class Client : IKeyedDataObject {

    public string Key => key;

    public string key;
    public string displayName;

    public Client(Client other) {
        key = other.key;
        displayName = other.displayName;
    }
}
