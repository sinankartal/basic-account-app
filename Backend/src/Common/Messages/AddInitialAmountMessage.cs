namespace Common.Messages;

[Serializable] 
public class AddInitialAmountMessage
{
    public Guid AccountId { get; set; }

    public decimal Amount { get; set; }
}