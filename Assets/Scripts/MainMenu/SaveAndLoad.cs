using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

namespace EducaNuclear
{
    public class SaveAndLoad : MonoBehaviour
    {
        public static PlayerData playerData;

        private static void Start()
        {
            LoadPlayerData();
        }

        public static void SavePlayerData(PlayerData newPlayerData)
        {
            PlayerPrefs.SetInt("CurrentPhase", newPlayerData.currentPhase);
        }

        public static void LoadPlayerData()
        {
            if (PlayerPrefs.HasKey("CurrentPhase"))
            {
                int currentPhase = PlayerPrefs.GetInt("CurrentPhase");
                playerData = new PlayerData(currentPhase);
            }
        }
    }
}
