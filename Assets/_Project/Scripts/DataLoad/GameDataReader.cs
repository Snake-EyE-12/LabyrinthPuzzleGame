using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class GameDataReader
{
    private static string ReadText(string path)
    {
        string finalPath = Application.streamingAssetsPath + "/" + path + ".json";
        return File.ReadAllText(finalPath);
    }
    private static T ReadJson<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }

    public static T ConvertToJsonObject<T>(string path)
    {
        return ReadJson<T>(ReadText(path));
    }
    
    public static List<string> GetAllGamemodeNames()
    {
        string relativePath = "LoadData/Gamemodes";
        List<string> folderNames = new List<string>();
        string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, relativePath);
        fullPath = fullPath.Replace('\\', '/');
        
        if (Directory.Exists(fullPath))
        {
            string[] directories = Directory.GetDirectories(fullPath);
            foreach (string directory in directories)
            {
                string folderName = new DirectoryInfo(directory).Name;
                folderNames.Add(folderName);
            }
        }
        else
        {
            Debug.LogWarning("Directory does not exist: " + fullPath);
        }

        return folderNames;
    }
    
    
}

public class FileReader
{
    
}

public class GameDataWriter
{
    
}