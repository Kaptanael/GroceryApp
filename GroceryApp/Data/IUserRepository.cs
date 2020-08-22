using GroceryApp.Models;
using System.Threading.Tasks;

namespace GroceryApp.Data
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> GetUserByUserNameAsync(string userName);

        Task<User> GetUserByEmailAsync(string email);
    }
}
