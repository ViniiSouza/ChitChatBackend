using Chat.Domain.Models;

namespace Chat.Domain.Interfaces.Repositories
{
    public interface IUser_ContactRepository : IRepository<UserContact>
    {
        List<UserContact> GetContactsByUserId(int userId);
    }
}
