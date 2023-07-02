using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Models;

namespace Chat.Application.Mappers.Profiles
{
    public class ConversationProfile : Profile
    {
        public ConversationProfile()
        {
            CreateMap<Conversation, ConversationSimpleDTO>();
        }
    }
}
