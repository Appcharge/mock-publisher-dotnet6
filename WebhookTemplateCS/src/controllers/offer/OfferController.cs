using System.Text.Json.Nodes;
using Microsoft.Extensions.Configuration;

namespace WebhookTemplateCS.controllers.offer;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using JsonSerializer = System.Text.Json.JsonSerializer;

[ApiController]
[Route("[controller]")]
public class OfferController : ControllerBase
{
    private readonly IConfiguration Configuration;

    public OfferController(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    [HttpPost]
    [Route("/mocker/offer")]
    public async Task<object> CreateOfferEndpoint([FromBody] CreateOfferRequest content)
    {
        var jsonData = System.IO.File.ReadAllText(
            Environment.CurrentDirectory + Configuration["OFFERS_FILE_PATH"]
            );
        var offersDataSetObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonData);
        
        string jsonString = JsonSerializer.Serialize(offersDataSetObject["create"]);
        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var signatureHashingService = new SignatureHashingService(Configuration["KEY"]);
        
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("signature", signatureHashingService.CreateSignature(jsonString));
        httpClient.DefaultRequestHeaders.Add("x-publisher-token", Configuration["PUBLISHER_TOKEN"]);
        
        HttpResponseMessage response = await httpClient.PostAsync(
            $"{Configuration["ASSET_UPLOAD_GATEWAY_URL"]}/offering/offer/",
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
            return JsonSerializer.Deserialize<object>(responseContent, options);
        }
        else
        {
            throw new Exception($"Request failed with status code {response.StatusCode}");
        }
    }
    
    [HttpPut]
    [Route("/mocker/offer/{offerId}")]
    public async Task<object> UpdateOfferEndpoint(string offerId)
    {
        var jsonData = System.IO.File.ReadAllText(
            Environment.CurrentDirectory + Configuration["OFFERS_FILE_PATH"]
        );
        var offersDataSetObject = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonData);
        
        string jsonString = JsonSerializer.Serialize(offersDataSetObject["update"]);
        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var signatureHashingService = new SignatureHashingService(Configuration["KEY"]);
        
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("signature", signatureHashingService.CreateSignature(jsonString));
        httpClient.DefaultRequestHeaders.Add("x-publisher-token", Configuration["PUBLISHER_TOKEN"]);
        
        HttpResponseMessage response = await httpClient.PutAsync(
            $"{Configuration["ASSET_UPLOAD_GATEWAY_URL"]}/offering/offer/{offerId}",
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
            return JsonSerializer.Deserialize<object>(responseContent, options);
        }
        else
        {
            throw new Exception($"Request failed with status code {response.StatusCode}");
        }
    }
}
