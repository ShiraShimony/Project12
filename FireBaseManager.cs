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
            await firebase.Child("Accounts").Child(account.Id).PutAsync<Account>(account);
        }

        //get a single object
        public async Task<Account> GetAccount(string title)
        {
            return await firebase.Child("Accounts").Child(title).OnceSingleAsync<Account>();
        }

        //get a list of all the objects in the firebase
        public async Task<List<Account>> GetAllAccounts()
        {
            return (await firebase.Child("Accounts").OnceAsync<Account>()).Select(item => new Account(
                item.Object.Price,
                item.Object.Title,
                item.Object.SubTitle,
                item.Object.Bitmap,
                item.Object.Location)
            ).ToList();
        }

        //delete a Account by its title
        public async Task DeleteAccount(string title)
        {
            await firebase.Child("Accounts").Child(title).DeleteAsync();
        }

    }
}