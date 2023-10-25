using Gflower.Models;

namespace Gflower.Data.Repository
{
    public interface IAccountRepository
    {
        Account GetAccByID(int accID);
        Account Login(string username, string password);
		Task InsertAccount(Account car);
		Task UpdateAccount(Account car);
        
        Account GetAccByUsername(string username);
    }
}
