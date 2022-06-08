namespace BasicChatApi.Schemas; 

public class Message {
    
    public string MessageId { get; set; }
    
    public string CreatorName { get; set; }
    
    public string Text { get; set; }
    
    public long CreatedAt { get; set; }

}