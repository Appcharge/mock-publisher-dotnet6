using Newtonsoft.Json;

namespace WebhookTemplateCS.controllers.auth.methods;

public class FacebookAuthService: IFacebookAuthService {
    
    public AuthResult Authenticate(string appId, string token, string appSecret)
    {
        string url = $"https://graph.facebook.com/debug_token?input_token={token}&access_token={appId}|{appSecret}";

        System.Console.WriteLine(url);

        HttpClient client = new HttpClient();
        HttpResponseMessage response = client.GetAsync(url).Result;

        if (response.IsSuccessStatusCode)
        {
            string responseString = response.Content.ReadAsStringAsync().Result;
            dynamic responseObj = JsonConvert.DeserializeObject(responseString);
            bool isValid = responseObj.data.is_valid;
            string userId = responseObj.data.user_id;

            return new AuthResult(isValid, userId);
        }

        return new AuthResult(false, "0");
    }

}