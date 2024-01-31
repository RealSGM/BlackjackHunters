using UnityEngine;

public class TroopIndividual : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    public Troop troop;

    public void SetHealth(float newHealth)
    {
        // get troop max health
        // scale.x = current health / max health
        // position.x = -0.5 * scale.x
    }
}
