// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public interface IAESDecryptorService {
    string Decrypt(string data);
}

public class AESDecryptorService : IAESDecryptorService
{
    private readonly byte[] _iv;
    private readonly byte[] _key;

    public AESDecryptorService(string iv, string key)
    {
        _iv = Encoding.ASCII.GetBytes(iv);
        _key = Encoding.ASCII.GetBytes(key);
    }

    public string Decrypt(string data)
    {
        byte[] encryptedData = StringToByteArray(data);

        using (Aes aes = Aes.Create())
        {
            aes.IV = _iv;
            aes.Key = _key;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }

    public static byte[] StringToByteArray(string hex) {
        return Enumerable.Range(0, hex.Length)
                            .Where(x => x % 2 == 0)
                            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                            .ToArray();
    }
     
    
}

