using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/[controller]")]
[Authorize]

public class WalletController : ControllerBase
{
    private readonly AppDbContext _db;
    public WalletController(AppDbContext db)
    {
        _db = db;
    }

    // Create wallet
    [HttpPost("create")]
    public IActionResult CreateWallet(WalletDto dto)
    {
        var userId = Guid.Parse(User.FindFirst("id")!.Value);

        // prevent duplicate wallets for the same currency
        var existing = _db.Wallets.FirstOrDefault(w => w.UserId == userId);
        if (existing != null)
        {
            return BadRequest(new { message = "Wallet already exists for this currency" });
        }

        var wallet = new Wallet
        {
            UserId = userId,
            Currency = dto.Currency,
            Balance = 0,
        };

        _db.Wallets.Add(wallet);
        _db.SaveChanges();

        return Ok(new { message = "Wallet created", walletId = wallet.Id });
    }

    // Get wallet by ID

    [HttpGet("{id:guid}")]
    public IActionResult GetWallet(Guid id)
    {
        var userId = Guid.Parse(User.FindFirst("id")!.Value);
        var wallet = _db.Wallets.FirstOrDefault(w => w.Id == id && w.UserId == userId);
        if (wallet == null)
        {
            return NotFound(new { message = "Wallet not found" });
        }
        return Ok(new WalletDto
        {
            Id = wallet.Id,
            Balance = wallet.Balance,
            Currency = wallet.Currency,
            CreatedAt = wallet.CreatedAt
        });
    }

    // Deposit

    [HttpPost("{id:guid}/deposit")]
    public IActionResult Deposit(Guid id, [FromBody] long amount)
    {
        var userId = Guid.Parse(User.FindFirst("id")!.Value);
        var wallet = _db.Wallets.FirstOrDefault(w => w.Id == id && w.UserId == userId);
        if (wallet == null)
        {
            return NotFound(new { message = "Wallet not found" });
        }
        if (amount <= 0)
        {
            return BadRequest(new { message = "Amount must be positive" });
        }
        wallet.Balance += amount;
        _db.SaveChanges();
        return Ok(new { message = "Deposit successful", newBalance = wallet.Balance });
    }

    // Withdraw
    [HttpPost("{id:guid}/withdraw")]
    public IActionResult Withdraw(Guid id, [FromBody] long amount)
    {
        var userId = Guid.Parse(User.FindFirst("id")!.Value);
        var wallet = _db.Wallets.FirstOrDefault(w => w.Id == id && w.UserId == userId);
        if (wallet == null)
        {
            return NotFound(new { message = "Wallet not found" });
        }
        if (amount <= 0)
        {
            return BadRequest(new { message = "Amount must be positive" });
        }
        if (wallet.Balance < amount)
        {
            return BadRequest(new { message = "Insufficient balance" });
        }
        wallet.Balance -= amount;
        _db.SaveChanges();
        return Ok(new { message = "Withdrawal successful", newBalance = wallet.Balance });
    }

}
