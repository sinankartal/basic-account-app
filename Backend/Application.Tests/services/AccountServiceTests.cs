using System;
using System.Threading.Tasks;
using Application.IServices;
using Application.Services;
using Common.Helpers;
using Common.Messages;
using Common.Requests;
using Common.Responses;
using Infrastructure.Azure;
using Moq;
using Persistence.IRepositories;
using Persistence.Models;
using Xunit;

namespace Application.UnitTests.Services;

    public class AccountServiceTests
    {
        private readonly Mock<IAccountRepository> _mockAccountRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IServiceBus> _mockServiceBus;
        private readonly IAccountService _accountService;

        public AccountServiceTests()
        {
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockServiceBus = new Mock<IServiceBus>();

            _accountService = new AccountService(_mockAccountRepository.Object, _mockUserRepository.Object, _mockServiceBus.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenUserExists_ShouldReturnCreateAccountResponseWithNewAccountId()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            decimal initialAmount = 1000;

            CreateAccountRequest request = new()
            {
                UserID = userId,
                InitialAmount = initialAmount
            };

            _mockUserRepository.Setup(x => x.IsExist(userId)).ReturnsAsync(true);

            Account accountToAdd = new();
            _mockAccountRepository.Setup(x => x.AddAsync(It.IsAny<Account>()))
                .Callback<Account>((account) =>
                {
                    accountToAdd = account;
                }).ReturnsAsync(Guid.NewGuid());

            // Act
            CreateAccountResponse response = await _accountService.CreateAsync(request);

            // Assert
            Assert.NotNull(accountToAdd);
            Assert.NotEqual(Guid.Empty, accountToAdd.Id);
            Assert.Equal(userId, accountToAdd.UserId);

            _mockAccountRepository.Verify(x => x.AddAsync(It.IsAny<Account>()), Times.Once);
            _mockAccountRepository.Verify(x => x.SaveAsync(), Times.Once);
            _mockServiceBus.Verify(x => x.SendMessageAsync("AddInitialAmount", It.IsAny<AddInitialAmountMessage>(), null), Times.Once);

            Assert.NotNull(response);
            Assert.Equal(accountToAdd.Id, response.Id);
        }

        [Fact]
        public async Task CreateAsync_WhenUserDoesNotExist_ShouldThrowAppExceptionWithNotFoundStatusCode()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            decimal initialAmount = 1000;

            CreateAccountRequest request = new()
            {
                UserID = userId,
                InitialAmount = initialAmount
            };

            _mockUserRepository.Setup(x => x.IsExist(userId)).ReturnsAsync(false);

            // Act
            async Task act() => await _accountService.CreateAsync(request);

            // Assert
            await Assert.ThrowsAsync<AppException>(act);
            _mockAccountRepository.Verify(x => x.AddAsync(It.IsAny<Account>()), Times.Never);
            _mockAccountRepository.Verify(x => x.SaveAsync(), Times.Never);
            _mockServiceBus.Verify(x => x.SendMessageAsync("AddInitialAmount", It.IsAny<AddInitialAmountMessage>(), null), Times.Never);
        }
    }
