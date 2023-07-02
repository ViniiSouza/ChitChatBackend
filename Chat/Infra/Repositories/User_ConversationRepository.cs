using Chat.Domain.Interfaces.Repositories;
using Chat.Domain.Models;
using Chat.Infra.Contexts;

namespace Chat.Infra.Repositories
{
    public class User_ConversationRepository : Repository<User_Conversation>, IUser_ConversationRepository
    {
        public User_ConversationRepository(ChatDbContext context) : base(context)
        {
        }

        public void VinculateParticipants(List<User_Conversation> participants)
        {
            _context.Set<User_Conversation>().AddRange(participants);
        }
    }
}
