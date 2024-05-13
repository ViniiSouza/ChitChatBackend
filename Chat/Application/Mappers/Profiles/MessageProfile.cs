using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Models;

namespace Chat.Application.Mappers.Profiles
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageSimpleDTO>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(from => from.Sender != null ? from.Sender.Name : ""))
                .ForMember(dest => dest.SendingTime, opt => opt.MapFrom(from => from.CreationDate))
                .ForMember(dest => dest.OwnMessage, opt => opt.Ignore());

            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(from => from.Sender != null ? from.Sender.Name : ""))
                .ForMember(dest => dest.SendingTime, opt => opt.MapFrom(from => from.CreationDate))
                .ForMember(dest => dest.OwnMessage, opt => opt.Ignore());

            CreateMap<MessageCreateDTO, Message>()
                .ForMember(dest => dest.ChatId, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(from => from.SendingTime));

            CreateMap<MessageRequest, MessageRequestDTO>().ForMember(dest => dest.Message, opt => opt.MapFrom(from => from.CustomInvite));
        }
    }
}
