namespace TransactionService;

[Serializable] 
public class GetTransactionsMessage
{
    public List<Guid> AccountIds { get; set; }
}