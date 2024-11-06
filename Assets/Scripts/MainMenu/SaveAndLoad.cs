using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace EducaNuclear
{
    public class SaveAndLoad : MonoBehaviour
    {
        private static string path = "Assets/JSON/playerData.json";

        public static PlayerData playerData;

        private static void Start()
        {
            LoadPlayerData();
        }

        public static void SavePlayerData(PlayerData newPlayerData)
        {
            string json = JsonConvert.SerializeObject(newPlayerData, Formatting.Indented);
            File.WriteAllText(path, json);
        }

        public static void LoadPlayerData()
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                playerData = JsonConvert.DeserializeObject<PlayerData>(json);
            }
        }
    }
}
