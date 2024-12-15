using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class DataService : IDataService
{
    private const string KEY = "ggdPhkeOoiv6YMiPWa34kIuOdDUL7NwQFg6l1DVdwN8=";
    private const string IV = "JZuM0HQsWSBVpRHTeRZMYQ==";

    public async Task<bool> SaveData<T>(string path, T data, bool isEncrypted)
    {
        try
        {
            using FileStream stream = File.Create(path);
            if (isEncrypted)
            {
                WriteEncryptedData(data, stream);
            }
            else
            {
                using StreamWriter writer = new StreamWriter(stream);
                await writer.WriteAsync(JsonConvert.SerializeObject(data));
            }

            Console.WriteLine($"Data saved successfully to {path}");
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"SaveData#Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    private void WriteEncryptedData<T>(T data, FileStream stream)
    {
        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);
        using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
        using CryptoStream cryptoStream = new CryptoStream(stream, cryptoTransform, CryptoStreamMode.Write);
        byte[] dataBytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data));
        cryptoStream.Write(dataBytes, 0, dataBytes.Length);
    }

    public T LoadData<T>(string path, bool isEncrypted)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine($"LoadData#File not found at path: {path}");
            return default;
        }

        try
        {
            return isEncrypted ? ReadEncryptedData<T>(path) : JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
        catch (Exception e)
        {
            Console.WriteLine($"LoadData#Failed to load data due to: {e.Message} {e.StackTrace}");
            return default;
        }
    }

    private T ReadEncryptedData<T>(string path)
    {
        byte[] fileBytes = File.ReadAllBytes(path);

        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);
        using CryptoStream cryptoStream = new CryptoStream(new MemoryStream(fileBytes), aesProvider.CreateDecryptor(), CryptoStreamMode.Read);
        using StreamReader reader = new StreamReader(cryptoStream);
        string result = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(result);
    }
}
