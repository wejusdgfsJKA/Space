using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{
    static GameSave currentSave;
    public static float PlayerPower { get; private set; } = 0;
    public static float EnemyPower { get; private set; } = 0;

    #region Pausing
    public static bool IsPaused => Time.timeScale == 0f;

    public static void Pause()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public static void TogglePause()
    {
        Cursor.visible = !Cursor.visible;
        Cursor.lockState = IsPaused ? CursorLockMode.Locked : CursorLockMode.None;
        Time.timeScale = IsPaused ? 1f : 0f;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    #endregion

    public static void ResetProgress()
    {
        currentSave = new();
        //Save("");
    }

    public static void Load(string fileName)
    {
        var s = GameSave.Load(GlobalConfig.GetSaveFilePath(fileName));
        currentSave = s != null ? s.Value : new();
    }

    public static void Save(string fileName)
    {
        currentSave.Save(GlobalConfig.GetSaveFilePath(fileName));
    }

    public static void EndMission()
    {
        SceneManager.LoadScene(GlobalConfig.EndMissionSceneIndex);
    }
}
