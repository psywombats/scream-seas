/// <summary>
/// A person who sends messages
/// </summary>
public class Client {

    public string ClientName { get; private set; }

    public Client(string name) {
        ClientName = name;
    }
}
