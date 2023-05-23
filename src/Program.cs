
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var iv = Environment.GetEnvironmentVariable("IV");
var key = Environment.GetEnvironmentVariable("KEY");
var facebookAppSecret = Environment.GetEnvironmentVariable("FACEBOOK_APP_SECRET");

if(iv == null) {
    Environment.ExitCode = -1;
    Console.WriteLine("Missing `IV` environment variable");
    return;
} 

if(key == null) {
    Environment.ExitCode = -1;
    Console.WriteLine("Missing `KEY` environment variable");
    return;
}

if(facebookAppSecret == null) {
    Environment.ExitCode = -1;
    Console.WriteLine("Missing `FACEBOOK_APP_SECRET` environment variable");
    return;
}

builder.Services.AddControllers();
builder.Services.AddScoped<IAESDecryptorService, AESDecryptorService>(provider => {
    return new AESDecryptorService(iv, key);
});
builder.Services.AddScoped<ISignatureHashingService, SignatureHashingService>(provider => {
    return new SignatureHashingService(key);
});
builder.Services.AddScoped<ISecretsService, SecretsService>(provider => {
    return new SecretsService(facebookAppSecret);
});

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<DecryptionMiddleware>();


app.MapControllers();

app.Run();
