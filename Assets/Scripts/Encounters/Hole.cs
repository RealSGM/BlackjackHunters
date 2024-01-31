using UnityEngine;

public class Hole : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(GameManager.Game.exitEncounterPrefab, GameObject.Find("Canvas").transform);
    }
}
