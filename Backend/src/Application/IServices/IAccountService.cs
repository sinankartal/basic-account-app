using Common.Requests;
using Common.Responses;

namespace Application.IServices;

public interface IAccountService
{
    Task<CreateAccountResponse> CreateAsync(CreateAccountRequest request);

    Task AddTransaction(AddTransactionRequest request);

}