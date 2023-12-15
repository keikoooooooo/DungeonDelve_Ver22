using System.IO;
using UnityEngine;

public static class FileHandle
{

    public static void Save<T>(T _data, string _fileName)
    {
        var _path = Path.Combine(Application.persistentDataPath, _fileName);
        var jsonText = JsonUtility.ToJson(_data, true);
        File.WriteAllText(_path, jsonText);
    }

    public static void Save<T>(T _data, string _folder, string _fileName)
    {
        var _path = Path.Combine(Application.persistentDataPath, _folder, _fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(_path) ?? string.Empty);

        var jsonText = JsonUtility.ToJson(_data, true);
        File.WriteAllText(_path, jsonText);
    }

    
    public static bool Load<T>(string _fileName, out T _data)
    {
        var _path = Path.Combine(Application.persistentDataPath, _fileName);
        return LoadFromFile(_path,out _data);
    }
    public static bool Load<T>(string _folder, string _fileName, out T _data)
    {
        var _path = Path.Combine(Application.persistentDataPath, _folder, _fileName);
        return LoadFromFile(_path, out _data);
    }
    private static bool LoadFromFile<T>(string _path, out T _data)
    {
        if (!File.Exists(_path))
        {
            _data = default;
            return false;
        }
        _data = JsonUtility.FromJson<T>(File.ReadAllText(_path));
        return true;
    }

    
    public static void Delete(string _fileName)
    {
        var _path = Path.Combine(Application.persistentDataPath, _fileName);
        DeleteFile(_path);
    }
    public static void Delete(string _folder, string _fileName)
    {
        var _path = Path.Combine(Application.persistentDataPath, _folder, _fileName);
        DeleteFile(_path);
    }
    private static void DeleteFile(string _path)
    {
        if (File.Exists(_path))
            File.Delete(_path);
    }
}
