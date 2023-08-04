using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{ 
    private readonly ISecretsService _secretsService;

    public AuthController(ISecretsService secretsService) {
        _secretsService = secretsService;
    }

    [HttpPost]
    [Route("/auth")]
    public async Task<ActionResult<AuthResponse>> PlayerAuth()
    {
        // Extract the request body as plain text
        string body = await new StreamReader(Request.Body).ReadToEndAsync();
        var authRequest = JsonSerializer.Deserialize<AuthenticationRequest>(body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if(authRequest is null) {
            Console.WriteLine($"Invalid request {body}");
            return BadRequest("login failed");
        }

        var authResult = new AuthResult(false, "0");

        var authMethod = authRequest?.AuthMethod;
        switch(authMethod)
        {
            case "facebook":
                authResult = FacebookAuth.authenticate(authRequest.AppId, authRequest.Token, _secretsService.getFacebookSecret());
                break;
            default:
                Console.WriteLine($"unknown authentication method {authMethod}");
                break;
        }
        
        if(authResult.IsValid) {
            System.Console.WriteLine("Successful login");

            Response.ContentType = "application/json";
            return new AuthResponse("valid", "<player profile image>", "<player id>", "<player name>", new List <string>{"seg1", "seg2"}, new List<ItemBalance>{
                new ItemBalance("diamonds", 15)
            });
        } 
        System.Console.WriteLine("failed login");
        Response.ContentType = "text/plain";
        return BadRequest("login failed");
    }
}