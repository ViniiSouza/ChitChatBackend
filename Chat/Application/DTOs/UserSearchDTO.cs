using Chat.Utils.Enums;

namespace Chat.Application.DTOs
{
    public class UserSearchDTO
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public ESearchUserType Type { get; set; }
    }
}
