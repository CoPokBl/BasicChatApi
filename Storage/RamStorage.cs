using BasicChatApi.Schemas;

namespace BasicChatApi.Storage; 

public class RamStorage : IStorageMethod {

    private List<Message> _messages;

    public void Init() {
        _messages = new List<Message>();
    }

    public void Deinit() { }

    public void CreateMessage(Message message) => _messages.Add(message);

    public void UpdateMessage(Message message) {
        int index = _messages.FindIndex(m => m.MessageId == message.MessageId);
        _messages[index] = message;
    }

    public void DeleteMessage(string id) {
        int index = _messages.FindIndex(m => m.MessageId == id);
        _messages.RemoveAt(index);
    }

    public Message[] GetMessages(int limit, int offset) => _messages.Skip(offset).Take(limit).ToArray();
    
}