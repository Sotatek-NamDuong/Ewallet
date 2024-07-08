using Cassandra;
using System.Linq;

namespace EWallet.API.Services;

public class TransactionService(IScyllaSession session, INotificationService notificationService)
{
    public async Task<bool> CreateTransactionAsync(Guid fromWalletId, Guid toWalletId, decimal amount, long nonce)
    {
        var batch = new BatchStatement();

        // Get current balances
        var fromWallet = await GetWalletAsync(fromWalletId);
        var toWallet = await GetWalletAsync(toWalletId);

        if (fromWallet.Balance < amount)
        {
            return false;
        }

        var transactionId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow;

        // Update wallets
        batch.Add(new SimpleStatement(
            "UPDATE wallets SET balance = balance - ? WHERE id = ?",
            amount, fromWalletId));
        batch.Add(new SimpleStatement(
            "UPDATE wallets SET balance = balance + ? WHERE id = ?",
            amount, toWalletId));

        // Insert transaction
        batch.Add(new SimpleStatement(
            "INSERT INTO transactions (id, from_wallet_id, to_wallet_id, amount, nonce, timestamp, " +
            "from_wallet_balance_before, from_wallet_balance_after, to_wallet_balance_before, to_wallet_balance_after) " +
            "VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)",
            transactionId, fromWalletId, toWalletId, amount, nonce, timestamp,
            fromWallet.Balance, fromWallet.Balance - amount,
            toWallet.Balance, toWallet.Balance + amount));

        // Update statistics
        batch.Add(new SimpleStatement(
            "UPDATE transaction_counts SET count = count + 1 WHERE date = ?",
            timestamp.Date));
        batch.Add(new SimpleStatement(
            "UPDATE transaction_volumes SET volume = volume + ? WHERE date = ?",
            amount, timestamp.Date));
        batch.Add(new SimpleStatement(
            "INSERT INTO active_users (date, user_id) VALUES (?, ?)",
            timestamp.Date, fromWallet.UserId));
        batch.Add(new SimpleStatement(
            "INSERT INTO active_users (date, user_id) VALUES (?, ?)",
            timestamp.Date, toWallet.UserId));

        await session.ExecuteAsync(batch);

        // Send notifications
        await notificationService.SendTransactionNotificationAsync(fromWallet.UserId, transactionId);
        await notificationService.SendTransactionNotificationAsync(toWallet.UserId, transactionId);

        return true;
    }

    private async Task<Wallet> GetWalletAsync(Guid walletId)
    {
        var statement = new SimpleStatement("SELECT id, user_id, balance FROM wallets WHERE id = ?", walletId);
        var resultSet = await session.ExecuteAsync(statement);
        var row = resultSet.FirstOrDefault();
        if (row == null)
        {
            return null;
        }
      
        return new Wallet
        {
            Id = row.GetValue<Guid>("id"),
            UserId = row.GetValue<Guid>("user_id"),
            Balance = row.GetValue<decimal>("balance")
        };
    }
}