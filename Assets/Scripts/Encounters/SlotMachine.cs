using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotMachine : MonoBehaviour
{
    // Game object storing the sprite
    public List<Image> slotHolderOne = new List<Image>();
    public List<Image> slotHolderTwo = new List<Image>();
    public List<Image> slotHolderThree = new List<Image>();
    List<List<Image>> slotImageTable = new List<List<Image>>();

    // Sprites used for the different slot icons
    [SerializeField] Sprite heart;
    [SerializeField] Sprite chest;
    [SerializeField] Sprite coin;
    [SerializeField] Sprite sword;
    [SerializeField] Sprite key;
    [SerializeField] Sprite skull;
    [SerializeField] Sprite apple;

    // List containing all of the sprite icons
    List<Sprite> slotSprites;

    // Lever button game object
    [SerializeField] Button leverButton;

    // Contains the order that which the slot "rotates" / the pointer moves along and shows the sprites
    List<Sprite> slotColumnOne = new List<Sprite>();
    List<Sprite> slotColumnTwo = new List<Sprite>();
    List<Sprite> slotColumnThree = new List<Sprite>();
    List<List<Sprite>> slots = new List<List<Sprite>>();

    // Contains the pointers for each slot
    List<int> slotPointerOne = new List<int>() { 0, 1, 2 };
    List<int> slotPointerTwo = new List<int>() { 0, 1, 2 };
    List<int> slotPointerThree = new List<int>() { 0, 1, 2 };

    List<List<int>> slotPointerTable = new List<List<int>>();

    //--------------------------------------------------------------------------
    private void Start()
    {
        // Create different lists used for the slot assignment
        slotSprites = new List<Sprite> { heart, chest, coin, sword, key, skull, apple };
        slots = new List<List<Sprite>>() { slotColumnOne, slotColumnTwo, slotColumnThree };
        slotImageTable = new List<List<Image>>() { slotHolderOne, slotHolderTwo, slotHolderThree };
        slotPointerTable = new List<List<int>>() { slotPointerOne, slotPointerTwo, slotPointerThree };

        // Create slot lists and shuffle contents
        for (int i = 0; i < 3; i++)
        {
            // This print statement fixes the code
            Debug.Log("");  
            foreach (Sprite slotIcon in slotSprites)
            {
                slots[i].Add(slotIcon);
                slots[i].Add(slotIcon);
                slots[i].Add(slotIcon);
            }
            slots[i].Shuffle();
        }

        // Display slots
        // Goes through each column, left to right
        for (int i = 0; i < 3; i++)
        {
            // Goes through each slot in the column, top to bottom
            for (int j = 0; j < 3; j++)
            {
                // Get the current columns sprite based on that slot's current pointer 
                slotImageTable[i][j].sprite = slots[i][slotPointerTable[i][j]];
            }
        }
    }

    //--------------------------------------------------------------------------
    public void FlickLever()
    {
        // Disable lever
        leverButton.enabled = false;
        StartCoroutine(SpinSlotMachine());
    }

    //--------------------------------------------------------------------------
    private IEnumerator SpinSlotMachine()
    {
        // Spin each individual slot column
        for (int i = 0; i < 3; i++)
        {
            // Generate how many times the slot should spin
            int slotCount = Random.Range(2, 35);

            for (int j = 0; j < slotCount; j++)
            {
                // Adjust speed based on progress of the slot column
                float waitTime = 0.08f;
                if (j > slotCount * 0.9f) waitTime = 0.45f;
                else if (j > slotCount * 0.75f) waitTime = 0.3f;
                else if (j > slotCount * 0.6f) waitTime = 0.15f;

                    // Goes through each each row of the current column
                    for (int k = 0; k < slotPointerTable[i].Count; k++)
                {
                    // Only decrement when the value is greater than the first index (aka 0)
                    if (slotPointerTable[i][k] > 0)
                    {
                        slotPointerTable[i][k]--;
                    }
                    else
                    {
                        slotPointerTable[i][k] = slots[i].Count - 1;
                    }
                    // Update sprites
                    slotImageTable[i][k].sprite = slots[i][slotPointerTable[i][k]];
                }
                yield return new WaitForSeconds(waitTime);
            }
        }

        // Scan icons and put into dictionary
        Dictionary<string,int> playerSlots = new Dictionary<string, int>();
        for (int i = 0; i < 3; i++)
        {
            // Retrieves the icon name
            string iconToFind = slotImageTable[i][1].sprite.name;
            if (!playerSlots.ContainsKey(iconToFind))
            {
                // Adds icon to the list if its unique
                playerSlots.Add(iconToFind,1);
            }
            else
            {
                playerSlots[iconToFind]++;
            }
        }

        // Prize check
        string prize = null;

        foreach (KeyValuePair<string,int> entry in playerSlots)
        {
            switch (entry.Value)
            {
                // Icon has been repeated twice
                case 2:
                    prize = entry.Key; 
                    Debug.Log("Double " + entry.Key);
                    break;

                // Icon has been repeated thrice
                case 3:      
                    prize = entry.Key;
                    Debug.Log("Triple " + entry.Key);
                    break;

                // No prize
                default:
                    break;
            }
        }

        if (prize == null)
        {
            Debug.Log("No prize");
        }

        // To Do: Give prize

        // Re-enable lever
        leverButton.enabled = true;
    }
}

//--------------------------------------------------------------------------
// Extension to implement list shuffling
public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rnd = new System.Random();
        for (var i = 0; i < list.Count; i++)
            list.Swap(i, rnd.Next(i, list.Count));
    }

    public static void Swap<T>(this IList<T> list, int i, int j)
    {
        var temp = list[i];
        list[i] = list[j];
        list[j] = temp;
    }
}