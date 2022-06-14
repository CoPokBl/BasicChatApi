using BasicChatApi.Schemas;

namespace BasicChatApi.Storage; 

public interface IStorageMethod {

    void Init();
    void Deinit();
    
    void CreateMessage(string channel, Message message);
    void UpdateMessage(string channel, Message message);
    void DeleteMessage(string id);
    
    Message[] GetMessages(string channel, int limit, int offset);

}