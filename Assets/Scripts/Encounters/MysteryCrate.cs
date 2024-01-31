using UnityEngine;

public class MysteryCrate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menu;

    void Start()
    {
        menu.SetActive(false);
    }

    //--------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Triggering the encounter
        switch (collision.name)
        {
            case "EncounterPoint":
                {
                    GameObject.Find("Main Camera").GetComponent<Explore>().EncounterStarted();
                    menu.SetActive(true);
                    break;
                }
            case "DestroyPoint":
                {
                    Destroy(this.gameObject);
                    break;
                }
        }
    }

    //--------------------------------------------------------------------------
    public void AcceptCrate()
    {
        menu.SetActive(false);

        /* Show animation of breaking the crate
         * Determine loot within crate
         * Show and give loot to player
         * Continue Run
        */

        /* Possible Drops:
         * Coins
         * Nothing (poof effect)
         * Stat Buff?
         * Damage a troop?
         * 
        */

        GameObject.Find("Main Camera").GetComponent<Explore>().ContinueGame();

    }

    //--------------------------------------------------------------------------
    public void RejectCrate()
    {
        // Ignore the crate, nothing happens
        menu.SetActive(false);
        GameObject.Find("Main Camera").GetComponent<Explore>().ContinueGame();
    }
}

