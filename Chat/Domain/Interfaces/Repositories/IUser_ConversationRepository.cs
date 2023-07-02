using Chat.Domain.Models;

namespace Chat.Domain.Interfaces.Repositories
{
    public interface IUser_ConversationRepository : IRepository<User_Conversation>
    {
        void VinculateParticipants(List<User_Conversation> participants);
    }
}
