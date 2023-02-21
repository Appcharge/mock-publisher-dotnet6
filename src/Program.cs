
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

var iv = Environment.GetEnvironmentVariable("IV");
var key = Environment.GetEnvironmentVariable("KEY");

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

builder.Services.AddControllers();
builder.Services.AddScoped<IAESDecryptorService, AESDecryptorService>(provider => {
    return new AESDecryptorService(iv, key);
});

var app = builder.Build();

app.MapControllers();

app.Run();
