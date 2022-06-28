using System.Data;
using BasicChatApi.Schemas;
using MySql.Data.MySqlClient;

namespace BasicChatApi.Storage; 

public class MySqlStorage : IStorageMethod {
    
    private MySqlConnection? _connection;  // MySQL Connection Object
    private string? _connectString;
    
    public void Init() {
        Logger.Info("Connecting to MySQL...");
        _connectString = $"server={Program.Config["mysql_ip"]};" +
                         $"userid={Program.Config["mysql_user"]};" +
                         $"password={Program.Config["mysql_password"]};" +
                         $"database={Program.Config["mysql_database"]}";

        try {
            _connection = new MySqlConnection(_connectString);
            _connection.Open();
        }
        catch (Exception e) {
            Logger.Debug(e.ToString());
            throw new StorageFailException("Failed to connect to MySQL");
        }
        Logger.Info("Connected MySQL");
        _connection.StateChange += DatabaseConnectStateChanged;
        Logger.Debug($"MySQL Version: {_connection.ServerVersion}");
        Logger.Info("Creating tables in MySQL...");
        CreateTables();
        Logger.Info("Created MySQL tables");
    }

    public void Deinit() {
        if (_connection == null) {
            Logger.Info("MySQL not initialized, skipping deinit");
            return;
        }
        
        try {
            _connection.Close();
        }
        catch (Exception) {
            Logger.Error("Failed to close MySQL connection");
        }
    }

    public void CreateMessage(string channel, Message message) {
        if (_connection == null) {
            Logger.Info("MySQL not initialized, skipping message creation");
            return;
        }
        
        try {
            MySqlCommand command = new MySqlCommand(
                "INSERT INTO chat_messages " +
                "       (channel,  id,  creatorName,  text,  createdAt,  signature,  publicKey) " +
                "VALUES (@channel, @id, @creatorName, @text, @createdAt, @signature, @publicKey)",
                _connection
            );
            command.Parameters.AddWithValue("@channel", channel);
            command.Parameters.AddWithValue("@id", channel);
            command.Parameters.AddWithValue("@creatorName", message.CreatorName);
            command.Parameters.AddWithValue("@text", message.Text);
            command.Parameters.AddWithValue("@createdAt", message.CreatedAt);
            command.Parameters.AddWithValue("@signature", message.Signature);
            command.Parameters.AddWithValue("@publicKey", message.PublicKey);
            command.ExecuteNonQuery();
        }
        catch (Exception e) {
            Logger.Error(e.ToString());
            throw new StorageFailException("Failed to create message");
        }
    }

    public void UpdateMessage(string channel, Message message) {
        if (_connection == null) {
            Logger.Info("MySQL not initialized, skipping message update");
            return;
        }
        
        try {
            MySqlCommand command = new MySqlCommand(
                "UPDATE chat_messages SET" +
                "       creatorName = @creatorName, " +
                "       text = @text, " +
                "       createdAt = @createdAt, " +
                "       signature = @signature, " +
                "       publicKey = @publicKey " +
                "WHERE id = @id",
                _connection
            );
            command.Parameters.AddWithValue("@channel", channel);
            command.Parameters.AddWithValue("@id", message.MessageId);
            command.Parameters.AddWithValue("@creatorName", message.CreatorName);
            command.Parameters.AddWithValue("@text", message.Text);
            command.Parameters.AddWithValue("@createdAt", message.CreatedAt);
            command.Parameters.AddWithValue("@signature", message.Signature);
            command.Parameters.AddWithValue("@publicKey", message.PublicKey);
            command.ExecuteNonQuery();
        }
        catch (Exception e) {
            Logger.Error(e.ToString());
            throw new StorageFailException("Failed to update message");
        }
    }

    public void DeleteMessage(string id) {
        if (_connection == null) {
            Logger.Info("MySQL not initialized, skipping message deletion");
            return;
        }
        
        try {
            MySqlCommand command = new MySqlCommand(
                "DELETE FROM chat_messages WHERE id = @id",
                _connection
            );
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }
        catch (Exception e) {
            Logger.Error(e.ToString());
            throw new StorageFailException("Failed to delete message");
        }
    }

    public Message[] GetMessages(string channel, int limit, int offset) {
        if (_connection == null) {
            Logger.Info("MySQL not initialized, skipping message retrieval");
            return new Message[0];
        }
        
        try {
            MySqlCommand command = new MySqlCommand(
                "SELECT * " +
                "FROM chat_messages " +
                "WHERE channel = @channel " +
                "ORDER BY createdAt DESC " +
                "LIMIT @limit " +
                "OFFSET @offset",
                _connection
            );
            command.Parameters.AddWithValue("@channel", channel);
            command.Parameters.AddWithValue("@limit", limit);
            command.Parameters.AddWithValue("@offset", offset);
            MySqlDataReader reader = command.ExecuteReader();
            List<Message> messages = new List<Message>();
            while (reader.Read()) {
                messages.Add(new Message() {
                    CreatedAt = long.Parse(reader.GetString("createdAt")),
                    CreatorName = reader.GetString("creatorName"),
                    MessageId = reader.GetString("id"),
                    PublicKey = reader.GetString("publicKey"),
                    Signature = reader.GetString("signature"),
                    Text = reader.GetString("text")
                });
            }
            reader.Close();
            return messages.ToArray();
        }
        catch (Exception e) {
            Logger.Error(e.ToString());
            throw new StorageFailException("Failed to get messages");
        }
    }
    
    private void CreateTables() {
        SendMySqlStatement(@"CREATE TABLE IF NOT EXISTS chat_messages(
                                channel     VARCHAR(255),
                                id          VARCHAR(64),
                                creatorName VARCHAR(255), 
                                text        VARCHAR(64),
                                createdAt   VARCHAR(64),
                                signature   VARCHAR(64),
                                publicKey   VARCHAR(800)
                                );");
    }
    
    private void SendMySqlStatement(string statement) {
        using MySqlCommand cmd = new ();
        cmd.Connection = _connection;
        cmd.CommandText = statement;
        cmd.ExecuteNonQuery();
    }

    private void DatabaseConnectStateChanged(object obj, StateChangeEventArgs args) {
        if (_connection == null) return;
        
        if (args.CurrentState != ConnectionState.Broken && 
            args.CurrentState != ConnectionState.Closed) {
            return;
        }
            
        // Reconnect
        try {
            _connection = new MySqlConnection(_connectString);
            _connection.Open();
        }
        catch (Exception e) {
            Logger.Error("MySQL reconnect failed: " + e);
            _connection.StateChange -= DatabaseConnectStateChanged;  // Don't loop connect
            throw new StorageFailException("Failed to reconnect to MySQL");
        }
    }
    
}