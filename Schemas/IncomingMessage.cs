namespace BasicChatApi.Schemas; 

public class IncomingMessage {

    public string CreatorName { get; set; }
    
    public string Text { get; set; }
    
    public Message ToMessage() {
        return new Message {
            CreatorName = CreatorName,
            Text = Text,
            CreatedAt = DateTime.UtcNow.ToBinary(),
            MessageId = Guid.NewGuid().ToString()
        };
    }

}