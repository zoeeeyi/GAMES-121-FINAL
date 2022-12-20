using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using UnityEngine.Events;
using static UnityEngine.LightProbeProxyVolume;

[CreateAssetMenu(fileName = "Game Data Controller", menuName = "Scriptable Objects/Game Data Controller")]
public class GameData : ScriptableObject
{
    #region Events
    //These events should do the same thing when called, regardless of current game state
    //[HideInInspector] public UnityEvent GAME_InitializeGame;
    [HideInInspector] public UnityEvent GAME_StartLevelPrep;
    [HideInInspector] public UnityEvent GAME_ContinueLevel;
    [HideInInspector] public UnityEvent GAME_RestartLevel;
    [HideInInspector] public UnityEvent GAME_PauseLevel;
    [HideInInspector] public UnityEvent GAME_WinLevel;
    [HideInInspector] public UnityEvent GAME_FailLevel;
    [HideInInspector] public UnityEvent GAME_QuitGame;
    #endregion

    #region General Settings
    [Header("Level Settings")]
    public string mainMenuName = "Main Menu";
    public string[] levelList;
    public Dictionary<string, int> levelDic { get; private set; }
    public NeonRounds.GameMode currentGameMode { get; private set; }
    public string currentLevel { get; private set; }
    public void SetGameModeAndLevel(NeonRounds.GameMode _mode, string _level)
    {
        currentGameMode = _mode;
        currentLevel = _level;
    }

    #endregion

    #region Gem Settings
    [Header("Gem Settings")]
    public int totalGems;
    public int gemCollected { get; private set; }

    public void SetGemCollected(int _g) { gemCollected = _g; }
    #endregion

    #region Speedrun Mode Settings
    [Header("Speedrun Mode Settings")]
    [Range(0, float.MaxValue)]
    public float speedRunStartTime;
    public float currentSessionRemainingTime { get; private set; }
    public Dictionary<string, float> speedRunBestTime { get; private set; }
    public void SetRemainingTime(float _rt) { currentSessionRemainingTime = _rt; }
    public void TrySetSpeedRunBestTime(float _time, string _levelName = null)
    {
        if (_levelName == null) _levelName = "Whole Game";
        if (!speedRunBestTime.TryGetValue(_levelName, out float _pb)) speedRunBestTime.Add(_levelName, _time);
        else if (_time > _pb) speedRunBestTime[_levelName] = _time;
    }
    #endregion

    #region Freerun Mode Settings
    public float currentSessionElapsedTime { get; private set; }
    public Dictionary<string, float> freerunBestTime { get; private set; }
    public void SetElapsedTime(float _et) { currentSessionElapsedTime = _et; }
    public void TrySetFreeRunBestTime(float _time, string _levelName = null)
    {
        if (_levelName == null) _levelName = "Whole Game";
        if (!freerunBestTime.TryGetValue(_levelName, out float _pb)) freerunBestTime.Add(_levelName, _time);
        else if (_time < _pb) freerunBestTime[_levelName] = _time;
    }

    #endregion

    #region Save and Load, limited to one file at this time
    /// <summary>
    ///Data loading should only happen when game is initialized from menu, to prevent corruption
    ///Save file should generally be isolated at all time outside of these method.
    /// </summary>
    /// <param name="_newSave"></param>

    //For now, Save Game Data should happen after we choose "NEW <GAME MODE>" in main menu
    public void SaveGameData(bool _newSave = false)
    {
        //prepare save file
        SaveFile _saveFile;
        if (_newSave)_saveFile = LogGameData(true);
        else _saveFile = LogGameData(false);

        //write save file to drive
        string _filePath = Path.Combine(Application.persistentDataPath, "NeonRoundsSave.json");
        Sirenix.Serialization.DataFormat _df = Sirenix.Serialization.DataFormat.JSON;
        byte[] bytes = SerializationUtility.SerializeValue(_saveFile, _df);
        File.WriteAllBytes(_filePath, bytes);
    }

