namespace Persistence.Models;

public class Account : BaseEntity
{
    public string AccountNumber { get; set; }

    public Guid UserId { get; set; }

    public User User { get; set; }
}