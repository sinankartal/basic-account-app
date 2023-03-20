using Common.DTOs;

namespace Common.Responses;

public class GetUserAccountsResponse
{
    public UserDTO User { get; set; }

    public List<AccountDTO> Accounts { get; set; }
    
}