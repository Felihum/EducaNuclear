using Corrida;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Corrida
{
    public static class DataHandler
    {
        private static readonly string _dataPath = $"{Application.persistentDataPath}/Corrida contra o tempo/Data/";
        private static readonly string _firstBootFileName = $"FirstBoot.json";
        private static readonly string _firstBootFileContent = $"This file exists.";
        private static readonly string _runnerSaveDataFileName = $"RunnerSaveData.json";

        public static event Action LevelDataNotFound;

        public static bool CheckIfSaveExists()
        {
            bool exists = !File.Exists(_dataPath + _runnerSaveDataFileName);

            return exists;
        }

        public static void CreateFirstBootFile()
        {
            if (CheckIfSaveExists())
            {
                File.WriteAllText(_dataPath + _firstBootFileName, _firstBootFileContent);
            }
        }

        public static void UpdateSavedLevelData(LevelData levelData)
        {
            if (levelData == null)
                return;

            SaveData saveData = GetRunnerSaveData();
            AllLevelData allLevelData = new(saveData);

            LevelData savedData = GetSingleLevelData(levelData.ID, allLevelData);
            if (savedData.ID != levelData.ID)
                return;
            savedData.UpdateRecord(levelData.CurrentRecord);

            LevelData nextLevelData;
            nextLevelData = SetLevelToUnlock(levelData.nextLevelID, allLevelData);

            FindLevelByID(levelData.ID, allLevelData, (data, index) => allLevelData.data[index] = savedData);
            FindLevelByID(nextLevelData.ID, allLevelData, (data, index) => allLevelData.data[index] = savedData);

            saveData = new(allLevelData);
            SaveRunnerData(saveData);
        }

        public static SaveData GetRunnerSaveData()
        {
            try
            {
                string jsonData = File.ReadAllText(_dataPath + _runnerSaveDataFileName);
                SaveData saveData = JsonUtility.FromJson<SaveData>(jsonData);

                return saveData;
            }
            catch(Exception ex)
            {
                Debug.LogException(ex);
                LevelDataNotFound?.Invoke();
                return new();
            }
        }

        public static void SaveRunnerData(SaveData saveData)
        {
            if (saveData.FactorySaveData == null || saveData.FarmSaveData == null || saveData.ForestSaveData == null)
                return;

            _ = CheckDataPathDirectory();

            string dataJson = JsonUtility.ToJson(saveData);
            File.WriteAllText(_dataPath + _runnerSaveDataFileName, dataJson);
        }

        private static bool CheckDataPathDirectory()
        {
            bool pathFound;

            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
                pathFound = false;
            }
            else
                pathFound = true;

            return pathFound;
        }

        private static LevelData GetSingleLevelData(LevelID levelID, AllLevelData allLevelData)
        {
            LevelData savedLevelData = null;

            FindLevelByID(levelID, allLevelData, (foundData, dummy) => savedLevelData = foundData);

            return savedLevelData;
        }
        private static LevelData GetSingleLevelData(LevelID levelID) // Caso AllLevelData não seja fornecido
        {
            AllLevelData allLevelData = new(GetRunnerSaveData());

            return GetSingleLevelData(levelID, allLevelData);
        }

        private static void FindLevelByID(LevelID levelID, AllLevelData allLevelData, Action<LevelData, int> OnLevelFound)
        {
            for (int i = 0; i < allLevelData.data.Count; i++)
            {
                if (allLevelData.data[i].ID == levelID)
                {
                    OnLevelFound.Invoke(allLevelData.data[i], i);
                    break;
                }
            }
        }

        private static LevelData SetLevelToUnlock(LevelID levelID, AllLevelData allLevelData)
        {
            LevelData levelData = GetSingleLevelData(levelID, allLevelData);
            
            if (!levelData.IsUnlocked)
            {
                levelData.ShouldUnlock = true;
                return levelData;
            }

            return levelData;
        }
        private static LevelData SetLevelToUnlock(LevelID levelID) // Caso AllLevelData não seja fornecido
        {
            AllLevelData allLevelData = new(GetRunnerSaveData());

            return SetLevelToUnlock(levelID, allLevelData);
        }
    }
}
