namespace Common.DTOs;

public class TransactionDTO
{
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }

    public DateTime Date { get; set; }
}