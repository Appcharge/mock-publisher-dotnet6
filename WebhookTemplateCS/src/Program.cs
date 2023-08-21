using System;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using WebhookTemplateCS.controllers.auth.methods;

var builder = WebApplication.CreateBuilder(args);

var key = Environment.GetEnvironmentVariable("KEY");
var facebookAppSecret = Environment.GetEnvironmentVariable("FACEBOOK_APP_SECRET");

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
builder.Services.AddScoped<ISignatureHashingService, SignatureHashingService>(provider => new SignatureHashingService(key));
builder.Services.AddScoped<ISecretsService, SecretsService>(provider => new SecretsService(facebookAppSecret));
builder.Services.AddScoped<IFacebookAuthService, FacebookAuthService>(provider => new FacebookAuthService());

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<DecryptionMiddleware>();


app.MapControllers();

app.Run();
