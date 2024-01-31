using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestSelection : MonoBehaviour
{
    // List of the locations of where the images will be shown
    public List<GameObject> imageLocations = new List<GameObject>();

    // List contianing each individual aniamtor
    public List<Animator> animators;

    // Placeholder items for now
    [SerializeField] Sprite coin;
    [SerializeField] Sprite heart;
    [SerializeField] Sprite shield;
    [SerializeField] Sprite potion;

    Dictionary<string, Sprite> lootDictionary = new Dictionary<string, Sprite>();

    private List<string> chosenItems = new List<string>();

    //--------------------------------------------------------------------------
    private void Start()
    {
        // Initalise loot table, for now it is a dictionary
        lootDictionary.Add("coin", coin);
        lootDictionary.Add("heart", heart);
        lootDictionary.Add("shield", shield);
        lootDictionary.Add("potion", potion);

        // Reduce transparency of each image to 0
        foreach (GameObject obj in imageLocations)
        {
            obj.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        }

        // Select 3 random loot
        for (int i = 0; i < 3; i++)
        {
            bool itemAllocated = false;
            while (itemAllocated == false)
            {
                // Loops through the dictionary
                foreach (KeyValuePair<string, Sprite> entry in lootDictionary)
                {
                    // Gives random chance to select the item and add it to a chest
                    if (Random.Range(0, 100) < 12)
                    {
                        chosenItems.Add(entry.Key);
                        itemAllocated = true;
                    }
                    if (itemAllocated) break;
                }
            }
            imageLocations[i].GetComponent<Image>().sprite = lootDictionary[chosenItems[i]];
        }
    }
    //--------------------------------------------------------------------------
    public void ChestPressed(int btnId)
    {
        // Play opening animation
        foreach (Animator animator in animators)
        {
            animator.SetTrigger("Chest Opened");
        }

        StartCoroutine(ShowPrizes());

        // Give prize to the player

        Instantiate(GameManager.Game.exitEncounterPrefab, GameObject.Find("Canvas").transform);
    }

    //--------------------------------------------------------------------------
    private IEnumerator ShowPrizes()
    {
        yield return new WaitForSeconds(1f);

        // Reset transparency
        foreach (GameObject obj in imageLocations)
        {
            obj.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
