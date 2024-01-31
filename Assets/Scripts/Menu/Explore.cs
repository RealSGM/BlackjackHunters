using System.Collections.Generic;
using UnityEngine;

public class Explore : MonoBehaviour
{
    // Character prefabs
    [SerializeField] private GameObject troopPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyEncounter;
    [SerializeField] private GameObject newAllyEncounter;
    [SerializeField] private GameObject healingWellEncounter;
    [SerializeField] private GameObject holeEncounter;
    [SerializeField] private GameObject mysteryCrateEncounter;
    [SerializeField] private GameObject devilEncounter;
    [SerializeField] private GameObject villageEncounter;
    [SerializeField] private GameObject bossEncounter;
    [SerializeField] private GameObject blackjackGame;
    [SerializeField] private GameObject slotMachineGame;
    [SerializeField] private GameObject chestSelectionGame;

    [HideInInspector] public GameObject currentEncounter;

    private List<GameObject> encounterList = new List<GameObject>();

    readonly private float distFromPlayer = 10;
    readonly private int moveSpeed = 2;

    // Used to check if the player is currently searching
    private bool searching = true;
    private float searchTime = 10f;

    // Used to check if the player is currently doing an event requiring the bottom half of the screen
    private bool idle = true;

    //--------------------------------------------------------------------------
    private void LoadTestGame()
    {
        // Temp Load Data

        // for(int i = 0; i < 3 ; i ++)
        // {
        //     TroopManager.CreateTroop("Archer",0,0,0,0,0,true,false);
        //     TroopManager.CreateTroop("Knight",0,0,0,0,0,true,false);
        //     TroopManager.CreateTroop("Spearman",0,0,0,0,0,true,false);
        //     TroopManager.CreateTroop("Infantry",0,0,0,0,0,true,false);
        // }

        SaveSystem.SaveGame();
        GameManager.LoadGame();
        SaveSystem.SaveArmy();
        ArmyManager.LoadArmy();

        encounterList = new List<GameObject> { enemyEncounter };
    }

    //--------------------------------------------------------------------------
    public void ExitExplore()
    {
        /* Display warning saying troops will be abandoned
         * Remove troops from selected troops, except prince / knight
         * Delete troops from army camp
        */
        GameManager.Game.LoadScene("Castle");
    }

    //--------------------------------------------------------------------------
    private void Start()
    {
        encounterList = new List<GameObject>() { enemyEncounter, newAllyEncounter,
            healingWellEncounter, holeEncounter, mysteryCrateEncounter,
            devilEncounter, villageEncounter, bossEncounter };

        LoadTestGame();

        // Load the prince to see if this is first time playing
        Troop Prince = ArmyManager.GetTroopByName(DataManager.Data.playerName);

        /* Find how many types of troops there are to determine the columns
         * Array is made to store troop types and if the troop type is already there then it is not added
         * .Count of that and you have the columns
        */

        List<string> TypeList = new List<string>();
        //int LEFT_POS = 0;
        List<int> NoTypes = new List<int>();

        // Set all types to 0
        for (int i = 0 ; i < 5 ; i ++)
        {
            NoTypes.Add(0);
        }

        for (int i = 0; i < DataManager.Data.selectedTroops.Count; i++)
        {
            string type = DataManager.Data.selectedTroops[i].Type;
            if (!TypeList.Contains(type))
            {   
                TypeList.Add(type);
            }
            // Finding the no of troop for each column that will be added
            int index = DataManager.Data.OrderedType.IndexOf(type);
            NoTypes[index] ++;

        }
        int Count = 0;
        foreach(string element in DataManager.Data.OrderedType)
        {
            foreach(Troop troop in DataManager.Data.selectedTroops)
            {
                if (troop.Type == element)
                {
                    int NoRows = NoTypes[Count];
                    
                }
            }
            Count ++;
        }

        

        //int Columns = TypeList.Count;
        //Debug.Log(string.Join(", ", NoTypes));
        //Debug.Log(Columns);

        /* We use the troop type list as a reference to where to put the troops but it needs to be ordered depending on their type
         * Prince/King  ,Archer,Ranger,  Knight,Bishop,  Spearman,Thearman,  Infantry,HeavyInfantry
         * This will be the order of the types, the upgrades types will always be infront however they will be grouped up into smaller groups
         * To find the rows we cycle through each troop type and find the maximum number of troops in any troop type
         * The grid will be even so all we need is the max
        */

        // Summon each troop
        for (int i = 0; i < DataManager.Data.selectedTroops.Count; i++)
        {
            Troop troop = DataManager.Data.selectedTroops[i];
            float xPos = (i + 1) * 0.55f -2.5f;
            float yPos = 2.6f;
            SummonTroop(troop, xPos, yPos);
        }

        // Check if this is a new prince
        if (!Prince.StatsUpgraded)
        {
            InitiateBlackjack(Prince);
        }
    }

