public interface ISecretsService {
    string getFacebookSecret();
}


public class SecretsService : ISecretsService {

    private string _facebookAppSecret;

    public SecretsService(string facebookAppSecret) {
        _facebookAppSecret = facebookAppSecret;
    }

    public string getFacebookSecret()
    {
        return _facebookAppSecret;
    }
}