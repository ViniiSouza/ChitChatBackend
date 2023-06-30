namespace Chat.Application.DTOs
{
    public class UserLoginDTO
    {
        public UserLoginDTO(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
