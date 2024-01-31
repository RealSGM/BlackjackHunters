using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /* Used to store game data and run general functions
     * 
    */
    public static GameManager Game { get; private set; }

    public GameObject loadingScrenePrefab;
    public GameObject exitEncounterPrefab;

    //--------------------------------------------------------------------------
    private void Awake()
    {
        if (Game == null)
        {
            // Singleton Setup
            Game = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy extra instances of singleton
            Destroy(gameObject);
        }
    }

    //--------------------------------------------------------------------------
    private void Start()
    {
        LoadGame();
    }

    //--------------------------------------------------------------------------
    public static void LoadGame()
    {
        // Load game data from file and save into data singleton
        GeneralGameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            DataManager.Data.newPlayer = data.newPlayer;
            DataManager.Data.coins = data.coins;
            DataManager.Data.audioLevel = data.audioLevel;
            DataManager.Data.playerName = data.playerName;
            DataManager.Data.audioMixer.SetFloat("Volume", DataManager.Data.audioLevel);
        }
    }

    //--------------------------------------------------------------------------
    public void LoadScene(string sceneName)
    {
        // Load a new scene via loading scene
        GameObject loadingScreen = Instantiate(loadingScrenePrefab, GameObject.Find("Canvas").transform);
        StartCoroutine(LoadAsynchronously(sceneName));
        loadingScreen.SetActive(true);
    }

    //--------------------------------------------------------------------------
    public static IEnumerator LoadAsynchronously(string sceneName)
    {
        Slider slider = GameObject.Find("LoadingSlider").GetComponent<Slider>();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }

    //--------------------------------------------------------------------------
    public void BlackjackResults(bool won)
    {
        int index = DataManager.Data.Troops.IndexOf(DataManager.Data.tempTroop);

        DataManager.Data.tempTroop = null;
        DataManager.Data.Troops[index].StatsUpgraded = true;

        Debug.Log("Troop: " + DataManager.Data.Troops[index].Name + " has finished blackjack ");

        if (won)
        {
            // Upgrade troops stats
        }
        else
        {
            // Either downgrade or keep the same?
        }

        SaveSystem.SaveArmy();
    }
}
