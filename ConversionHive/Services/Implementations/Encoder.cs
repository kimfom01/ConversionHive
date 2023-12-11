using System.Security.Cryptography;
using System.Text;

namespace ConversionHive.Services.Implementations;

public class Encoder : IEncoder
{
    private readonly string _salt;
    private const int Iterations = 1000;
    private byte[] _saltyBytes = [];

    public Encoder(string salt)
    {
        _salt = salt;
    }

    public string EncodeValue(string value, string key)
    {
        GetSalt();

        return Encrypt(GetHashKey(key), value);
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

    private string Encrypt(byte[] key, string dataToEncrypt)
    {
        // Initialize
        var encryptionAlgorithm = Aes.Create();

        if (encryptionAlgorithm is null)
        {
            throw new Exception("Encryption algorithm might not exist");
        }

        // Set the key
        encryptionAlgorithm.Key = key;
        encryptionAlgorithm.IV = key;

        // create a memory stream
        using var encryptionStream = new MemoryStream();

        // Create the crypto stream
        using var encrypt = new CryptoStream(encryptionStream, encryptionAlgorithm.CreateEncryptor(),
            CryptoStreamMode.Write);

        // Encrypt
        byte[] utfD1 = Encoding.UTF8.GetBytes(dataToEncrypt);
        encrypt.Write(utfD1, 0, utfD1.Length);
        encrypt.FlushFinalBlock();
        encrypt.Close();

        // Return the encrypted data
        return Convert.ToBase64String(encryptionStream.ToArray());
    }
}