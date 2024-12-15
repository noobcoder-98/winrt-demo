using System.Threading.Tasks;

public interface IDataService
{
    Task<bool> SaveData<T>(string path, T data, bool isEncrypted);

    T LoadData<T>(string path, bool isEncrypted);
}