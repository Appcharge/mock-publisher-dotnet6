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

    public async Task Invoke(HttpContext context, ISignatureHashingService signatureHashingService)
    {
        try
        {
            var originalBody = context.Request.Body;
            using var reader = new StreamReader(originalBody);
            var requestBody = await reader.ReadToEndAsync();

            var serializedJson = JsonSerializer.Serialize(JsonSerializer.Deserialize<object>(requestBody));
            var (signature, expectedSignature) = signatureHashingService.SignPayload(context.Request.Headers["signature"], serializedJson);
            if (!signature.Equals(expectedSignature))
            {
                throw new Exception("Signatures doesn't match");
            }
            var requestBodyBytes = Encoding.UTF8.GetBytes(requestBody);
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
