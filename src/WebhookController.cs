using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


public class PlayerUpdateBalanceResponse {
    public string publisherPurchaseId { get; set; } = default!;
}

[ApiController]
[Route("[controller]")]
public class WebhookController : ControllerBase
{ 
    private readonly IAESDecryptorService _aesDecryptor;

    public WebhookController(IAESDecryptorService aesDecryptor) {
        _aesDecryptor = aesDecryptor;
    }

    [HttpPost]
    [Route("/updateBalance")]
    public async Task<PlayerUpdateBalanceResponse> PlayerUpdateBalanceEndpoint()
    {
        using (var reader = new StreamReader(Request.Body))
        {
            var body = await reader.ReadToEndAsync();
            var decrypted = _aesDecryptor.Decrypt(body);
            var publisherPayload = JsonSerializer.Deserialize<PublisherPayload>(decrypted);

            if(publisherPayload == null) {
                throw new Exception("could not parse publisher payload");
            }

            Console.WriteLine(publisherPayload);

            // TODO
            // Here goes your piece of code that is responsible for handling player update balance requests coming from appcharge systems
            
            return new PlayerUpdateBalanceResponse {
                // TODO change the <PURCHASE-ID> with a real purchase ID
                publisherPurchaseId = "<PURCHASE-ID>"
            };
        }
        
    }
}