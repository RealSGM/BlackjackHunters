using UnityEngine;
using System.Collections.Generic;

public class TroopManager : MonoBehaviour
{
    /* Stores troop data
     * 
    */

    private List<string> types = new List<string>() { "Archer", "Infantry", "Knight", "Prince", "Spearman" };
    private List<int> attacks = new List<int>() { };
    private List<int> defences = new List<int>() { };
    private List<int> healths = new List<int>() { };
    private List<int> rangeCoeffs = new List<int>() { };
    private List<int> upgradeMultiplier = new List<int>() { };
    private List<string> upgradedName = new List<string>() { };


    public static TroopManager TM { get; private set; }

    //--------------------------------------------------------------------------
    private void Awake()
    {
        if (TM == null)
        {
            // Singleton Setup
            TM = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy extra instances of singleton
            Destroy(gameObject);
        }
    }

    //--------------------------------------------------------------------------
    public static void CreatePrince()
    {
        Troop Prince = new Troop(DataManager.Data.playerName, "Prince", 10, 40, 16, 0, 0.5f, true, false);
        DataManager.Data.Troops.Add(Prince);
        DataManager.Data.selectedTroops.Add(Prince);
        SaveSystem.SaveArmy();
    }

    //--------------------------------------------------------------------------
    public static void CreateTroop(string _type, int _attack, int _defence, int _health, int _level, float _rangeCoeff, bool _isSelected, bool _statsUpgraded)
    {
        // Generate random name
        string _name = DataManager.Data.RandomString(5);

        Troop newTroop = new Troop(_name, _type, _attack, _defence, _health, _level, _rangeCoeff, _isSelected, _statsUpgraded);
        DataManager.Data.Troops.Add(newTroop);
        SaveSystem.SaveArmy();
    }

}
