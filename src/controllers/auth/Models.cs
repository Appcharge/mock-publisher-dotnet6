public class AuthResult 
{
    public Boolean IsValid { get; }
    public string UserId { get; }
    
    public AuthResult(Boolean IsValid, string UserId) {
        this.IsValid = IsValid;
        this.UserId = UserId;
    }
}

public class AuthenticationRequest
{
    public string AuthMethod { get; set; } = default!;
    public string AuthType { get; set; } = default!;
    public string Token { get; set; } = default!;
    public DateTime Date { get; set; } = default!;
    public string AppId { get; set; } = default!;
    public string PublisherToken { get; set; } = default!;
}


public class ItemBalance {
    public string Currency {get; set;} = default!;
    public int Balance {get; set;} = default!;

    public ItemBalance(string currency, int balance) {
        this.Currency = currency;
        this.Balance = balance;
    }
}

public class AuthResponse
{
    public string Status { get; set; } = default!;
    public string PlayerProfileImage { get; set; } = default!;
    public string PublisherPlayerId { get; set; } = default!;
    public string PlayerName { get; set; } = default!;
    public List<string> Segments { get; set; } = default!;
    public List<ItemBalance> Balances { get; set; } = default!;

    public AuthResponse(string status, string playerProfileImage, string publisherPlayerId, string playerName, List<string> segments, List<ItemBalance> balances)
    {
        Status = status;
        PlayerProfileImage = playerProfileImage;
        PublisherPlayerId = publisherPlayerId;
        PlayerName = playerName;
        Segments = segments;
        Balances = balances;
    }
}