    //--------------------------------------------------------------------------
    private void Update()
    {
        if (searching)
        {
            // Move troops
            if (idle)
            {
                UpdateAllTroops("StartRunning");
                idle = false;
            }

            // Decrease search time
            searchTime = Mathf.Max(0, searchTime - Time.deltaTime);

            // Move camera / move troops across map
            Vector2 camPos = GameObject.Find("Main Camera").transform.position;
            GameObject.Find("Main Camera").transform.position = new Vector2(camPos.x + Time.deltaTime * moveSpeed, camPos.y);
            if (searchTime == 0)
            {
                // Predetermine encounter, when the actual troop is visible on the screen past a point, then the troops will stop running
                CreateEncounter();
            }
        }
    }

    //--------------------------------------------------------------------------
    private void SummonTroop(Troop _troop,float xPos, float yPos)
    {
        // Instantiate a troop prefab
        GameObject newTroopPrefab = Instantiate(troopPrefab, GameObject.Find("World").GetComponent<Transform>());

        newTroopPrefab.transform.position = new Vector3(xPos,yPos,10);

        newTroopPrefab.name = _troop.Name;

        // Saves the troop class into the troop prefab
        newTroopPrefab.transform.GetComponent<TroopIndividual>().troop = _troop;

        // Set sprites / animation players based on troop.Type
        int index = DataManager.Data.troopIndexes.IndexOf(_troop.Type);
        newTroopPrefab.transform.GetComponent<Animator>().runtimeAnimatorController = DataManager.Data.troopRuntimeAnimatorControllers[index];

        // Possibly add a nametag with troop.Name
        //newTroopPrefab.transform.Find("Nametag").GetComponent<TextMeshPro>().text = _troop.Name;

        // For healthbar: as scale reduces, the position.x becomes: -0.5 * scale.x
    }

    //--------------------------------------------------------------------------
    private void UpdateAllTroops(string triggerName)
    {
        // Apply animation to each troop in the exploration army
        foreach (Troop troop in DataManager.Data.selectedTroops)
        {
            Animator animator = GameObject.Find(troop.Name).GetComponent<Animator>();
            animator.SetTrigger(triggerName);
        }
    }

    //--------------------------------------------------------------------------
    private void CreateEncounter()
    {
        // Spawn random encounter ahead of the prince / king  
        if (currentEncounter == null)
        {
            currentEncounter = Instantiate(encounterList[Random.Range(0, encounterList.Count - 1)]); // Temporary way to choose encounter, odds will have to be changed
            Vector2 pos = GameObject.Find(DataManager.Data.playerName).transform.position; 
            currentEncounter.transform.position = new Vector2(pos.x + distFromPlayer, pos.y);
        }
    }

    //--------------------------------------------------------------------------
    public void InitiateBlackjack(Troop troop)
    {
        //Instantiate blackjack for new prince
        currentEncounter = Instantiate(blackjackGame,GameObject.Find("Main Camera").transform);
        idle = false;
        searching = false;
        DataManager.Data.tempTroop = troop;
    }

    //--------------------------------------------------------------------------
    public void ContinueGame()
    {
        // Player will continue exploring after ending encounter
        idle = true;
        searching = true;
        currentEncounter = null;
    }

    //--------------------------------------------------------------------------
    public void EncounterStarted()
    {
        searching = false;
        idle = false;
        searchTime = Random.Range(5, 20);
        UpdateAllTroops("StopRunning");
    }
}
