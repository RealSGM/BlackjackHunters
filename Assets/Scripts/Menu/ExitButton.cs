using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void Exit()
    {
        Destroy(GameObject.Find("Main Camera").GetComponent<Explore>().currentEncounter);
        Destroy(this.gameObject);
        GameObject.Find("Main Camera").GetComponent<Explore>().ContinueGame();
    }
}
