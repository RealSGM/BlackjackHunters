using System.Collections.Generic;
using System.Linq;
using UnityEngine.Audio;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    /* Used for storing persistent data
     * 
     * - Create random name generator
    */
    public static DataManager Data { get; private set; }

    // Army Data
    [HideInInspector] public int maxArmySize = 30;
    [HideInInspector] public List<Troop> selectedTroops = new List<Troop>();
    // Stores the troops within the army camp
    [HideInInspector] public List<Troop> Troops = new List<Troop>();

    [Header("Troop Data")]
    public List<string> troopIndexes;
    public List<Sprite> troopSprites;
    public List<RuntimeAnimatorController> troopRuntimeAnimatorControllers;
    [HideInInspector] public List<string> OrderedType = new List<string> { "Prince", "Archer", "Knight", "Spearman", "Infantry" };

    [Header("Enemy Data")]
    public List<string> goblinIndexes;
    public List<Sprite> goblinSprites;
    public List<RuntimeAnimatorController> goblinRuntimeAnimatorControllers;

    [Header("Game Data")]
    public bool newPlayer = true;
    public int coins = 0;
    public string playerName = "";
    public float audioLevel = 0f;

    [Header("Audio Data")]
    public AudioMixer audioMixer;

    [Header("Explore Data")]
    [HideInInspector] public Troop tempTroop;

    [HideInInspector] public string enemyEncounterName = "EnemyEncounter(Clone)";



    //--------------------------------------------------------------------------
    private void Awake()
    {
        if (Data == null)
        {
            // Singleton Setup
            Data = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy extra instances of singleton
            Destroy(gameObject);
        }
    }

    //--------------------------------------------------------------------------
    public string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    // Random string generator
    private static readonly System.Random random = new System.Random();

}
