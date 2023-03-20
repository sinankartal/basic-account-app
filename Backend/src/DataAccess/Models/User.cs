namespace Persistence.Models;

public class User: BaseEntity
{
    public string Name { get; set; }

    public string Surname { get; set; }

    public List<Account> Accounts { get; set; }
}