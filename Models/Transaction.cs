public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid WalletId { get; set; }
    public long Amount { get; set; } // +deposit / -withdraw
    public string Type { get; set; } // DEPOSIT, WITHDRAW
    public string Status { get; set; } = "PENDING"; // PENDING / CONFIRMED
    public string Reference { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Wallet Wallet { get; set; }
}

