using System.Net;
using Application.IServices;
using Common.Helpers;
using Common.Messages;
using Common.Requests;
using Common.Responses;
using Infrastructure.Azure;
using Persistence.IRepositories;
using Persistence.Models;

namespace Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUserRepository _userRepository;
    private readonly IServiceBus _serviceBus;

    public AccountService(IAccountRepository accountRepository, IUserRepository userRepository,
        IServiceBus serviceBus)
    {
        _accountRepository = accountRepository;
        _userRepository = userRepository;
        _serviceBus = serviceBus;
    }

    public async Task<CreateAccountResponse> CreateAsync(CreateAccountRequest request)
    {
        await CheckUserExistence(request.UserID);

        Account newAccount = new();
        var accountNumber = GenerateAccountNumber();

        newAccount.AccountNumber = accountNumber;
        newAccount.UserId = request.UserID;

        newAccount.Id = await _accountRepository.AddAsync(newAccount);
        await _accountRepository.SaveAsync();

        CreateAccountResponse response = new()
        {
            Id = newAccount.Id
        };

        if (request.InitialAmount != 0)
        {
            await _serviceBus.SendMessageAsync("AddInitialAmount", new AddInitialAmountMessage()
            {
                AccountId = newAccount.Id, Amount = request.InitialAmount
            }, null);
        }

        return response;
    }

    public async Task AddTransaction(AddTransactionRequest request)
    {
        await _serviceBus.SendMessageAsync("AddInitialAmount", new AddInitialAmountMessage()
        {
            AccountId = request.AccountId, Amount = request.Amount
        }, null);
    }

    private async Task CheckUserExistence(Guid userId)
    {
        bool userExists = await _userRepository.IsExist(userId);
        if (!userExists)
        {
            throw new AppException($"User cannot be found with {userId}", HttpStatusCode.NotFound);
        }
    }

    private string GenerateAccountNumber()
    {
        Random random = new Random();
        int accountNumber = random.Next(100000000, 999999999);
        return accountNumber.ToString();
    }
}