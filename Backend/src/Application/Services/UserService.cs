using System.Net;
using Application.IServices;
using AutoMapper;
using Common.DTOs;
using Common.Helpers;
using Common.Responses;
using Infrastructure.Azure;
using Newtonsoft.Json;
using Persistence.IRepositories;
using Persistence.Models;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IMapper _mapper;
    private readonly IServiceBus _serviceBus;

    public UserService(IUserRepository userRepository, IAccountRepository accountRepository, IMapper mapper,
        IServiceBus serviceBus)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _serviceBus = serviceBus;
        _accountRepository = accountRepository;
    }

    public async Task<List<UserDTO>> GetAll()
    {
        List<User> users = await _userRepository.GetAll();

        return _mapper.Map<List<UserDTO>>(users);
    }

    public async Task<GetUserAccountsResponse> GetAccountsAsync(Guid userId)
    {
        await CheckUserExistence(userId);

        GetUserAccountsResponse response = new();
        User user = await _userRepository.FindAsync(userId);
        UserDTO userDto = _mapper.Map<UserDTO>(user);
        response.User = userDto;

        List<Account> userAccounts = await _accountRepository.GetUserAccounts(userId);

        response.Accounts = new();

        string correlationId = Guid.NewGuid().ToString();
        await _serviceBus.SendMessageAsync("GetTransactions", userAccounts.Select(u => u.Id), correlationId);

        string transactionsString = await _serviceBus.ReceiveMessageAsync("TransactionsEmit", correlationId);

        List<TransactionDTO> transactionDtos = JsonConvert.DeserializeObject<List<TransactionDTO>>(transactionsString);

        foreach (var userAccount in userAccounts)
        {
            AccountDTO accountDto = new();
            accountDto.Id = userAccount.Id;
            accountDto.AccountNumber = userAccount.AccountNumber;
            accountDto.Transactions = transactionDtos.Where(t => t.AccountId.Equals(userAccount.Id)).ToList();
            accountDto.Balance = GetAccountBalance(accountDto.Transactions);

            response.Accounts.Add(accountDto);
        }

        return response;
    }

    private decimal GetAccountBalance(List<TransactionDTO> transactionDtos)
    {
        decimal balance = 0;

        if (transactionDtos != null && transactionDtos.Count > 0)
        {
            foreach (var transaction in transactionDtos)
            {
                balance += transaction.Amount;
            }
        }

        return balance;
    }

    private async Task CheckUserExistence(Guid userId)
    {
        bool userExists = await _userRepository.IsExist(userId);
        if (!userExists)
        {
            throw new AppException($"User cannot be found with {userId}", HttpStatusCode.NotFound);
        }
    }
}