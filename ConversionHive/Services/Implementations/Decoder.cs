using System.Security.Cryptography;
using System.Text;

namespace ConversionHive.Services.Implementations;

public class Decoder : IDecoder
{
    private readonly string _salt;
    private const int Iterations = 1000;
    private byte[] _saltyBytes = [];

    public Decoder(string salt)
    {
        _salt = salt;
    }

    public string DecodeValue(string encodedValue, string key)
    {
        GetSalt();
        
        return Decrypt(GetHashKey(key), encodedValue);
    }

    private byte[] GetHashKey(string hashKey)
    {
        // Setup the hasher
        var rfc = new Rfc2898DeriveBytes(hashKey, _saltyBytes, Iterations, HashAlgorithmName.SHA256);

        // Return the key
        return rfc.GetBytes(16);
    }
    
    private void GetSalt()
    {
        var encoder = new UTF8Encoding();
        _saltyBytes = encoder.GetBytes(_salt);
    }

    private string Decrypt(byte[] key, string encryptedString)
    {
        // Initialize
        var decryptionAlgorithm = Aes.Create();

        if (decryptionAlgorithm is null)
        {
            throw new Exception("Decryption algorithm might not exist");
        }

        byte[] encryptedData = Convert.FromBase64String(encryptedString);

        // Set the key
        decryptionAlgorithm.Key = key;
        decryptionAlgorithm.IV = key;

        // create a memory stream
        using var decryptionStream = new MemoryStream();

        // Create the crypto stream
        using var decrypt = new CryptoStream(decryptionStream, decryptionAlgorithm.CreateDecryptor(),
            CryptoStreamMode.Write);

        // Encrypt
        decrypt.Write(encryptedData, 0, encryptedData.Length);
        decrypt.Flush();
        decrypt.Close();

        // Return the unencrypted data
        byte[] decryptedData = decryptionStream.ToArray();

        return Encoding.UTF8.GetString(decryptedData, 0, decryptedData.Length);
    }
}