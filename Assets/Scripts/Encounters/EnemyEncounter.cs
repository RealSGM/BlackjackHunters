using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyEncounter : MonoBehaviour
{
    /* Enemy encounters
     * 
     * - Create proper layout for enemies 
     * - Tap screen for plinko ball to drop on command
     * - Summon balls, each representing troops and enemies, troops on left, enemies on right
     * - As troops drop balls, enemies will do shortly after
     * - Damage characters when balls land in detectors
     * - Create floating healing orbs

    */


    
    [HideInInspector] public GameObject currentTroopBall;
    [HideInInspector] public GameObject currentEnemyBall;

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject plinkoPrefab;
    [SerializeField] private GameObject plinkoBallPrefab;

    private List<Enemy> enemyList = new List<Enemy>();
    private List<Troop> troopList = new List<Troop>();

    public bool playerDroppedAllBalls = false;
    public bool enemyDroppedAllBalls = false;

    private void Start()
    {
        this.gameObject.transform.position = new Vector3(10, 2.75f, 10) + GameObject.Find("Main Camera").transform.position;

        // Hide plinko
        plinkoPrefab.SetActive(false);

        // Move plinko under world object
        plinkoPrefab.transform.SetParent(GameObject.Find("World").transform);
        plinkoPrefab.transform.localPosition = new Vector3(0, 0, 1);

        troopList = new List<Troop>(DataManager.Data.selectedTroops);

        int enemyCount = DataManager.Data.selectedTroops.Count;

        for (int i = 0; i < enemyCount; i++)
        {
            Enemy enemy = new Enemy("Enemy" + i,"Chief",1,1,1,0,1); // Only one chief, rest should be random or preconfigured teams
            float xPos = (i + 1) * -0.55f + 30f;
            float yPos = this.gameObject.transform.position.y;
            SummonEnemy(enemy, xPos, yPos); // Position need to be set properly later like with player troops
        }

        StartNewRound();        
    }


    private void Update()
    {
        if (playerDroppedAllBalls && enemyDroppedAllBalls)
        {
            StartNewRound();

            // Reset troop list 
            troopList = new List<Troop>(DataManager.Data.selectedTroops);

            enemyList = new List<Enemy>();
            
            // Figure out how to loop through child nodes
            foreach (GameObject enemyObject in transform.Find("EnemyEncounter(Clone").transform) 
            {
                // Add the enemy data type to the enemy list
            }

            // Add enemy ball drop

        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Triggering the encounter
        switch (collision.name)
        {
            case "EncounterPoint":
                {
                    Debug.Log("Encounter Point has been triggered");
                    GameObject.Find("Main Camera").GetComponent<Explore>().EncounterStarted();
                    EnemyEncounterStarted();
                    break;
                }
            case "DestroyPoint":
                {
                    Destroy(this.gameObject);
                    break;
                }
        }
    }

    private void EnemyEncounterStarted()
    {
        foreach (Enemy enemy in enemyList)
        {
            string eName = enemy.Name;
            GameObject enemyObj = GameObject.Find(eName);
            enemyObj.GetComponent<Animator>().SetTrigger("StopRunning");
        }
        // Dialogues / Grunts

        // Start first round of plinko 
        plinkoPrefab.SetActive(true);
    }

    private void SummonEnemy(Enemy _enemy, float xPos, float yPos)
    {
        // Instantiate enemy prefab
        GameObject newEnemyPrefab = Instantiate(enemyPrefab,this.gameObject.transform);

        newEnemyPrefab.transform.position = new Vector3(xPos,yPos, 10);

        newEnemyPrefab.name = _enemy.Name;

        // Saves the troop class into the troop prefab
        newEnemyPrefab.transform.GetComponent<EnemyIndividual>().enemydata = _enemy;

        // Set sprites / animation players based on troop.Type

        //Uses goblin indexes for now
        int index = DataManager.Data.goblinIndexes.IndexOf(_enemy.Type);
        newEnemyPrefab.transform.GetComponent<Animator>().runtimeAnimatorController = DataManager.Data.goblinRuntimeAnimatorControllers[index];

        // Possibly add a nametag with troop.Name
        newEnemyPrefab.transform.Find("Nametag").GetComponent<TextMeshPro>().text = _enemy.Name;
        newEnemyPrefab.GetComponent<Animator>().SetTrigger("StartRunning");


        enemyList.Add(_enemy);

        // For healthbar: as scale reduces, the position.x becomes: -0.5 * scale.x
    }
    
    private void StartNewRound()
    {
        playerDroppedAllBalls = false;
        enemyDroppedAllBalls = false;

        currentTroopBall = SpawnBall("Troop", 0,"TroopBallPos","Troop");
        currentEnemyBall = SpawnBall("Enemy", 0,"EnemyBallPos","Enemy");
        Physics2D.IgnoreCollision(currentTroopBall.GetComponent<CircleCollider2D>(),currentEnemyBall.GetComponent<CircleCollider2D>());

    }

    private GameObject SpawnBall(string type, int index,string location,string ballType)
    {
        GameObject ball = Instantiate(plinkoBallPrefab, plinkoPrefab.transform.Find(location).transform);
        
        switch (type)
        {
            case "Troop":
                {
                    // Set ball colour
                    ball.name = troopList[index].Name + "-Ball";
                    troopList.RemoveAt(index);
                    break;
                }
            case "Enemy":
                {
                    ball.name = enemyList[index].Name + "-Ball";
                    enemyList.RemoveAt(index);
                    break;
                }
        }
        ball.GetComponent<PlinkoBall>().ballType = ballType;
        ball.GetComponent<PlinkoBall>().MoveBall(-1);

        return ball;
    }


    public void DropTroopBall()
    {
        if (currentTroopBall != null)
        {
            // For current ball:
            currentTroopBall.GetComponent<PlinkoBall>().hasDropped = true;
            Rigidbody2D rb = currentTroopBall.GetComponent<Rigidbody2D>();
            rb.gravityScale = 0.5f;
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.7f);

            if (troopList.Count == 0)
            {
                playerDroppedAllBalls = true;
            }
        }
    }

    public void DropEnemyBall()
    {
        // drop enemy ball
        currentEnemyBall.GetComponent<PlinkoBall>().hasDropped = true;
        Rigidbody2D rb = currentTroopBall.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.5f;
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.7f);

        if (enemyList.Count == 0)
        {
            enemyDroppedAllBalls = true;
        }

    }


    public void SpawnTroopBall()
    {
        if (troopList.Count > 0)
        {
            currentTroopBall = SpawnBall("Troop", 0, "TroopBallPos","Troop");
            Physics2D.IgnoreCollision(currentTroopBall.GetComponent<CircleCollider2D>(), currentEnemyBall.GetComponent<CircleCollider2D>());
        }
    }

    public void SpawnEnemyBall()
    {   
        if (enemyList.Count > 0)
        {
            currentEnemyBall = SpawnBall("Enemy", 0, "EnemyBallPos","Enemy");
            Physics2D.IgnoreCollision(currentTroopBall.GetComponent<CircleCollider2D>(), currentEnemyBall.GetComponent<CircleCollider2D>());
        }
    }
}

