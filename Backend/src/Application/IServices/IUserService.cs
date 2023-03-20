using Common.DTOs;
using Common.Responses;
using Persistence.Models;

namespace Application.IServices;

public interface IUserService
{
    Task<List<UserDTO>> GetAll();

    Task<GetUserAccountsResponse> GetAccountsAsync(Guid userId);
}