using System.Net;
using System.Text.Json;
using System.Text;

public class DecryptionMiddleware
{
    private readonly RequestDelegate _next;
    
    public DecryptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IAESDecryptorService aesDecryptor)
    {
        try
        {
            var originalBody = context.Request.Body;
            using var reader = new StreamReader(originalBody);
            var requestBody = await reader.ReadToEndAsync();
            var decrypted = aesDecryptor.Decrypt(requestBody);
            var requestBodyBytes = Encoding.UTF8.GetBytes(decrypted);
            context.Request.Body = new MemoryStream(requestBodyBytes);

            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            context.Response.StatusCode = 400;
        }
    }
}
