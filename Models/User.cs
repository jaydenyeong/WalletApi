public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } = "User";
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Wallet Wallet { get; set; }
}
