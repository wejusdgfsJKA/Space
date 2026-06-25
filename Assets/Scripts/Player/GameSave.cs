using System;
using System.IO;
using UnityEngine;
namespace Player
{
    [Serializable]
    public struct GameSave
    {
        public static GameSave? Load(string saveFilePath)
        {
            if (!File.Exists(saveFilePath))
            {
                Debug.LogError("No save file found!");
                return null;
            }
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<GameSave>(json);
        }
        public readonly void Save(string saveFilePath)
        {
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(saveFilePath, json);
        }
    }
}
