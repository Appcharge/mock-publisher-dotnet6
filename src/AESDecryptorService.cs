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
        // Convert the encrypted data into byte array
        try {
            byte[] encryptedData = StringToByteArray(data);

            using (Aes aes = Aes.Create())
            {
                aes.IV = _iv;
                aes.Key = _key;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

                // Convert the decrypted result into a UTF8 string
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            
        } catch(Exception ex) {
            throw new Exception("Failed to decrypt data", ex);
        }        
    }

    // Converts a hex string ('3f2ba56b') into byte array ([0x3f, 0x2b, 0xa5, 0x6b])
    public static byte[] StringToByteArray(string hex) {
        return Enumerable.Range(0, hex.Length)
                            .Where(x => x % 2 == 0)
                            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                            .ToArray();
    }
     
    
}

