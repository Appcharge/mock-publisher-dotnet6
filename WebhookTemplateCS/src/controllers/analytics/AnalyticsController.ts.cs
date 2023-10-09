using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using WebhookTemplateCS.controllers.analytics;
using JsonSerializer = System.Text.Json.JsonSerializer;


[ApiController]
[Route("[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IConfiguration Configuration;

    public AnalyticsController(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    [HttpPost]
    [Route("/mocker/analytics")]
    public async Task<object> GetAnalyticsEndpoint([FromBody] GetAnalyticsRequest content)
    {
        string jsonString = JsonSerializer.Serialize(content);
        var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        var signatureHashingService = new SignatureHashingService(Configuration["KEY"]);
        
        HttpClient httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("signature", signatureHashingService.CreateSignature(jsonString));
        httpClient.DefaultRequestHeaders.Add("x-publisher-token", Configuration["PUBLISHER_TOKEN"]);
        
        HttpResponseMessage response = await httpClient.PostAsync(
            $"{Configuration["REPORTING_API_URL"]}/reporting/reports/analytics/",
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