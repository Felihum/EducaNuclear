using Corrida;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Corrida
{
    // ScriptableObject de referência dos dados das fases
    [CreateAssetMenu(menuName = "ScriptableObjects/Level Data")]
    [Serializable]
    public class LevelData : ScriptableObject
    {
        public string SceneName;
        public LevelID ID;
        public LevelID nextLevelID;
        public bool IsUnlocked;
        public bool ShouldUnlock;
        [Min(0)] public float DefaultRecord;
        [Min(0)] public float CurrentRecord;
        [Min(0)] public float PreviousRecord;

        public static event Action RecordUpdated;

        public LevelData(LevelSaveData saveData)
        {
            SceneName = saveData.SceneName;
            ID = saveData.ID;
            nextLevelID = saveData.nextLevelID;
            IsUnlocked = saveData.IsUnlocked;
            ShouldUnlock = saveData.ShouldUnlock;
            DefaultRecord = saveData.DefaultRecord;
            CurrentRecord = saveData.CurrentRecord;
            PreviousRecord = saveData.PreviousRecord;
        }

        public void UpdateRecord(float newRecord)
        {
            if (newRecord < 0f)
                newRecord = 0f;
            
            if (newRecord < CurrentRecord)
            {
                PreviousRecord = CurrentRecord;
                CurrentRecord = newRecord;
            }

            RecordUpdated?.Invoke();
        }
    }

    [Serializable]
    public enum LevelID
    {
        Factory,
        Farm,
        Forest,
        None
    }

    // Grupo de LevelDatas
    // Não pode ser salvo, pois ScriptableObjects não são serializados corretamente
    [Serializable]
    public class AllLevelData
    {
        public List<LevelData> data;

        private List<LevelData> SortLevelData(List<LevelData> data)
        {
            List<LevelData> sortedData = new();

            for (int i = 0; i < data.Count; i++)
            {
                if ((int)data[i].ID == i)
                {
                    sortedData.Add(data[i]);
                }
                else
                {
                    for (int j = 0; j < data.Count; j++)
                    {
                        if ((int)data[j].ID == i)
                        {
                            sortedData.Add(data[j]);
                            break;
                        }
                    }
                }
            }

            if (sortedData.Count == data.Count)
                return sortedData;
            else
                return data;
        }

        public AllLevelData(List<LevelData> data)
        {
            this.data = SortLevelData(data);
        }

        public AllLevelData(SaveData saveData)
        {
            List<LevelData> data = new()
            {
                new(saveData.FactorySaveData),
                new(saveData.FarmSaveData),
                new(saveData.ForestSaveData)
            };


            this.data = SortLevelData(data);
        }

        public LevelData GetLevelDataByID(LevelID id)
        {
            LevelData levelData = data[(int)id];
            return levelData;
        }
    }

    // Dados de LevelData que podem ser salvos
    [Serializable]
    public class LevelSaveData
    {
        public string SceneName;
        public LevelID ID;
        public LevelID nextLevelID;
        public bool IsUnlocked;
        public bool ShouldUnlock;
        public float DefaultRecord;
        public float CurrentRecord;
        public float PreviousRecord;

        public LevelSaveData(LevelData data)
        {
            SceneName = data.SceneName;
            ID = data.ID;
            nextLevelID = data.nextLevelID;
            IsUnlocked = data.IsUnlocked;
            ShouldUnlock = data.ShouldUnlock;
            DefaultRecord = data.DefaultRecord;
            CurrentRecord = data.CurrentRecord;
            PreviousRecord = data.PreviousRecord;
        }
    }

    // O grupo de LevelDatas das fases planejadas, que pode ser salvo
    [Serializable]
    public struct SaveData
    {
        public LevelSaveData FactorySaveData;
        public LevelSaveData FarmSaveData;
        public LevelSaveData ForestSaveData;

        public SaveData(LevelData factoryData, LevelData farmData, LevelData forestData)
        {
            LevelSaveData factorySaveData = new(factoryData);
            LevelSaveData farmSaveData = new(farmData);
            LevelSaveData forestSaveData = new(forestData);

            FactorySaveData = factorySaveData;
            FarmSaveData = farmSaveData;
            ForestSaveData = forestSaveData;
        }

        public SaveData(AllLevelData allLevelData)
        {
            FactorySaveData = new(allLevelData.data[(int)LevelID.Factory]);
            FarmSaveData = new(allLevelData.data[(int)LevelID.Farm]);
            ForestSaveData = new(allLevelData.data[(int)LevelID.Forest]);
        }

        public void UpdateLevelData(LevelSaveData levelSaveData)
        {
            switch (levelSaveData.ID)
            {
                case LevelID.Factory:
                    FactorySaveData = levelSaveData;
                    break;
                case LevelID.Farm:
                    FarmSaveData = levelSaveData;
                    break;
                case LevelID.Forest:
                    ForestSaveData = levelSaveData;
                    break;
                default:
                    throw new Exception("No corresponding Level ID found.");
            }
        }

        public LevelSaveData GetLevelSaveData(LevelID id)
        {
            LevelSaveData data = id switch
            {
                LevelID.Factory => FactorySaveData,
                LevelID.Farm => FarmSaveData,
                LevelID.Forest => ForestSaveData,
                _ => throw new Exception("No corresponding Level ID found."),
            };
            return data;
        }
    }
}
