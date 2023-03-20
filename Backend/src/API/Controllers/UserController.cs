
using Application.IServices;
using Common.DTOs;
using Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("all")]
    public Task<List<UserDTO>> GetAll()
    {
        return _userService.GetAll();
    }
    
    [HttpGet("{userId}/accounts")]
    public Task<GetUserAccountsResponse> GetAccounts(Guid userId)
    {
        return _userService.GetAccountsAsync(userId);
    }
}