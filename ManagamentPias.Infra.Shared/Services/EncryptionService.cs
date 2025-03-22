using ManagementPias.App.Common.Services;
using System.Security.Cryptography;
using System.Text;

namespace ManagementPias.Infra.Shared.Services;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;
    public EncryptionService(string key)
    {
        // Usar SHA256 para generar una clave segura de 32 bytes
        using var sha256 = SHA256.Create();
        _key = sha256.ComputeHash(Encoding.UTF8.GetBytes(key));
        _iv = _key[..16]; // Usar los primeros 16 bytes como IV
    }
    public string Decrypt(string encryptedText)
    {
        if (IsValidBase64(encryptedText))
        {
            var cipherBytes = Convert.FromBase64String(encryptedText);

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var memoryStream = new MemoryStream(cipherBytes);
            using var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }

        return encryptedText;
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            throw new ArgumentNullException(nameof(plainText), "El texto a encriptar no puede estar vacío o nulo.");

        try
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;

            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cryptoStream))
            {
                writer.Write(plainText);
            }

            // Asegúrate de que el MemoryStream contiene datos antes de convertir a Base64
            var encryptedData = memoryStream.ToArray();
            if (encryptedData.Length == 0)
                throw new InvalidOperationException("El proceso de encriptación no generó datos.");

            return Convert.ToBase64String(encryptedData);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error durante la encriptación.", ex);
        }
    }

    private bool IsValidBase64(string input)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        // Intentar convertir desde Base64
        try
        {
            Convert.FromBase64String(input);
            return true;
        }
        catch
        {
            return false;
        }
    }

}
