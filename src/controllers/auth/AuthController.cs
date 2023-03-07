using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{ 
    private readonly IAESDecryptorService _aesDecryptor;

    public AuthController(IAESDecryptorService aesDecryptor) {
        _aesDecryptor = aesDecryptor;
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

        var authResult = new AuthResult(false, "0");

        switch(authRequest.AuthType)
        {
            case "facebook":
                authResult = FacebookAuth.authenticate(authRequest.AppId, authRequest.Token, "0904f2b1d025723b62e58febbe83e155");
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