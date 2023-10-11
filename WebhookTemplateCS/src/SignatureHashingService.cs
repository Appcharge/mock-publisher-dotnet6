// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Moq;

public interface ISignatureHashingService {
    (string signature, string expectedSignature) SignPayload(string expectedSignature, string data);
}

record Signature
{
    public string T { get; init; }
    public string V1 { get; init; }
}

public class SignatureHashingService : ISignatureHashingService
{
    private readonly byte[] _key;

    public SignatureHashingService(string key)
    {
        _key = Encoding.ASCII.GetBytes(key);
    }

    private Signature ParseSignature(string signatureString)
    {
        var regex = new System.Text.RegularExpressions.Regex(@"t=(.*),v1=(.*)");
        var match = regex.Match(signatureString);

        if (!match.Success || match.Groups.Count < 3)
        {
            throw new ArgumentException("Invalid signature format");
        }

        var t = match.Groups[1].Value;
        var v1 = match.Groups[2].Value;

        return new Signature { T = t, V1 = v1 };
    }

    public (string signature, string expectedSignature) SignPayload(string expectedSignatureHeader, string data)
    {
        var expectedSignature = ParseSignature(expectedSignatureHeader);
        byte[] dataBytes = Encoding.UTF8.GetBytes($"{expectedSignature.T}.{data}");

        using var hmac = new HMACSHA256(_key);
        byte[] hmacBytes = hmac.ComputeHash(dataBytes);
        var signature = BitConverter.ToString(hmacBytes).Replace("-", "").ToLower();
        return (signature, expectedSignature.V1);
    }
    
    public string CreateSignature(object requestBody)
    {
        var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        byte[] dataBytes = Encoding.UTF8.GetBytes($"{timeStamp}.{requestBody}");

        using var hmac = new HMACSHA256(_key);
        byte[] hmacBytes = hmac.ComputeHash(dataBytes);
        var signature = BitConverter.ToString(hmacBytes).Replace("-", "").ToLower();
        return $"t={timeStamp},v1={signature}";
    }
}

