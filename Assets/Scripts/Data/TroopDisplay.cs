using UnityEngine;

public class TroopDisplay : MonoBehaviour
{
    // On-click button function of the troop display prefab
    public void TroopSelected()
    {
        ArmyCamp.TroopSelected(this.gameObject);
    }
}
