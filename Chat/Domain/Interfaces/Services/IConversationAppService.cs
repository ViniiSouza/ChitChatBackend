using Chat.Application.DTOs;
using Chat.Domain.Models;
using Chat.Utils.Enums;

namespace Chat.Domain.Interfaces.Services
{
    public interface IConversationAppService : IBaseAppService<ConversationDTO, Conversation>
    {
        List<ConversationSimpleDTO> LoadConversationsByUser(string username);

        ConversationSimpleDTO CreatePrivateByRequest(int messageRequestId, string userName);

        ConversationSimpleDTO CreateAllowedPrivate(ConversationCreateDTO dto, string userName);
    }
}
