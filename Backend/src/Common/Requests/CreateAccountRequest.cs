namespace Common.Requests;

public class CreateAccountRequest
{
    public Guid UserID { get; set; }

    public decimal InitialAmount { get; set; }
}