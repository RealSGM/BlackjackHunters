using UnityEngine.UI;
using UnityEngine;

public class CastleMenu : MonoBehaviour
{
    /* Castle Scene - make area where player prepares to explore
     * 
     *  - Add input validation for name creation
    */ 

    [SerializeField] GameObject castle;
    [SerializeField] Transform canvas;
    [SerializeField] GameObject newName;

    //--------------------------------------------------------------------------
    public void SwitchScene(string newScene)
    {
        GameManager.Game.LoadScene(newScene);   
    }

    //--------------------------------------------------------------------------
    private void Start()
    {
        SaveSystem.SaveArmy();

        if (DataManager.Data.newPlayer)
        {
            ToggleMenus(newName);
        }
        else
        {
            ToggleMenus(castle);
            Destroy(newName);
        }
    }

    //--------------------------------------------------------------------------
    public void OnConfirmNameCreation()
    {
        // Retrieve input field text, save the player name back into the binary file
        string playerName =  GameObject.Find("InputField").GetComponent<InputField>().text;
        DataManager.Data.playerName = playerName;
        DataManager.Data.newPlayer = false;
        SaveSystem.SaveGame();
        if (!ArmyManager.HasTroop("Prince", "Knight"))
        {
            TroopManager.CreatePrince();
        }

        ToggleMenus(castle);
        Destroy(newName);
    }

    //--------------------------------------------------------------------------
    private void ToggleMenus(GameObject activeMenu)
    {
        castle.SetActive(false);
        newName.SetActive(false);
        activeMenu.SetActive(true);
    }
}
