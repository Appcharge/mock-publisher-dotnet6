using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;


public class PlayerUpdateBalanceResponse {
    public string publisherPurchaseId { get; set; } = default!;
    public DateTime purchaseTime { get; set; } = default!;
}


[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IConfiguration Configuration;

    public PlayerController(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    [HttpPost]
    [Route("/mocker/updateBalance")]
    public async Task<object> PlayerUpdateBalanceEndpoint()
    {
        string body = await new StreamReader(Request.Body).ReadToEndAsync();
        var httpContent = new StringContent(body, Encoding.UTF8, "application/json");
        var signatureHashingService = new SignatureHashingService(Configuration["KEY"]);
        
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("signature", signatureHashingService.CreateSignature(body));
        httpClient.DefaultRequestHeaders.Add("x-publisher-token", Configuration["PUBLISHER_TOKEN"]);
        
        HttpResponseMessage response = await httpClient.PostAsync(
            $"{Configuration["AWARD_PUBLISHER_URL"]}",
            httpContent
        );
        
        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString |
                                 JsonNumberHandling.WriteAsString
            };

            string responseContent = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<object>(responseContent, options);;
        }
        else
        {
            throw new Exception($"Request failed with status code {response.StatusCode}");
        }
        
    }
    
    [HttpPost]
    [Route("/mocker/infoSync")]
    public async Task<object> PlayerInfoSync()
    {   
        var jsonData = System.IO.File.ReadAllText(
            Environment.CurrentDirectory + Configuration["PLAYER_DATASET_FILE_PATH"]
        );
        var playerDataSetObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonData);
        return playerDataSetObject;
    }
}
