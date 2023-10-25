using Gflower.Models;
using Microsoft.EntityFrameworkCore;

namespace Gflower.Data.DataAccess
{
    public class AccountManagement
    {
        private static AccountManagement instance = null;
        private static readonly object instanceLock = new object();
        private AccountManagement() { }
        public static AccountManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AccountManagement();
                    }
                    return instance;
                }
            }
        }


        public Account GetAccountByID(int accID)
        {
            Account car = null;
            try
            {
                var flowerDB = new GFlowersContext();
                car = flowerDB.Accounts.SingleOrDefault(acc => acc.AccountId == accID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return car;
        }

        public Account GetAccountByUsername(string username)
        {
            Account car = null;
            try
            {
                var flowerDB = new GFlowersContext();
                car = flowerDB.Accounts.SingleOrDefault(acc => acc.Username == username);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return car;
        }

        public Account GetAccountByUserPass(string username, string password)
        {
            Account car = null;
            try
            {
                var flowerDB = new GFlowersContext();
                car = flowerDB.Accounts.FirstOrDefault(u => u.Username == username && u.Password == password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return car;
        }

        public async Task AddNew(Account acc)
        {
            try
            {
                Account _acc = GetAccountByID(acc.AccountId);
                if (_acc == null)
                {
                    var flowerDB = new GFlowersContext();
					await flowerDB.Accounts.AddAsync(acc);
					await flowerDB.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("The account is already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task Update(Account acc)
        {
            try
            {
                Account c = GetAccountByID(acc.AccountId);
                if (c != null)
                {
                    var flowerDB = new GFlowersContext();
					flowerDB.Entry<Account>(acc).State = EntityState.Modified;
					await flowerDB.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("The account does not already exist.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

