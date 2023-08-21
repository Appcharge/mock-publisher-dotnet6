namespace WebhookTemplateCS.controllers.auth.methods;

public interface IFacebookAuthService
{
    AuthResult Authenticate(string appId, string token, string appSecret);
}