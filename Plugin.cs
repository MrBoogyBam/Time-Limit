using UnityEngine;
using UnityEngine.SceneManagement;
using BepInEx;
using BepInEx.Configuration;

namespace TimeLimit
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public ConfigEntry<int> minutes;
        public ConfigEntry<int> seconds;

        private float timeRemaining;

        private bool gameStarted = false;

        private void Awake()
        {
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            minutes = Config.Bind("Time Limit", "Minutes", 1, "The amount of minutes in the time limit.");
            seconds = Config.Bind("Time Limit", "Seconds", 30, "The amount of seconds in the time limit.");

            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void Update()
        {
            if(timeRemaining > 0 && gameStarted)
            {
                timeRemaining -= Time.deltaTime;

                Logger.LogInfo(timeRemaining);
            }
            else if (timeRemaining <= 0 && gameStarted)
            {
                RestartGame();

                gameStarted = false;

                Logger.LogInfo("Time ran out.");
            }
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.name == "Mian")
            {
                timeRemaining = minutes.Value * 60 + seconds.Value;

                gameStarted = true;

                Logger.LogInfo("Timer Started");
            }
            else
            {
                gameStarted = false;
            }
        }

        private void RestartGame()
        {
            PlayerPrefs.DeleteKey("NumSaves");
            PlayerPrefs.DeleteKey("SaveGame0");
            PlayerPrefs.DeleteKey("SaveGame1");
            PlayerPrefs.Save();

            SceneManager.LoadScene("Mian");
        }

        private void OnGUI()
        {
            if(SceneManager.GetActiveScene().name == "Loader")
            {
                if(GUI.Button(new Rect(10, 10, 200, 100), "Test"))
                {
                    Logger.LogInfo("Button Pressed");
                }
            }
        }
    }
}
