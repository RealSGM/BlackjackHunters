using UnityEngine;
using System.Collections.Generic;

public class NewAlly : MonoBehaviour
{
    [SerializeField] GameObject troopInstance;
    [SerializeField] GameObject rejectButton;
    [SerializeField] GameObject acceptButton;

    [HideInInspector]Animator animator;
    Troop newAlly;

    //--------------------------------------------------------------------------
    void Start()
    {
        // Hide menus
        rejectButton.SetActive(false);
        acceptButton.SetActive(false);

        // Generate new random ally
        List<string> types = DataManager.Data.OrderedType;
        types.Remove("Prince");

        int index = Random.Range(0, types.Count - 1);
        string allyType = types[index];
        string allyName = DataManager.Data.RandomString(5);

        newAlly = new Troop(allyName,allyType,0,0,0,0,0,false,false); // Determine base stats later

        // Instance
        troopInstance.name = allyName;

        // Start walking
        animator = troopInstance.GetComponent<Animator>();
        animator.runtimeAnimatorController = DataManager.Data.troopRuntimeAnimatorControllers[index];
        animator.SetTrigger("StartRunning");

        // Make troop face the other way
        troopInstance.transform.localScale= new Vector2(-troopInstance.transform.localScale.x, troopInstance.transform.localScale.y);

        troopInstance.GetComponent<TroopIndividual>().troop = newAlly;
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
                    animator.SetTrigger("StopRunning");
                    acceptButton.SetActive(true);
                    rejectButton.SetActive(true);
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
    public void AcceptTroop()
    {
        /* Check if troop can fit into selected slots
         * If so add them into selected troops
         * Show them running into the correct position
        */

        // Otherwise move them into army camp and show running animation
        DataManager.Data.Troops.Add(newAlly);

        GameObject.Find("Main Camera").GetComponent<Explore>().InitiateBlackjack(newAlly);
        acceptButton.SetActive(false);
        rejectButton.SetActive(false);

    }

    //--------------------------------------------------------------------------
    public void RejectTroop()
    {
        acceptButton.SetActive(false);
        rejectButton.SetActive(false);
        GameObject.Find("Main Camera").GetComponent<Explore>().ContinueGame();
        animator.SetTrigger("StartRunning");
    }
}
