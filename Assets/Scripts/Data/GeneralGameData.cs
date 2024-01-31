
[System.Serializable]
public class GeneralGameData
{
    // Class containing general data that the user will store
    public int coins;
    public string playerName;
    public bool newPlayer;
    public float audioLevel;


    public GeneralGameData()
    {
        coins = DataManager.Data.coins;
        playerName = DataManager.Data.playerName;
        newPlayer = DataManager.Data.newPlayer;
        audioLevel = DataManager.Data.audioLevel;
    }
}
