namespace EWallet.API;

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Wallet> Wallets { get; set; }
}

public class Wallet
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
}

public class Transaction
{
    public Guid Id { get; set; }
    public Guid FromWalletId { get; set; }
    public Guid ToWalletId { get; set; }
    public decimal Amount { get; set; }
    public long Nonce { get; set; }
    public DateTime Timestamp { get; set; }
    public decimal FromWalletBalanceBefore { get; set; }
    public decimal FromWalletBalanceAfter { get; set; }
    public decimal ToWalletBalanceBefore { get; set; }
    public decimal ToWalletBalanceAfter { get; set; }
}