using BasicChatApi.Schemas;

namespace BasicChatApi.Storage; 

public interface IStorageMethod {

    void Init();
    void Deinit();
    
    void CreateMessage(Message message);
    void UpdateMessage(Message message);
    void DeleteMessage(string id);
    
    Message[] GetMessages(int limit, int offset);

}