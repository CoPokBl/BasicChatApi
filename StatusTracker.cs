namespace BasicChatApi; 

public static class StatusTracker {
    private static readonly Dictionary<string, List<(DateTime, string)>> OnlineUsers = new ();

    public static void UserPinged(string user, string channel) {
        if (!OnlineUsers.ContainsKey(channel)) {
            OnlineUsers[channel] = new List<(DateTime, string)>();
        }
        
        // remove all entries older than 10 seconds
        OnlineUsers[channel].RemoveAll(x => (DateTime.Now - x.Item1).TotalSeconds > 10);
        
        if (OnlineUsers[channel].Any(x => x.Item2 == user)) {
            OnlineUsers[channel].RemoveAll(x => x.Item2 == user);
        }
        OnlineUsers[channel].Add((DateTime.Now, user));
    }
    
    public static List<string> GetOnlineUsers(string channel) {
        if (!OnlineUsers.ContainsKey(channel)) {
            OnlineUsers[channel] = new List<(DateTime, string)>();
        }
        OnlineUsers[channel].RemoveAll(x => (DateTime.Now - x.Item1).TotalSeconds > 10);
        return OnlineUsers[channel].Select(x => x.Item2).ToList();
    }
    
}