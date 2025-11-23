public class WalletDto
{
    public Guid Id { get; set; }
    public long Balance { get; set; }
    public string Currency { get; set; }
    public DateTime CreatedAt { get; set; }
}