    //For now, Load Game Data should only happen when there's a "Continue" Button
    public bool LoadGameData(bool _tryLoad = false)
    {
        string _filePath = Path.Combine(Application.persistentDataPath, "NeonRoundsSave.json");
        if (File.Exists(_filePath))
        {
            if (_tryLoad) return true;
            //string _jsonData = File.ReadAllText(_filePath);
            byte[] bytes = File.ReadAllBytes(_filePath);
            SaveFile _saveFile = SerializationUtility.DeserializeValue<SaveFile>(bytes, Sirenix.Serialization.DataFormat.JSON);
            //SaveFile _saveFile = JsonUtility.FromJson<SaveFile>(_jsonData);
            ExtractGameData(_saveFile);
            return true;
        } else
        {
            return false;
        }
    }

    public void DeleteGameData()
    {
        string _filePath = Path.Combine(Application.persistentDataPath, "NeonRoundsSave.json");
        if (File.Exists(_filePath)) File.Delete(_filePath);
    }

    SaveFile LogGameData(bool _createNew)
    {
        SaveFile _saveFile = new SaveFile();
        _saveFile.SAVE_currentGameMode = currentGameMode;
        _saveFile.SAVE_currentLevel = currentLevel;
        _saveFile.SAVE_gemCollected = (_createNew) ? (gemCollected = 0) : gemCollected;
        _saveFile.SAVE_speedrunBestTime = (speedRunBestTime == null) ? (speedRunBestTime = new Dictionary<string, float>()) : speedRunBestTime;
        _saveFile.SAVE_freerunBestTime = (freerunBestTime == null) ? (freerunBestTime = new Dictionary<string, float>()) : freerunBestTime;
        switch (currentGameMode)
        {
            case NeonRounds.GameMode.Speedrun:
                _saveFile.SAVE_currentTime = (_createNew) ? (currentSessionRemainingTime = speedRunStartTime) : currentSessionRemainingTime;
                break;

            case NeonRounds.GameMode.Freerun:
                _saveFile.SAVE_currentTime = (_createNew) ? 0 : currentSessionElapsedTime;
                break;

            default:
                _saveFile.SAVE_currentTime = 0;
                break;
        }

        return _saveFile;
    }
    void ExtractGameData(SaveFile _saveFile)
    {
        currentGameMode = _saveFile.SAVE_currentGameMode;
        currentLevel = _saveFile.SAVE_currentLevel;
        gemCollected = _saveFile.SAVE_gemCollected;
        speedRunBestTime = _saveFile.SAVE_speedrunBestTime;
        freerunBestTime = _saveFile.SAVE_freerunBestTime;
        switch (currentGameMode)
        {
            case NeonRounds.GameMode.Speedrun:
                currentSessionRemainingTime = _saveFile.SAVE_currentTime;
                break;

            case NeonRounds.GameMode.Freerun:
                currentSessionElapsedTime = _saveFile.SAVE_currentTime;
                break;
        }

    }
    #endregion

    #region Utility Methods
    public void LoadLevelHelper(string _levelName)
    {
        SceneManager.LoadScene(_levelName);

        //Test Codes:
        //if (_levelName != "Main Menu") NeonRounds.instance.ChangeGameState(NeonRounds.GameState.InGame);
    }

    public void LevelDirectoryCreator()
    {
        levelDic = new Dictionary<string, int>();
        for (int i = 0; i < levelList.Length; i++)
        {
            levelDic.Add(levelList[i], i);
        }
    }
    #endregion
}

[System.Serializable]
class SaveFile
{
    public NeonRounds.GameMode SAVE_currentGameMode;
    public string SAVE_currentLevel;
    public float SAVE_currentTime;
    public Dictionary<string,float> SAVE_speedrunBestTime;
    public Dictionary<string, float> SAVE_freerunBestTime;
    public int SAVE_gemCollected;
}
