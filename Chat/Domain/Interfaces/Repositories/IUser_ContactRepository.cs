using Chat.Domain.Models;

namespace Chat.Domain.Interfaces.Repositories
{
    public interface IUser_ContactRepository : IRepository<UserContact>
    {
        List<UserContact> GetContactsByUserId(int userId);

        bool UserIsContact(int userId, int targetId);

        bool AddUserContact(int userId, int targetId);

        bool RemoveUserContact(int userId, int contactId);
    }
}
