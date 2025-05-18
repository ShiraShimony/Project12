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
    public class FirebaseManager
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseManager(string firebaseUrl)
        {
            _firebaseClient = new FirebaseClient(firebaseUrl);
        }

        public async Task SaveAccountAsync(Account account)
        {
            await _firebaseClient
                .Child("accounts")
                .Child(account.Name)
                .PutAsync(account);
        }

        public async Task<Account> GetAccountAsync(string title)
        {
            var account = await _firebaseClient.Child("accounts").Child(title).OnceSingleAsync<Account>();
            if (account == null)
                throw new AccountNotFoundException(title);
            return account;
        }

        public async Task<List<Account>> GetAccountsAsync()
        {
            var accounts = await _firebaseClient
                .Child("accounts")
                .OnceAsync<Account>();

            return accounts.Select(a => a.Object).ToList();
        }

        public async Task UpdateAccountAsync(string accountId, Account updatedAccount)
        {
            await _firebaseClient
                .Child("accounts")
                .Child(accountId)
                .PutAsync(updatedAccount);
        }

        public async Task DeleteAccountAsync(string accountId)
        {
            await _firebaseClient
                .Child("accounts")
                .Child(accountId)
                .DeleteAsync();
        }

        public async Task SendTransferAsync(string fromName, string toName, int amount, bool isARequest)
        {
            if (amount <= 0)
                throw new InvalidTransferAmountException(amount);

            var fromAcc = await GetAccountAsync(fromName);
            var toAcc = await GetAccountAsync(toName);

            if (fromAcc.Transfers == null)
                throw new MissingTransfersException(fromName);
            if (toAcc.Transfers == null)
                throw new MissingTransfersException(toName);

            string id = Guid.NewGuid().ToString();
            var newTransfer = new Transfer(DateTime.Now.ToString("dd/MM/yyyy"), fromName, toName, amount, isARequest, Transfer.RequestStatus.waiting);

            toAcc.Transfers[id] = newTransfer;
            fromAcc.Transfers[id] = newTransfer;

            toAcc.Transfers = toAcc.Transfers
                .OrderBy(kv => kv.Value.Status)
                .ThenBy(kv => DateTime.TryParse(kv.Value.Date, out var dt) ? dt : DateTime.MaxValue)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            fromAcc.Transfers = fromAcc.Transfers
                .OrderBy(kv => kv.Value.Status)
                .ThenBy(kv => DateTime.TryParse(kv.Value.Date, out var dt) ? dt : DateTime.MaxValue)
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            await SaveAccountAsync(toAcc);
            await SaveAccountAsync(fromAcc);
        }

        public async Task<Transfer?> GetTransferAsync(string accountName, string transferId)
        {
            var transfer = await _firebaseClient
                .Child("accounts")
                .Child(accountName)
                .Child("Transfers")
                .Child(transferId)
                .OnceSingleAsync<Transfer>();

            if (transfer == null)
                throw new TransferNotFoundException(transferId, accountName);

            return transfer;
        }

        public async Task UpdateTransferAsync(string accountName, string transferId, Transfer updatedTransfer)
        {
            await _firebaseClient
                .Child("accounts")
                .Child(accountName)
                .Child("Transfers")
                .Child(transferId)
                .PutAsync(updatedTransfer);
        }

        public async Task ApproveTransfer(string transferId, string destId)
        {
            var transfer = await GetTransferAsync(destId, transferId);

            if (transfer.Status != Transfer.RequestStatus.waiting)
                throw new InvalidTransferStatusException(transferId, transfer.Status);

            Account dest = await GetAccountAsync(destId);
            Account source = await GetAccountAsync(transfer.Source);

            if (dest.Transfers == null)
                throw new MissingTransfersException(destId);
            if (source.Transfers == null)
                throw new MissingTransfersException(transfer.Source);

            int transferCoefficient = transfer.IsARequest ? -1 : 1;

            dest.addAmmount(transfer.Amount * transferCoefficient);
            source.addAmmount(transfer.Amount * transferCoefficient * -1);

            transfer.Status = Transfer.RequestStatus.approved;
            dest.Transfers[transferId] = transfer;
            source.Transfers[transferId] = transfer;

            await UpdateAccountAsync(dest.Name, dest);
            await UpdateAccountAsync(source.Name, source);
        }

        public async Task RejectTransferAsync(string transferId, string destId)
        {
            var transfer = await GetTransferAsync(destId, transferId);

            if (transfer.Status != Transfer.RequestStatus.waiting)
                throw new InvalidTransferStatusException(transferId, transfer.Status);

            Account dest = await GetAccountAsync(destId);
            Account source = await GetAccountAsync(transfer.Source);

            if (dest.Transfers == null)
                throw new MissingTransfersException(destId);
            if (source.Transfers == null)
                throw new MissingTransfersException(transfer.Source);

            transfer.Status = Transfer.RequestStatus.rejected;

            dest.Transfers[transferId] = transfer;
            source.Transfers[transferId] = transfer;

            await UpdateAccountAsync(dest.Name, dest);
            await UpdateAccountAsync(source.Name, source);
        }
    }
}