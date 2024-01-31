using UnityEngine;
using System.Linq;

public class ArmyManager : MonoBehaviour
{
    /* Singleton used for handling the army, both inactive and active
     * 
     * - Create proper troop generation + stat creation
    */

    public static ArmyManager Army { get; private set; }

    //--------------------------------------------------------------------------
    private void Awake()
    {
        if (Army == null)
        {
            // Singleton Setup
            Army = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy extra instances of singleton
            Destroy(gameObject);
        }
    }

    //--------------------------------------------------------------------------
    private void Start()
    {
        LoadArmy();
    }

    //--------------------------------------------------------------------------
    public static void LoadArmy()
    {
        if (DataManager.Data.Troops.Count == 0)
        {
            ArmyGameData data = SaveSystem.LoadArmy();
            if (data != null)
            {
                DataManager.Data.Troops = data.Troops;
            }

            foreach (Troop troop in DataManager.Data.Troops)
            {
                if (troop.IsSelected)
                {
                    DataManager.Data.selectedTroops.Add(troop);
                }
            }
        }
    }

    //--------------------------------------------------------------------------
    public static bool HasTroop(params string[] troopType)
    {
        // Linear search
        foreach(Troop troop in DataManager.Data.Troops)
        {
            if (troopType.Contains(troop.Type)) return true;
        }
        return false;
    }

    //--------------------------------------------------------------------------
    public static Troop GetTroopByName(string troopName)
    {
        foreach(Troop troop in DataManager.Data.Troops)
        {
            if (troop.Name == troopName) return troop;
        }
        return null;
    }
}
