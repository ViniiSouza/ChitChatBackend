namespace ChatAPI.Hubs
{
    public static class HubConnections
    {
        private static Dictionary<string, List<string>> OnlineUsers = new();

        public static bool UserHasConnection(string userId, string connectionId)
        {
            return OnlineUsers.ContainsKey(userId) && OnlineUsers[userId].Any(any => any.Contains(connectionId));
        }

        public static bool HasUser(string userId)
        {
            return OnlineUsers.ContainsKey(userId) && OnlineUsers[userId].Any();
        }

        public static bool UserHasConnectionLimit(string userId)
        {
            return HasUser(userId) && OnlineUsers[userId].Count >= 3;
        }

        public static void AddUserConnection(string userId, string connectionId)
        {
            if (!UserHasConnection(userId, connectionId))
            {
                AddToDictionary(userId, connectionId);
            }
        }

        public static void RemoveUserConnection(string userId, string connectionId)
        {
            if (UserHasConnection(userId, connectionId))
            {
                RemoveFromDictionary(userId, connectionId);
            }
        }

        public static List<string> GetOnlineUsers()
        {
            return OnlineUsers.Keys.ToList();
        }

        public static List<string>? GetConnectionsByUser(string userId)
        {
            if (OnlineUsers.ContainsKey(userId))
            {
                return OnlineUsers[userId].ToList();
            }
            return null;
        }

        private static void AddToDictionary(string key, string value)
        {
            if (OnlineUsers.ContainsKey(key))
                OnlineUsers[key].Add(value);
            else
                OnlineUsers.Add(key, new List<string> { value });
        }

        private static void RemoveFromDictionary(string key, string value)
        {
            if (OnlineUsers.ContainsKey(key))
            {
                OnlineUsers[key].Remove(value);
                if (!OnlineUsers[key].Any())
                {
                    OnlineUsers.Remove(key);
                }
            }
        }
    }
}
