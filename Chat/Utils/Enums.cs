namespace Chat.Utils.Enums
{
    public enum EChatType
    {
        None = 0,
        Private = 1,
        Group = 2
    }

    public enum EMessageAction
    {
        None = 0,
        Content = 1,
        Creation = 2,
        Addition = 3,
        Removal = 4
    }

    public enum EMessageSender
    {
        None = 0,
        Self = 1,
        Other = 2
    }

    public enum ESearchUserType
    {
        None = 0,
        PublicProfile = 1,
        PrivateProfile = 2,
        HasPermission = 3,
        AlreadyHasChat = 4,
        AlreadySentInvite = 5,
    }
}
