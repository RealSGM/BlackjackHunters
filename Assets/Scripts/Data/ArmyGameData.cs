using System.Collections.Generic;

[System.Serializable]
public class ArmyGameData
{
    // Class containing army data which will get stored
    public List<Troop> Troops;

    public ArmyGameData()
    {
        Troops = DataManager.Data.Troops;
    }
}
