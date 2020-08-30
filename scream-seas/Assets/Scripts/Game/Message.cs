public class Message {

    public Conversation Conversation { get; private set; }
    public string Text { get; private set; }
    public Client Client { get; private set; }

    public Message(Conversation conversation, Client client, string text) {
        Conversation = conversation;
        Text = text;
        Client = client;
    }
}
