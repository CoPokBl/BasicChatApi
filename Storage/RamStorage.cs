using BasicChatApi.Schemas;

namespace BasicChatApi.Storage; 

public class RamStorage : IStorageMethod {

    private List<(string, Message)>? _messages;

    public void Init() {
        _messages = new List<(string, Message)>();
    }

    public void Deinit() { }

    public void CreateMessage(string channel, Message message) => _messages.Add((channel, message));

    public void UpdateMessage(string channel, Message message) {
        int index = _messages.FindIndex(m => m.Item2.MessageId == message.MessageId);
        (string, Message) msg = _messages[index];
        _messages[index] = (msg.Item1, message);
    }

    public void DeleteMessage(string id) {
        int index = _messages.FindIndex(m => m.Item2.MessageId == id);
        _messages.RemoveAt(index);
    }

    public Message[] GetMessages(string channel, int limit, int offset) =>
        _messages
            .Where(m => m.Item1 == channel)
            .OrderByDescending(m => m.Item2.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .Select(m => m.Item2)
            .ToArray();
}