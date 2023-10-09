using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using WebhookTemplateCS.controllers.order;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.Extensions.Configuration;


[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IConfiguration Configuration;

    public OrderController(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    [HttpPost]
    [Route("/mocker/orders")]
    public async Task<GetOrdersResponse> GetOrdersEndpoint([FromBody] GetOrdersRequest content)
    {
        string jsonString = JsonSerializer.Serialize(content);
        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var signatureHashingService = new SignatureHashingService(Configuration["KEY"]);
        
        Console.WriteLine(Configuration["KEY"], Configuration["PUBLISHER_TOKEN"]);
        
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("signature", signatureHashingService.CreateSignature(jsonString));
        httpClient.DefaultRequestHeaders.Add("x-publisher-token", Configuration["PUBLISHER_TOKEN"]);
        
        HttpResponseMessage response = await httpClient.PostAsync(
            $"{Configuration["REPORTING_API_URL"]}/reporting/reports/orders/",
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
            GetOrdersResponse ordersResponse = JsonSerializer.Deserialize<GetOrdersResponse>(responseContent, options);

            return ordersResponse;
        }
        else
        {
            throw new Exception($"Request failed with status code {response.StatusCode}");
        }
    }
}