using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

public class HealthResponse {
    public string message { get; set; } = default!;
}

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [Route("/mocker//health")]
    public async Task<HealthResponse> HealthEndpoint()
    {
        return new HealthResponse {
            message = "OK"
        };
        
    }
}