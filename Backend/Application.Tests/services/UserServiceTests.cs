using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Application.IServices;
using Application.Services;
using AutoMapper;
using Common.DTOs;
using Common.Helpers;
using Common.Responses;
using Infrastructure.Azure;
using Moq;
using Newtonsoft.Json;
using Persistence.IRepositories;
using Persistence.Models;
using Xunit;

namespace Application_Tests.services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IAccountRepository> _accountRepositoryMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<IServiceBus> _serviceBusMock = new();
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _userService = new UserService(_userRepositoryMock.Object, _accountRepositoryMock.Object, _mapperMock.Object,
            _serviceBusMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Name = "John", Surname = "Doe" },
            new User { Id = Guid.NewGuid(), Name = "Jane", Surname = "Doe" }
        };
        var expected = new List<UserDTO>
        {
            new UserDTO { Id = users[0].Id, Name = users[0].Name, Surname = users[0].Surname },
            new UserDTO { Id = users[1].Id, Name = users[1].Name, Surname = users[1].Surname }
        };
        _userRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(users);
        _mapperMock.Setup(x => x.Map<List<UserDTO>>(users)).Returns(expected);

        // Act
        var result = await _userService.GetAll();

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public async Task GetAccountsAsync_WhenUserNotFound_ShouldThrowAppException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userRepositoryMock.Setup(x => x.IsExist(userId)).ReturnsAsync(false);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<AppException>(async () => await _userService.GetAccountsAsync(userId));
        Assert.Equal((int)HttpStatusCode.NotFound, exception.StatusCode);
        Assert.Equal($"User cannot be found with {userId}", exception.Message);
    }
    
          [Fact]
        public async Task GetAccountsAsync_WhenUserFound_ShouldReturnAccounts()
        {
            // Arrange
            Guid userId = Guid.NewGuid();

            User user = new User { Id = userId };
            UserDTO userDto = new UserDTO { Id = userId };
            
            _userRepositoryMock.Setup(repo => repo.IsExist(userId)).ReturnsAsync(true);
            _userRepositoryMock.Setup(repo => repo.FindAsync(userId)).ReturnsAsync(user);
            _mapperMock.Setup(mapper => mapper.Map<UserDTO>(user)).Returns(userDto);

            List<Account> userAccounts = new List<Account>
            {
                new Account { Id = Guid.NewGuid(), UserId = userId, AccountNumber = "123456" },
                new Account { Id = Guid.NewGuid(), UserId = userId, AccountNumber = "654321" },
            };
            _accountRepositoryMock.Setup(repo => repo.GetUserAccounts(userId)).ReturnsAsync(userAccounts);

            List<TransactionDTO> transactions = new List<TransactionDTO>
            {
                new TransactionDTO { AccountId = userAccounts[0].Id, Amount = 100.0m },
                new TransactionDTO { AccountId = userAccounts[0].Id, Amount = 50.0m },
                new TransactionDTO { AccountId = userAccounts[1].Id, Amount = -25.0m },
            };
            string transactionsString = JsonConvert.SerializeObject(transactions);
            _serviceBusMock.Setup(bus => bus.ReceiveMessageAsync("TransactionsEmit", It.IsAny<string>())).ReturnsAsync(transactionsString);

            // Act
            GetUserAccountsResponse result = await _userService.GetAccountsAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDto.Id, result.User.Id);
            Assert.Equal(userAccounts.Count, result.Accounts.Count);

            foreach (var account in result.Accounts)
            {
                Account matchingUserAccount = userAccounts.FirstOrDefault(a => a.Id == account.Id);
                Assert.NotNull(matchingUserAccount);

                decimal expectedBalance = transactions.Where(t => t.AccountId == account.Id).Sum(t => t.Amount);
                Assert.Equal(expectedBalance, account.Balance);
                Assert.Equal(matchingUserAccount.AccountNumber, account.AccountNumber);
            }
        }

}