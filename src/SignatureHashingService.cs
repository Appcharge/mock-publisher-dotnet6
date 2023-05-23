// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public interface ISignatureHashingService {
    void SignPayload(string expectedSignature, string data);
}

public class SignatureHashingService : ISignatureHashingService
{
    private readonly byte[] _key;

    public SignatureHashingService(string key)
    {
        _key = Encoding.ASCII.GetBytes(key);
    }

    public void SignPayload(string expectedSignature, string data)
    {
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);

        using var hmac = new HMACSHA256(_key);
        byte[] hmacBytes = hmac.ComputeHash(dataBytes);
        var signature = BitConverter.ToString(hmacBytes).Replace("-", "").ToLower();

        if (!$"Signature {signature}".Equals(expectedSignature))
        {
            throw new Exception("Signatures doesn't match");
        }
    }
}

