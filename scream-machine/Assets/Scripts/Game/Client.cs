/// <summary>
/// A person who sends messages
/// </summary>
public class Client : IKeyedDataObject {

    public string Key { get; private set; }
    public string DisplayName { get; set; }
}
