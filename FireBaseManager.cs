using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project12
{
    internal class FireBaseManager
    {
        FirebaseClient firebase = new FirebaseClient("https://project12-f950c-default-rtdb.europe-west1.firebasedatabase.app/");

        public async Task AddAccount(Account account)
        {
            await firebase.Child("Accounts").Child(account.Name).PutAsync<Account>(account);
        }

        //get a single object
        public async Task<Account> GetAccount(string title)
        {
            return await firebase.Child("Accounts").Child(title).OnceSingleAsync<Account>();
        }

        //get a list of all the objects in the firebase
        public async Task<List<Account>> GetAllAccounts()
        {
            return (await firebase.Child("Accounts").OnceAsync<Account>()).Select
                (item => new Account(
                item.Object.Name,
                item.Object.Hashed_passward,
                item.Object.Remainder,
                item.Object.Transfers)
            ).ToList();
        }

        //delete a Account by its title
        public async Task DeleteAccount(string name)
        {
            await firebase.Child("Accounts").Child(name).DeleteAsync();
        }

        public async Task Clean()
        {
            List<Account> accounts = await GetAllAccounts();
            foreach (Account a in accounts)
            {
                await DeleteAccount(a.Name);
            }
        }


        public async Task<bool> CheckPassword(string username, string password)
        {
            Account account = await GetAccount(username);
            return (account.Hashed_passward) == (Utilities.GetHashString(password));
        }
    }
}