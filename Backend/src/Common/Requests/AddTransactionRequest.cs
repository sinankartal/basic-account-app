namespace Common.Requests;

public class AddTransactionRequest
{
    public Guid AccountId { get; set; }

    public decimal Amount { get; set; }
}