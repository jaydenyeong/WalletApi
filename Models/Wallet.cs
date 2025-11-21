public class Wallet
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public long Balance { get; set; } = 0; // store in cents
    public string Currency { get; set; } = "USD";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
    public ICollection<Transaction> Transactions { get; set; }
}

