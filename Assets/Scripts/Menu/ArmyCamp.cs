using UnityEngine;
using UnityEngine.UI;

public class ArmyCamp : MonoBehaviour
{
    // Temporary Code, will change later

    [SerializeField] GameObject TroopDisplay;

    //--------------------------------------------------------------------------
    public void ExitCamp()
    {
        GameManager.Game.LoadScene("Castle");
        SaveSystem.SaveArmy();
    }
    //--------------------------------------------------------------------------
    private void Start()
    {
        // Initial position of the first troop display
        float position = -140;

        // Adjust the size of the scroll container based on the size of each stats box and the number of troops shown
        RectTransform rT = GameObject.Find("Content").GetComponent<RectTransform>();

        // Expand content box to fit troop count
        GameObject tempTd = Instantiate(TroopDisplay);
        float boxHeight = tempTd.transform.GetComponent<RectTransform>().sizeDelta.y;
        float gapWidth = 0.00f;
        float spacing = (boxHeight + gapWidth);
        rT.sizeDelta = new Vector2(rT.sizeDelta.x, (316.69f) * DataManager.Data.Troops.Count);
        Destroy(tempTd);

        foreach (Troop troop in DataManager.Data.Troops)
        {
            // Instantiate troop display under the scroll view content box
            GameObject td = Instantiate(TroopDisplay);
            td.transform.SetParent(GameObject.Find("Content").transform, false);
            td.transform.position = GameObject.Find("Content").transform.position + new Vector3(0, position, 0);

            //Find and set the sprite of the troop
            int index = DataManager.Data.troopIndexes.IndexOf(troop.Type);
            td.transform.Find("Icon").GetComponentInChildren<Image>().sprite = DataManager.Data.troopSprites[index];

            // Update text labels with the troop's stats
            td.transform.Find("Name").GetComponentInChildren<Text>().text = "Name: " + troop.Name;
            td.transform.Find("Attack").GetComponentInChildren<Text>().text = "Attack: " + troop.Attack;
            td.transform.Find("Defence").GetComponentInChildren<Text>().text = "Defence: " + troop.Defence;
            td.transform.Find("Health").GetComponentInChildren<Text>().text = "Health: " + troop.Health;
            td.transform.Find("Type").GetComponentInChildren<Text>().text = "Type: " + troop.Type;
            td.transform.Find("Level").GetComponentInChildren<Text>().text = "Level: " + troop.Level;

            if (troop.IsSelected)
            {           
                Button b = td.GetComponent<Button>();
                ChangeButtonColor(b, Color.grey);
            }

            // Decrement counter
            position -= spacing;      
        }
    }

    //--------------------------------------------------------------------------
    public static void TroopSelected(GameObject obj)
    {        
        // Retrieves name of the troop selected
        string _name = obj.transform.Find("Name").GetComponent<Text>().text.Replace("Name: ", "");
        Debug.Log(_name);

        // Finds the troop with that name
        foreach (Troop troop in DataManager.Data.Troops)
        {
            if (troop.Name == _name)
            {
                // Checks if troop already selected
                if (troop.IsSelected)
                {
                    // Prince and King cannot be de-selected
                    if (!(troop.Type == "Prince" || troop.Type == "King")) 
                        {
                            // Deselects the troop
                            troop.IsSelected = false;
                            DataManager.Data.selectedTroops.Remove(troop);

                            Button b = obj.GetComponent<Button>();
                            ChangeButtonColor(b, Color.white);

                            SaveSystem.SaveArmy();
                            Debug.Log("Troop has been removed");
                        }
                }

                // Checks if the troop can be selected
                else if (DataManager.Data.selectedTroops.Count < DataManager.Data.maxArmySize)
                {
                    // Selects the troop
                    troop.IsSelected = true;
                    DataManager.Data.selectedTroops.Add(troop);

                    Button b = obj.GetComponent<Button>();
                    ChangeButtonColor(b, Color.grey);

                    SaveSystem.SaveArmy();
                }
            }
        }
    }

    //--------------------------------------------------------------------------
    public static void ChangeButtonColor(Button b, Color color)
    {
        // Change the target button's different color blocks to the respective color
        ColorBlock cb = b.colors;
        cb.normalColor = color;
        cb.selectedColor = color;
        cb.highlightedColor = color;
        b.colors = cb;
    }
}
