using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Application.IServices;
using Common.Requests;
using Common.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class AccountController : ControllerBase
{
    private IAccountService _accountService;
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpPost("create")]
    public Task<CreateAccountResponse> Create(CreateAccountRequest request)
    {
        return _accountService.CreateAsync(request);
    }
    
    [HttpPost("add-transaction")]
    public Task AddNewTransaction(AddTransactionRequest request)
    {
        return _accountService.AddTransaction(request);
    }
}