using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Corrida
{
    public class SaveDataManager : MonoBehaviour
    {
        [SerializeField] private LevelData _factoryDataObject, _farmDataObject, _forestDataObject;
        private AllLevelData _allLevelObjectData;
        private AllLevelData _defaultLevelData;
        public static LevelID CurrentLevelID;
        private SaveData _saveData;

        public static SaveDataManager Instance { get; private set; }

        private void SetDefaultDataVariables()
        {
            List<LevelData> data = new()
            {
                _factoryDataObject,
                _farmDataObject,
                _forestDataObject
            };

            _defaultLevelData = new(data);
        }

        private SaveData SaveDefaultLevelData()
        {
            SaveData saveData = GenerateDefaultSaveData();
            DataHandler.SaveRunnerData(saveData);
            Debug.Log("Default Save Data Created");
            return saveData;
        }

        private SaveData GenerateDefaultSaveData()
        {
            if (_defaultLevelData == null)
                SetDefaultDataVariables();

            var saveData = new SaveData(_defaultLevelData);

            return saveData;
        }

        private LevelID CheckSceneLevelID()
        {
            LevelID id;

            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == _factoryDataObject.SceneName)
                id = _factoryDataObject.ID;
            else if (currentScene == _farmDataObject.SceneName)
                id = _farmDataObject.ID;
            else if (currentScene == _forestDataObject.SceneName)
                id = _forestDataObject.ID;
            else
                id = LevelID.None;

            return id;
        }


        private void GetSavedLevelData()
        {
            _saveData = DataHandler.GetRunnerSaveData();

            if (_saveData.FactorySaveData != null && _saveData.FarmSaveData != null && _saveData.ForestSaveData != null)
            {
                UpdateAllLevelDataObjects(new(_saveData));
                Debug.Log("Save Data Loaded");
            }
            else
            {
                _saveData = SaveDefaultLevelData();
            }
        }

        private void SaveObjectsData()
        {
            _saveData = new(_allLevelObjectData);
            DataHandler.SaveRunnerData(_saveData);
        }

        private void UpdateSingleLevelDataObject(LevelData levelData)
        {
            switch (levelData.ID)
            {
                case LevelID.Factory:
                    _factoryDataObject = levelData;
                    break;
                case LevelID.Farm:
                    _farmDataObject = levelData;
                    break;
                case LevelID.Forest:
                    _forestDataObject = levelData;
                    break;
                default:
                    break;
            }
        }

        private void UpdateAllLevelDataObjects(AllLevelData allLevelData)
        {
            foreach (LevelData levelData in allLevelData.data)
            {
                UpdateSingleLevelDataObject(levelData);
            }
        }

        private List<LevelData> ListLevelDataFromObjects()
        {
            List<LevelData> levelList = new()
            {
                _factoryDataObject,
                _farmDataObject,
                _forestDataObject
            };

            return levelList;
        }

        private void CompareLevelData(AllLevelData allLevelData, Action DiscrepancyDetected)
        {
            AllLevelData objectsData = new(ListLevelDataFromObjects());

            for (int i = 0; i < allLevelData.data.Count; i++)
            {
                if (allLevelData.data[i].ID != objectsData.data[i].ID || allLevelData.data[i].DefaultRecord != objectsData.data[i].DefaultRecord)
                {
                    DiscrepancyDetected?.Invoke();
                    break;
                }
            }
        }

        public LevelData GetCurrentLevelData()
        {
            return _allLevelObjectData.GetLevelDataByID(CurrentLevelID);
        }

        private void OnGoalReached()
        {
            if (TimeController.Instance != null)
            {
                _allLevelObjectData.data[(int)CurrentLevelID].UpdateRecord(TimeController.Instance.TotalTimeSpan);
            }
        }

        private void OnRecordUpdated()
        {
            SaveObjectsData();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            SetDefaultDataVariables();

            bool isFirstBoot = DataHandler.CheckIfSaveExists();

            if (isFirstBoot)
            {
                _saveData = SaveDefaultLevelData();
            }
            else
            {
                GetSavedLevelData();
            }

            _allLevelObjectData = new(ListLevelDataFromObjects());

            CurrentLevelID = CheckSceneLevelID();

            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            if (Instance == null)
                Instance = this;

            Goal.GoalReached += OnGoalReached;
            LevelData.RecordUpdated += OnRecordUpdated;
        }

        private void OnDisable()
        {
            if (Instance == this)
                Instance = null;

            Goal.GoalReached -= OnGoalReached;
            LevelData.RecordUpdated -= OnRecordUpdated;
        }
    }
}
