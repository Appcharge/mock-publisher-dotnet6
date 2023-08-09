using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


public class PlayerUpdateBalanceResponse {
    public string publisherPurchaseId { get; set; } = default!;
    public DateTime purchaseTime { get; set; } = default!;
}


[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    [HttpPost]
    [Route("/updateBalance")]
    public async Task<PlayerUpdateBalanceResponse> PlayerUpdateBalanceEndpoint()
    {
        string body = await new StreamReader(Request.Body).ReadToEndAsync();
        var publisherPayload = JsonSerializer.Deserialize<PublisherPayload>(body);

        if(publisherPayload == null) {
            throw new Exception("could not parse publisher payload");
        }

        Console.WriteLine(body);

        // TODO
        // Here goes your piece of code that is responsible for handling player update balance requests coming from appcharge systems
        
        return new PlayerUpdateBalanceResponse {
            purchaseTime = DateTime.Now,
            // TODO change the <PURCHASE-ID> with a real purchase ID
            publisherPurchaseId = "<PURCHASE-ID>"
        };
        
    }
}